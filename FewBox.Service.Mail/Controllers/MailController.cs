using FewBox.Core.Web.Dto;
using FewBox.Service.Mail.Configs;
using FewBox.Service.Mail.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FewBox.Service.Mail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : BaseMessageController
    {
        public MailController(SmtpConfig smtpConfig) : base(smtpConfig)
        {
        }

        [HttpPost]
        public MetaResponseDto Send(SendMailRequestDto sendMailRequestDto)
        {
            this.SendMessage(sendMailRequestDto.FromAddress, sendMailRequestDto.FromDisplayName, sendMailRequestDto.ToAddresses, sendMailRequestDto.CCAddresses, sendMailRequestDto.BCCAddresses,
            sendMailRequestDto.ReplyToAddresses, sendMailRequestDto.Headers, sendMailRequestDto.Subject, sendMailRequestDto.Body, sendMailRequestDto.IsBodyHtml);
            return new MetaResponseDto();
        }
    }
}
