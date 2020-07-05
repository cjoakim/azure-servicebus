# azure-servicebus

Azure Service Bus examples

---

## .Net Core

This sample program implements **Exponential Backoff** using SDK class **RetryExponential**.

See https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-get-started-with-queues

### Create and Compile the Console App

```
$ dotnet new console -o ServiceBusConsoleApp
$ cd ServiceBusConsoleApp
$ dotnet add package Microsoft.Azure.ServiceBus --version 4.1.3
$ dotnet build
```

### Put 20 messages on the Queue

```
$ dotnet run send 20
...
Sending message: Message 1 at 2020-07-05T20:22:28.0796110Z 1593980548
Sending message: Message 2 at 2020-07-05T20:22:29.0291960Z 1593980549
Sending message: Message 3 at 2020-07-05T20:22:29.0994390Z 1593980549
Sending message: Message 4 at 2020-07-05T20:22:29.1567030Z 1593980549
Sending message: Message 5 at 2020-07-05T20:22:29.2291670Z 1593980549
Sending message: Message 6 at 2020-07-05T20:22:29.3145900Z 1593980549
Sending message: Message 7 at 2020-07-05T20:22:29.3819960Z 1593980549
Sending message: Message 8 at 2020-07-05T20:22:29.4635860Z 1593980549
Sending message: Message 9 at 2020-07-05T20:22:29.5342270Z 1593980549
Sending message: Message 10 at 2020-07-05T20:22:29.5929440Z 1593980549
Sending message: Message 11 at 2020-07-05T20:22:29.6594810Z 1593980549
Sending message: Message 12 at 2020-07-05T20:22:29.7145940Z 1593980549
Sending message: Message 13 at 2020-07-05T20:22:29.7721910Z 1593980549   <-- this one is trouble, 13
Sending message: Message 14 at 2020-07-05T20:22:29.8309720Z 1593980549
Sending message: Message 15 at 2020-07-05T20:22:29.8896410Z 1593980549
Sending message: Message 16 at 2020-07-05T20:22:29.9537500Z 1593980549
Sending message: Message 17 at 2020-07-05T20:22:30.0072720Z 1593980550
Sending message: Message 18 at 2020-07-05T20:22:30.1095070Z 1593980550
Sending message: Message 19 at 2020-07-05T20:22:30.1840280Z 1593980550
Sending message: Message 20 at 2020-07-05T20:22:30.2548960Z 1593980550
Hit Enter to continue...
```

### Receive up to 14 messages from the Queue

```
$ dotnet run receive 14
Received message: seq: 449 dc: 1 enq: 7/5/2020 8:22:28 PM text: Message 1 at 2020-07-05T20:22:28.0796110Z 1593980548
Received message: seq: 450 dc: 1 enq: 7/5/2020 8:22:29 PM text: Message 2 at 2020-07-05T20:22:29.0291960Z 1593980549
Received message: seq: 451 dc: 1 enq: 7/5/2020 8:22:29 PM text: Message 3 at 2020-07-05T20:22:29.0994390Z 1593980549
Received message: seq: 452 dc: 1 enq: 7/5/2020 8:22:29 PM text: Message 4 at 2020-07-05T20:22:29.1567030Z 1593980549
Received message: seq: 453 dc: 1 enq: 7/5/2020 8:22:29 PM text: Message 5 at 2020-07-05T20:22:29.2291670Z 1593980549
Received message: seq: 454 dc: 1 enq: 7/5/2020 8:22:29 PM text: Message 6 at 2020-07-05T20:22:29.3145900Z 1593980549
Received message: seq: 455 dc: 1 enq: 7/5/2020 8:22:29 PM text: Message 7 at 2020-07-05T20:22:29.3819960Z 1593980549
Received message: seq: 456 dc: 1 enq: 7/5/2020 8:22:29 PM text: Message 8 at 2020-07-05T20:22:29.4635860Z 1593980549
Received message: seq: 457 dc: 1 enq: 7/5/2020 8:22:29 PM text: Message 9 at 2020-07-05T20:22:29.5342270Z 1593980549
Received message: seq: 458 dc: 1 enq: 7/5/2020 8:22:29 PM text: Message 10 at 2020-07-05T20:22:29.5929440Z 1593980549
Received message: seq: 459 dc: 1 enq: 7/5/2020 8:22:29 PM text: Message 11 at 2020-07-05T20:22:29.6594810Z 1593980549
Received message: seq: 460 dc: 1 enq: 7/5/2020 8:22:29 PM text: Message 12 at 2020-07-05T20:22:29.7145940Z 1593980549
Received message: seq: 461 dc: 1 enq: 7/5/2020 8:22:29 PM text: Message 13 at 2020-07-05T20:22:29.7721910Z 1593980549
Encountered unlucky message 13!
Received message: seq: 462 dc: 1 enq: 7/5/2020 8:22:29 PM text: Message 14 at 2020-07-05T20:22:29.8309720Z 1593980549
```

### Receive up to 14 messages from the Queue, again

Message 13 isn't ready to be read yet per RetryExponential policy in the Receiver.

```
$ dotnet run receive 14
Received message: seq: 463 dc: 2 enq: 7/5/2020 8:22:29 PM text: Message 15 at 2020-07-05T20:22:29.8896410Z 1593980549
Received message: seq: 464 dc: 1 enq: 7/5/2020 8:22:29 PM text: Message 16 at 2020-07-05T20:22:29.9537500Z 1593980549
Received message: seq: 465 dc: 1 enq: 7/5/2020 8:22:30 PM text: Message 17 at 2020-07-05T20:22:30.0072720Z 1593980550
Received message: seq: 466 dc: 1 enq: 7/5/2020 8:22:30 PM text: Message 18 at 2020-07-05T20:22:30.1095070Z 1593980550
Received message: seq: 467 dc: 1 enq: 7/5/2020 8:22:30 PM text: Message 19 at 2020-07-05T20:22:30.1840280Z 1593980550
Received message: seq: 468 dc: 1 enq: 7/5/2020 8:22:30 PM text: Message 20 at 2020-07-05T20:22:30.2548960Z 1593980550
```

### Receive up to 14 messages from the Queue, again, and again

```
$ dotnet run receive 14
Received message: seq: 461 dc: 2 enq: 7/5/2020 8:22:29 PM text: Message 13 at 2020-07-05T20:22:29.7721910Z 1593980549
Encountered unlucky message 13!
```

Finally, Message 13 can be read.

```
$ dotnet run receive 14
Encountered unlucky message 13!
Ok, I'll finally process you message 13
```
