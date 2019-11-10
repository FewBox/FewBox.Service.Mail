using System;
using System.Linq;
using FewBox.Core.Utility.Formatter;
using FewBox.Core.Web.Dto;
using FewBox.Core.Web.Filter;
using FewBox.Service.Mail.Configs;
using FewBox.Service.Mail.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FewBox.Service.Mail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : BaseMessageController
    {
        private TemplateConfig TemplateConfig { get; set; }
        private NotificationTemplateConfig NotificationTemplateConfig { get; set; }
        public NotificationController(SmtpConfig smtpConfig, TemplateConfig templateConfig, NotificationTemplateConfig notificationTemplateConfig) : base(smtpConfig)
        {
            this.TemplateConfig = templateConfig;
            this.NotificationTemplateConfig = notificationTemplateConfig;
        }

        [HttpPost]
        [Trace]
        public NotificationResponseDto Send(NotificationRequestDto notificationRequest)
        {
            string subject = String.Format(this.NotificationTemplateConfig.SubjectWapper, notificationRequest.Name);
            string body = String.Format(Base64Utility.Deserialize(this.NotificationTemplateConfig.BodyWapper), notificationRequest.Param);
            this.SendMessage(this.TemplateConfig.FromAddress, this.TemplateConfig.FromDisplayName, notificationRequest.ToAddresses != null ? notificationRequest.ToAddresses : this.TemplateConfig.ToAddresses, this.TemplateConfig.CCAddresses, this.TemplateConfig.BCCAddresses,
            this.TemplateConfig.ReplyToAddresses, this.TemplateConfig.Headers != null ? this.TemplateConfig.Headers.Select(h => new HeaderDto { Name = h.Name, Value = h.Value }).ToList() : null, subject, body, true);
            return new NotificationResponseDto();
        }
    }
}