using FewBox.SDK.Extension;
using FewBox.SDK.Mail;
using FewBox.Service.Mail.Domain.Services;
using FewBox.Service.Mail.Model.Configs;
using FewBox.Service.Mail.Model.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FewBox.Service.Mail.MQ
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; private set; }

        public Startup()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFewBoxSDK(MQConsumerType.Email);
            var email = this.Configuration.GetSection("Email").Get<Email>();
            services.AddSingleton(email);
            services.AddLogging();
            services.AddSingleton<ISMTPService, SMTPService>();
            services.AddSingleton<IMQMailHandler, MQMailHandler>();
        }
    }
}