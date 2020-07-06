# azure-servicebus

Azure Service Bus examples

---

## Links

- [Intro](https://azure.microsoft.com/en-us/services/service-bus/)
- [Overview](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-messaging-overview)
- [Docs](https://docs.microsoft.com/en-us/azure/service-bus-messaging/)
- [Messaging Units](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-premium-messaging#messaging-unit---how-many-are-needed)
- [Pricing](https://azure.microsoft.com/en-us/pricing/details/service-bus/) 
- [JMS & AMQP](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-java-how-to-use-jms-api-amqp)
- [Geo DR](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-geo-dr)

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
Sending message: Message 1 at 2020-07-06T12:25:15.2790440Z 1594038315
Sending message: Message 2 at 2020-07-06T12:25:16.1856530Z 1594038316
Sending message: Message 3 at 2020-07-06T12:25:16.2510760Z 1594038316
Sending message: Message 4 at 2020-07-06T12:25:16.3112040Z 1594038316
Sending message: Message 5 at 2020-07-06T12:25:16.3824650Z 1594038316
Sending message: Message 6 at 2020-07-06T12:25:16.4440420Z 1594038316
Sending message: Message 7 at 2020-07-06T12:25:16.5076570Z 1594038316
Sending message: Message 8 at 2020-07-06T12:25:16.5701890Z 1594038316
Sending message: Message 9 at 2020-07-06T12:25:16.6425320Z 1594038316
Sending message: Message 10 at 2020-07-06T12:25:16.7002890Z 1594038316
Sending message: Message 11 at 2020-07-06T12:25:16.7807360Z 1594038316
Sending message: Message 12 at 2020-07-06T12:25:16.8499780Z 1594038316
Sending message: Message 13 at 2020-07-06T12:25:16.9162180Z 1594038316 TROUBLE!
Sending message: Message 14 at 2020-07-06T12:25:18.3533100Z 1594038318
Sending message: Message 15 at 2020-07-06T12:25:18.4130720Z 1594038318
Sending message: Message 16 at 2020-07-06T12:25:18.4832860Z 1594038318
Sending message: Message 17 at 2020-07-06T12:25:18.5563040Z 1594038318
Sending message: Message 18 at 2020-07-06T12:25:18.6098420Z 1594038318
Sending message: Message 19 at 2020-07-06T12:25:18.6636990Z 1594038318
Sending message: Message 20 at 2020-07-06T12:25:18.7213000Z 1594038318
Hit Enter to continue...
```

### Receive the Messages, reprocessing #13 with Exponential Backoff

```
$ dotnet run receive 30
...
Received message: seq: 556 dc: 1 enq: 7/6/2020 12:25:16 PM text: Message 1 at 2020-07-06T12:25:15.2790440Z 1594038315
Received message: seq: 557 dc: 1 enq: 7/6/2020 12:25:16 PM text: Message 2 at 2020-07-06T12:25:16.1856530Z 1594038316
Received message: seq: 558 dc: 1 enq: 7/6/2020 12:25:16 PM text: Message 3 at 2020-07-06T12:25:16.2510760Z 1594038316
Received message: seq: 559 dc: 1 enq: 7/6/2020 12:25:16 PM text: Message 4 at 2020-07-06T12:25:16.3112040Z 1594038316
Received message: seq: 560 dc: 1 enq: 7/6/2020 12:25:16 PM text: Message 5 at 2020-07-06T12:25:16.3824650Z 1594038316
Received message: seq: 561 dc: 1 enq: 7/6/2020 12:25:16 PM text: Message 6 at 2020-07-06T12:25:16.4440420Z 1594038316
Received message: seq: 562 dc: 1 enq: 7/6/2020 12:25:16 PM text: Message 7 at 2020-07-06T12:25:16.5076570Z 1594038316
Received message: seq: 563 dc: 1 enq: 7/6/2020 12:25:16 PM text: Message 8 at 2020-07-06T12:25:16.5701890Z 1594038316
Received message: seq: 564 dc: 1 enq: 7/6/2020 12:25:16 PM text: Message 9 at 2020-07-06T12:25:16.6425320Z 1594038316
Received message: seq: 565 dc: 1 enq: 7/6/2020 12:25:16 PM text: Message 10 at 2020-07-06T12:25:16.7002890Z 1594038316
Received message: seq: 566 dc: 1 enq: 7/6/2020 12:25:16 PM text: Message 11 at 2020-07-06T12:25:16.7807360Z 1594038316
Received message: seq: 567 dc: 1 enq: 7/6/2020 12:25:16 PM text: Message 12 at 2020-07-06T12:25:16.8499780Z 1594038316
Received message: seq: 568 dc: 1 enq: 7/6/2020 12:25:18 PM text: Message 13 at 2020-07-06T12:25:16.9162180Z 1594038316 TROUBLE!
Encountered unlucky message 13!
Received message: seq: 569 dc: 1 enq: 7/6/2020 12:25:18 PM text: Message 14 at 2020-07-06T12:25:18.3533100Z 1594038318
Received message: seq: 570 dc: 1 enq: 7/6/2020 12:25:18 PM text: Message 15 at 2020-07-06T12:25:18.4130720Z 1594038318
Received message: seq: 571 dc: 1 enq: 7/6/2020 12:25:18 PM text: Message 16 at 2020-07-06T12:25:18.4832860Z 1594038318
Received message: seq: 572 dc: 1 enq: 7/6/2020 12:25:18 PM text: Message 17 at 2020-07-06T12:25:18.5563040Z 1594038318
Received message: seq: 573 dc: 1 enq: 7/6/2020 12:25:18 PM text: Message 18 at 2020-07-06T12:25:18.6098420Z 1594038318
Received message: seq: 574 dc: 1 enq: 7/6/2020 12:25:18 PM text: Message 19 at 2020-07-06T12:25:18.6636990Z 1594038318
Received message: seq: 575 dc: 1 enq: 7/6/2020 12:25:18 PM text: Message 20 at 2020-07-06T12:25:18.7213000Z 1594038318
Received message: seq: 568 dc: 2 enq: 7/6/2020 12:25:18 PM text: Message 13 at 2020-07-06T12:25:16.9162180Z 1594038316 TROUBLE!
Encountered unlucky message 13!
Received message: seq: 568 dc: 3 enq: 7/6/2020 12:25:18 PM text: Message 13 at 2020-07-06T12:25:16.9162180Z 1594038316 TROUBLE!
Encountered unlucky message 13!
Ok, I'll finally process you message 13
```

#### Setting the Exponential Backoff when Reading

See file **Program.cs** in this repo.

```
    queueClient = new QueueClient(connString, queueName, receiveMode,  GetRetryPolicy());

    ...

    private static RetryExponential GetRetryPolicy() {
        return new RetryExponential(
            minimumBackoff: TimeSpan.FromSeconds(2),
            maximumBackoff: TimeSpan.FromSeconds(30),
            maximumRetryCount: 5);
    }
```
