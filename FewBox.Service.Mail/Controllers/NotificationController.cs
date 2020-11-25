using FewBox.Core.Web.Dto;
using FewBox.Service.Mail.Dtos;
using FewBox.Service.Mail.Model.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FewBox.Service.Mail.Controllers
{
    public class NotificationController : BaseMessageController
    {
        public NotificationController(ISMTPService smtpService) : base(smtpService)
        {
        }

        [HttpPost]
        public MetaResponseDto Send(NotificationDto notificationDto)
        {
            var headers = notificationDto.Headers.Select(h => new Header { Name = h.Name, Value = h.Value }).ToList();
            this.SMTPService.SendNotification(notificationDto.FromAddress, notificationDto.FromDisplayName, notificationDto.ToAddresses, notificationDto.CCAddresses, notificationDto.BCCAddresses,
            notificationDto.ReplyToAddresses, headers, notificationDto.Subject, notificationDto.Body, notificationDto.IsBodyHtml);
            return new MetaResponseDto();
        }
    }
}
