using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FewBox.Service.Mail.MQ
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection().AddLogging();
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
            logger.LogInformation("Wellcome to use FewBox Mail MQ!");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return new HostBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                Startup startup = new Startup();
                startup.ConfigureServices(services);
            });
        }
    }
}
