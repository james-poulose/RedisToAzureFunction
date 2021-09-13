using Microsoft.Extensions.Hosting;

namespace RedisToAzureFunction.Function
{
    public class Program
    {
        public static void Main()
        {
            IHost host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .Build();

            host.Run();
        }
    }
}