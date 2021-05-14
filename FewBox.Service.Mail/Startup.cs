using System.Collections.Generic;
using FewBox.Core.Web.Extension;
using FewBox.SDK.Extension;
using FewBox.SDK.Mail;
using FewBox.Service.Mail.Domain.Services;
using FewBox.Service.Mail.Model.Configs;
using FewBox.Service.Mail.Model.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors.Security;

namespace FewBox.Service.Mail
{
    public class Startup
    {
        private IList<ApiVersionDocument> ApiVersionDocuments = new List<ApiVersionDocument> {
                new ApiVersionDocument{
                    ApiVersion = new ApiVersion(1, 0),
                    IsDefault = true
                },
                new ApiVersionDocument{
                    ApiVersion = new ApiVersion(2, 0, "alpha1")
                },
                new ApiVersionDocument{
                    ApiVersion = new ApiVersion(2, 0, "beta1")
                }
            };

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            this.Configuration = configuration;
            this.HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFewBoxSDK(FewBoxIntegrationType.MessageQueue, FewBoxListenerHostType.Web, FewBoxListenerType.Email);
            services.AddFewBox(this.ApiVersionDocuments, FewBoxDBType.None, FewBoxAuthType.Payload);
            var templateConfig = this.Configuration.GetSection("TemplateConfig").Get<TemplateConfig>();
            services.AddSingleton(templateConfig);
            var notificationTemplateConfig = this.Configuration.GetSection("NotificationTemplateConfig").Get<NotificationTemplateConfig>();
            services.AddSingleton(notificationTemplateConfig);
            var smtpConfig = this.Configuration.GetSection("SmtpConfig").Get<SmtpConfig>();
            services.AddSingleton(smtpConfig);
            services.AddScoped<ISMTPService, SMTPService>();
            services.AddScoped<IMQMailHandler, MQMailHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseFewBox(this.ApiVersionDocuments);
        }
    }
}