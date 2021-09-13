using System;
using System.Threading;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace RedisToAzureFunction.Function
{
    public static class TimerFunction
    {
        [Function("TimerFunction")]
        public static void Run([TimerTrigger("*/5 * * * * *")] CustomType customType, FunctionContext context)
        {
            ILogger logger = context.GetLogger("TimerFunction");
            ReadFromQueue(logger);
        }

        private static void ReadFromQueue(ILogger logger)
        {
            string? serverWithPort = Environment.GetEnvironmentVariable("Server");

            if (string.IsNullOrEmpty(serverWithPort))
            {
                logger.LogInformation("Invalid redis instance name");
                return;
            }

            string? queueName = Environment.GetEnvironmentVariable("QueueName");
            if (string.IsNullOrEmpty(queueName))
            {
                logger.LogInformation("Invalid redis instance name");
                return;
            }

            logger.LogInformation($"Subscribing to: {serverWithPort}, {queueName}");

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(serverWithPort);
            ISubscriber subscriber = redis.GetSubscriber();
            
            subscriber.Subscribe(new RedisChannel(queueName, RedisChannel.PatternMode.Pattern), (channel, message) => OnMessageFound(message));
            Thread.Sleep(2000);
        }

        private static void OnMessageFound(RedisValue message)
        {
            Console.WriteLine("JP: msg found: " + message);
        }
    }

    public class CustomType
    {
        public bool Enabled { get; set; }
    }
}