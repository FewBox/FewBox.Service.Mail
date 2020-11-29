using System;
using FewBox.SDK.Core;
using FewBox.SDK.Mail;
using FewBox.Service.Mail.Model.Services;

namespace FewBox.Service.Mail.Domain.Services
{
    public class MQMailHandler : IMQMailHandler
    {
        private ISMTPService SMTPService { get; }

        public MQMailHandler(ISMTPService smtpService)
        {
            this.SMTPService = smtpService;
        }

        public Func<EmailMessage, bool> Handle()
        {
            return (mailMessage) =>
            {
                this.SMTPService.SendOpsNotification(mailMessage.Name, mailMessage.Content, mailMessage.ToAddresses);
                return true;
            };
        }
    }
}
