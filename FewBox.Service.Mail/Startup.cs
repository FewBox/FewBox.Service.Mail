using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FewBox.Service.Mail.Configs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FewBox.Service.Mail
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.Configure<RouteOptions>(options=>{
                options.LowercaseUrls=true;
            });
            services.AddCors();
            var smtpConfig = this.Configuration.GetSection("SmtpConfig").Get<SmtpConfig>();
            services.AddSingleton(smtpConfig);
            services.AddOpenApiDocument(config => {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "FewBox Mail API";
                    document.Info.Description = "A simple ASP.NET Core web API";
                    document.Info.TermsOfService = "https://fewbox.com/terms";
                    document.Info.Contact = new NSwag.SwaggerContact
                    {
                        Name = "XL Pang",
                        Email = "support@fewbox.com",
                        Url = "https://fewbox.com/support"
                    };
                    document.Info.License = new NSwag.SwaggerLicense
                    {
                        Name = "Use under license",
                        Url = "https://raw.githubusercontent.com/FewBox/FewBox.Service.Mail/master/LICENSE"
                    };
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseStaticFiles();
            app.UseSwagger();
            if (env.IsDevelopment() || env.IsStaging())  
            {
                app.UseSwagger();  
                app.UseSwaggerUi3();  
            }
            else
            {
                app.UseReDoc();
            }
            app.UseCors();
        }
    }
}
