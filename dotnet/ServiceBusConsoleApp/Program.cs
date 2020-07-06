using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

// Example C# .Net Core Azure Service Bus send and receive client console program.
// Chris Joakim, Microsoft, 2020/07/06

namespace ServiceBusConsoleApp {
    
    class Program {

        private static IQueueClient queueClient;  // Microsoft.Azure.ServiceBus.QueueClient
        private static string connString = Environment.GetEnvironmentVariable("AZURE_SERVICEBUS_CONN_STRING");
        private static string queueName  = Environment.GetEnvironmentVariable("AZURE_SERVICEBUS_QUEUE");
        private static int maxReceiveCount = 0;
        private static int receiveCount    = 0;

        public static async Task Main(string[] args) {
            if (args.Length > 0) {
                string function = args[0].ToLower();

                if (function == "send") {
                    int sendCount = Int32.Parse(args[1]);
                    Console.WriteLine("send:       " + sendCount);
                    Console.WriteLine("connString: " + connString);
                    Console.WriteLine("queueName:  " + queueName);
                    queueClient = new QueueClient(connString, queueName);
                    Console.WriteLine("queueClient: " + queueClient.ToString());
                    await SendMessagesAsync(sendCount);
                    await endProgram();
                }
                if (function == "receive") {
                    maxReceiveCount = Int32.Parse(args[1]);
                    Console.WriteLine("receive:    " + maxReceiveCount);
                    Console.WriteLine("connString: " + connString);
                    Console.WriteLine("queueName:  " + queueName);
                    ReceiveMode receiveMode = ReceiveMode.PeekLock;
                    queueClient = new QueueClient(connString, queueName, receiveMode,  GetRetryPolicy());
                    Console.WriteLine("queueClient: " + queueClient.ToString());
                    RegisterOnMessageHandlerAndReceiveMessages();
                    await endProgram();
                }
            }
            else {
                Console.WriteLine("Invalid program args; should specify function and message count");
                Console.WriteLine("dotnet run send 10");
                Console.WriteLine("dotnet run receive 5");
            }
        }

        private static RetryExponential GetRetryPolicy() {
            // See https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.servicebus.retryexponential?view=azure-dotnet
            return new RetryExponential(
                minimumBackoff: TimeSpan.FromSeconds(2),
                maximumBackoff: TimeSpan.FromSeconds(30),
                maximumRetryCount: 5);
        }

        static async Task SendMessagesAsync(int sendCount) {
            try {
                for (var i = 1; i <= sendCount; i++) {
                    // Create a new Message and put it on the queue
                    TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1);
                    int epoch = (int) ts.TotalSeconds;
                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ");
                    string msg = "";
                    if (i == 13) { msg = "TROUBLE!"; }
                    string messageBody = $"Message {i} at {timestamp} {epoch} {msg}";
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                    Console.WriteLine($"Sending message: {messageBody}");
                    await queueClient.SendAsync(message);
                }
            }
            catch (Exception exception) {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }

        static void RegisterOnMessageHandlerAndReceiveMessages() {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler) {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
            Console.WriteLine("RegisterOnMessageHandlerAndReceiveMessages start...");
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
            Console.WriteLine("RegisterOnMessageHandlerAndReceiveMessages registered...");
        }

        static async Task ProcessMessagesAsync(Message message, CancellationToken token) {
            receiveCount++;
            if (receiveCount > maxReceiveCount) {
                await queueClient.AbandonAsync(message.SystemProperties.LockToken);
                await queueClient.CloseAsync();
                queueClient = null;
                Console.WriteLine("max messages read");
            }
            else {
                // See https://docs.microsoft.com/en-us/rest/api/servicebus/message-headers-and-properties#message-properties
                long seq = message.SystemProperties.SequenceNumber;
                int  dc  = message.SystemProperties.DeliveryCount;
                string enq = message.SystemProperties.EnqueuedTimeUtc.ToString();
                string messageText = Encoding.UTF8.GetString(message.Body);
                Console.WriteLine($"Received message: seq: {seq} dc: {dc} enq: {enq} text: {messageText}");
                bool abandoned = false;

                if (messageText.Contains(" 13 ")) {
                    Console.WriteLine("Encountered unlucky message 13!");
                    if (dc < 3) {
                        abandoned = true;
                        //await queueClient.AbandonAsync(message.SystemProperties.LockToken);  <-- no need to do this
                    }
                    else {
                        Console.WriteLine("Ok, I'll finally process you message 13");  
                    }
                }
                if (!abandoned) {
                    // Complete the message so that it is not received again.
                    // This can be done only if the queueClient is created in ReceiveMode.PeekLock mode (the default).
                    await queueClient.CompleteAsync(message.SystemProperties.LockToken);
                }
            }
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs) {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

        static async Task endProgram() {
            Console.WriteLine("Hit Enter to continue...");
            Console.ReadKey();
            if (queueClient != null) {
                await queueClient.CloseAsync();
                Console.WriteLine("QueueClient closed");
            }  
        }
    }
}
