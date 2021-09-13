using System;
using System.Configuration;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using StackExchange.Redis;

namespace RedisToAzureFunction.Cli
{
    internal class Program
    {
        private static Options _options;
        private static string _serverWithPort;
        private static string _queueName;

        public static void SendMessageToQueue()
        {
            Console.WriteLine($"Connecting to instance: {_serverWithPort} - {_queueName}");
            Console.WriteLine($"Message count: {_options.NumberOfMessages}");
            Console.WriteLine($"Interval: {_options.IntervalInSeconds}");

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(_serverWithPort);
            ISubscriber subscriber = redis.GetSubscriber();

            for (var i = 0; i < _options.NumberOfMessages; i++)
            {
                Thread.Sleep(_options.IntervalInSeconds * 1000);
                var message = new Message();
                subscriber.Publish(_queueName, JsonSerializer.Serialize(message));
                Console.WriteLine($"Sent {i + 1} of {_options.NumberOfMessages} : {message.Data} {message.Created.ToLongTimeString()}");
            }
        }

        private static void Main(string[] args)
        {
            ParserResult<Options> parserResult = Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o => { _options = o; });
            if (!Validate(parserResult))
            {
                return;
            }

            if (_options.Mode == "write")
            {
                var task = new Task(SendMessageToQueue);
                task.Start();
                Task.WaitAll(task);
            }
            else if (_options.Mode == "read")
            {
                ReadFromQueue();
            }

            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }

        private static void ReadFromQueue()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(_serverWithPort);
            ISubscriber subscriber = redis.GetSubscriber();
            subscriber.Subscribe(_queueName).OnMessage(channelMessage =>
            {
                Console.WriteLine($"JP: {channelMessage.Message}");
            });
        }

        private static bool Validate(ParserResult<Options> parserResult)
        {
            if (parserResult.Tag == ParserResultType.NotParsed)
            {
                Console.WriteLine("Command line parameter parsing failed...");
                return false;
            }

            // Server name
            _serverWithPort = _options.Server;

            if (string.IsNullOrEmpty(_serverWithPort))
            {
                _serverWithPort = ConfigurationManager.AppSettings.Get("Server");
            }

            if (string.IsNullOrEmpty(_serverWithPort))
            {
                Console.WriteLine("Invalid server name");
                return false;
            }

            // Queue name
            _queueName = _options.QueueName;

            if (string.IsNullOrEmpty(_queueName))
            {
                _queueName = ConfigurationManager.AppSettings.Get("QueueName");
            }

            if (string.IsNullOrEmpty(_queueName))
            {
                Console.WriteLine("Invalid queue name");
                return false;
            }

            return true;
        }
    }

    class Message
    {
        public Message()
        {
            Created = DateTime.Now;
            Data = Guid.NewGuid().ToString();
        }

        public DateTime Created { get; set; }
        public object Data { get; set; }
    }

}