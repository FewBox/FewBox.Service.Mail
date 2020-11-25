﻿using System.Collections.Generic;
using FewBox.Core.Web.Extension;
using FewBox.SDK.Extension;
using FewBox.Service.Mail.Domain.Services;
using FewBox.Service.Mail.Model.Configs;
using FewBox.Service.Mail.Model.Services;
using FewBox.Service.Mail.MQ;
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
            services.AddFewBoxSDK(FewBoxIntegrationType.MessageQueue);
            services.AddFewBox(FewBoxDBType.None, FewBoxAuthType.Payload, new ApiVersion(1, 0, "alpha1"));
            var templateConfig = this.Configuration.GetSection("TemplateConfig").Get<TemplateConfig>();
            services.AddSingleton(templateConfig);
            var notificationTemplateConfig = this.Configuration.GetSection("NotificationTemplateConfig").Get<NotificationTemplateConfig>();
            services.AddSingleton(notificationTemplateConfig);
            var smtpConfig = this.Configuration.GetSection("SmtpConfig").Get<SmtpConfig>();
            services.AddSingleton(smtpConfig);
            services.AddScoped<ISMTPService, SMTPService>();
            services.AddScoped<IScopedMQService, ScopedMQService>();
            // Background MQ Service
            services.AddHostedService<MailBackgroundService>();
            // Used for Swagger Open Api Document.
            services.AddOpenApiDocument(config =>
            {
                this.InitAspNetCoreOpenApiDocumentGeneratorSettings(config, "v1", new[] { "1-alpha1", "1-beta1", "1" }, "v1");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseFewBox(new List<string> { "/swagger/v1/swagger.json" });
        }

        private void InitAspNetCoreOpenApiDocumentGeneratorSettings(AspNetCoreOpenApiDocumentGeneratorSettings config, string documentName, string[] apiGroupNames, string documentVersion)
        {
            config.DocumentName = documentName;
            config.ApiGroupNames = apiGroupNames;
            config.PostProcess = document =>
            {
                this.InitDocumentInfo(document, documentVersion);
            };
            config.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT"));
            config.DocumentProcessors.Add(
                new SecurityDefinitionAppender("JWT", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    Description = "Bearer [Token]",
                    In = OpenApiSecurityApiKeyLocation.Header
                })
            );
        }

        private void InitDocumentInfo(OpenApiDocument document, string version)
        {
            document.Info.Version = version;
            document.Info.Title = "FewBox Auth Api";
            document.Info.Description = "FewBox Auth, for more information please visit the 'https://fewbox.com'";
            document.Info.TermsOfService = "https://fewbox.com/terms";
            document.Info.Contact = new OpenApiContact
            {
                Name = "FewBox",
                Email = "support@fewbox.com",
                Url = "https://fewbox.com/support"
            };
            document.Info.License = new OpenApiLicense
            {
                Name = "Use under license",
                Url = "https://fewbox.com/license"
            };
        }
    }
}