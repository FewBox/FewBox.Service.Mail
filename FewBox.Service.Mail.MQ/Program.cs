using System;
using FewBox.Service.Mail.Model.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FewBox.Service.Mail.MQ
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            Startup startup = new Startup();
            startup.ConfigureServices(services);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
            logger.LogInformation("Wellcome to use FewBox Mail MQ!");

            // Get Service and call method
            var mqService = serviceProvider.GetService<IScopedMQService>();
            mqService.Process();
            logger.LogInformation("Press any key to exit!");
            Console.ReadKey();
        }
    }
}
