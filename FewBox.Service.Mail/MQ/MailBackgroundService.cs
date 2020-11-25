using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FewBox.Service.Mail.Model.Services;
using System;

namespace FewBox.Service.Mail.MQ
{
    public class MailBackgroundService : BackgroundService
    {
        private IServiceProvider Services { get; }
        private ILogger<MailBackgroundService> Logger { get; set; }
        public MailBackgroundService(IServiceProvider services, ILogger<MailBackgroundService> logger)
        {
            this.Services = services;
            this.Logger = logger;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            this.Logger.LogDebug($"Mail MQ background task is starting.");

            stoppingToken.Register(() =>
                this.Logger.LogDebug($"Mail MQ background task is stopping."));

            using (var scope = this.Services.CreateScope())
            {
                var scopedMQService = 
                    scope.ServiceProvider
                        .GetRequiredService<IScopedMQService>();
                scopedMQService.Process();
            }

            this.Logger.LogDebug($"Mail MQ background task is stopping.");
            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            this.Logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");
            await base.StopAsync(stoppingToken);
        }
    }
}