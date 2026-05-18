# Notification delivery API

A small HTTP API that manages and delivers notifications across email, SMS, and push channels.

## Install

```
dotnet restore
```

## Run

```
dotnet run
```

Starts the HTTP server on port 3000.

NOTE: The server pre-populates in-memory storage with a few sample notifications on startup.

## Endpoints

### POST /notifications

Create a notification.

```
curl -X POST http://localhost:3000/notifications \
  -H "Content-Type: application/json" \
  -d '{"message":"Hello","targetChannels":[{"type":"email","value":"user@example.com"}]}'
```

### GET /notifications

List all notifications.

```
curl http://localhost:3000/notifications
```

### GET /notifications/:id

Fetch a single notification by id. Returns 404 if not found.

```
curl http://localhost:3000/notifications/1
```

### PUT /notifications/:id

Update a notification. Returns 404 if not found.

```
curl -X PUT http://localhost:3000/notifications/1 \
  -H "Content-Type: application/json" \
  -d '{"message":"Updated message"}'
```

### POST /notifications/:id/send

Send a single notification. Returns 404 if not found.

```
curl -X POST http://localhost:3000/notifications/1/send
```

### POST /notifications/send-bulk

Send all pending notifications.

```
curl -X POST http://localhost:3000/notifications/send-bulk
```

------------------------------------------------

## hi, my name is Leah☺️

**1)** Added Swagger - to keep track of the code in an organized and clear way.

**2)** Each message was always marked as Sent - we didn't see a real result.

-Treated so that they knew what the correct message was.

**3)** Only sends one channel - if all else fails

-I gave a pass on all channels.

**4)** I added 3 attempts to send a message to simulate a real system. It won't crash after one attempt to send.

```
Used to fix the code in: ChatGPT

Translation: TranslateGoogle
```
## Thank you very much.

#
