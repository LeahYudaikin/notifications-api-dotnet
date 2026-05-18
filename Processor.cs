using NotificationApi.Providers;

namespace NotificationApi;

public class NotificationProcessor
{
    private const int MaxRetries = 3;

    public void SendOne(Notification n)
    {
        n.Status = NotificationStatuses.Processing;
        n.Attempts += 1;
        n.LastAttemptAt = DateTime.Now.ToString("O");

        if (n.TargetChannels == null || n.TargetChannels.Count == 0)
        {
            n.Status = NotificationStatuses.Failed;
            n.LastError = "No target channels";
            return;
        }

        bool anySuccess = false;
        bool anyTemporaryFailure = false;

        foreach (var target in n.TargetChannels)
        {
            var req = new Dictionary<string, string>
            {
                { "recipient", target.Value },
                { "message", n.Message }
            };

            ProviderResponse response = target.Type switch
            {
                "email" => EmailProvider.Send(req),
                "sms" => SmsProvider.Send(req),
                "push" => PushProvider.Send(req),
                _ => new ProviderResponse
                {
                    Result = "InvalidRequest",
                    ErrorCode = "UNKNOWN_CHANNEL",
                    Message = "Unknown channel"
                }
            };

            if (response.Result == "Success")
            {
                anySuccess = true;
            }
            else if (response.Result == "TemporaryFailure")
            {
                anyTemporaryFailure = true;
                n.LastError = response.Message;
            }
            else
            {
                n.LastError = response.Message;
            }
        }

        if (anySuccess)
        {
            n.Status = NotificationStatuses.Sent;
        }
        else if (anyTemporaryFailure)
        {
            if (n.Attempts < MaxRetries)
                n.Status = NotificationStatuses.RetryPending;
            else
                n.Status = NotificationStatuses.Failed;
        }
        else
        {
            n.Status = NotificationStatuses.Failed;
        }
    }

    public void SendAll()
    {
        var pending = Storage.Notifications
            .Where(n => n.Status == NotificationStatuses.Pending ||
                        n.Status == NotificationStatuses.RetryPending)
            .ToList();

        foreach (var n in pending)
        {
            SendOne(n);
        }
    }
}