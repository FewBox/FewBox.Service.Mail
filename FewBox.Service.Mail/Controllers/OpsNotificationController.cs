using FewBox.Core.Web.Dto;
using FewBox.Service.Mail.Dtos;
using FewBox.Service.Mail.Model.Services;
using Microsoft.AspNetCore.Mvc;

namespace FewBox.Service.Mail.Controllers
{
    public class OpsNotificationController : BaseMessageController
    {
        public OpsNotificationController(ISMTPService smtpService) : base(smtpService)
        {
        }

        [HttpPost]
        public MetaResponseDto Send(OpsNotificationDto opsNotificationDto)
        {
            this.SMTPService.SendOpsNotification(opsNotificationDto.Name, opsNotificationDto.Content, opsNotificationDto.ToAddresses);
            return new MetaResponseDto();
        }
    }
}