using System;
using FewBox.SDK.Core;
using FewBox.SDK.Mail;
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
            var mqListenerService = serviceProvider.GetService<IMQConsumerService<EmailMessage>>();
            var mqMailHandler = serviceProvider.GetService<IMQMailHandler>();
            using (mqListenerService)
            {
                mqListenerService.Start(QueueNames.Mail, mqMailHandler.Handle());
                logger.LogInformation("Press any key to exit!");
                Console.ReadLine();
            }
        }
    }
}
