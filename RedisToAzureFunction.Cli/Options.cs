using CommandLine;

namespace RedisToAzureFunction.Cli
{
    public class Options
    {
        [Option('i', "interval", Required = false, Default = 2, HelpText = "Interval in seconds. How often should the data be written to Redis (Default=2 seconds).")]
        public int IntervalInSeconds { get; set; }

        [Option('n', "number", Required = false, Default = 5, HelpText = "Total number of messages to send (Default=5).")]
        public int NumberOfMessages { get; set; }

        [Option('q', "queue", Required = false, HelpText = "Target queue name for sending messages to.")]
        public string QueueName { get; set; }

        [Option('s', "server", Required = false, HelpText = "Server name with port number.")]
        public string Server { get; set; }

        [Option('m', "mode", Required = false, Default = "write", HelpText = "Mode of operation ('read', 'write'.")]
        public string Mode { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }
    }
}