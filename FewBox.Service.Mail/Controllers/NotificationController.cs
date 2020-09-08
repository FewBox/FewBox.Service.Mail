using FewBox.Core.Web.Dto;
using FewBox.Service.Mail.Configs;
using FewBox.Service.Mail.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FewBox.Service.Mail.Controllers
{
    [ApiController]
    public class NotificationController : BaseMessageController
    {
        public NotificationController(SmtpConfig smtpConfig) : base(smtpConfig)
        {
        }

        [HttpPost]
        public MetaResponseDto Send(NotificationDto notificationDto)
        {
            this.SendMessage(notificationDto.FromAddress, notificationDto.FromDisplayName, notificationDto.ToAddresses, notificationDto.CCAddresses, notificationDto.BCCAddresses,
            notificationDto.ReplyToAddresses, notificationDto.Headers, notificationDto.Subject, notificationDto.Body, notificationDto.IsBodyHtml);
            return new MetaResponseDto();
        }
    }
}
