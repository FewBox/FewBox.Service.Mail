using System;
using System.Threading;
using System.Threading.Tasks;
using FewBox.SDK.Mail;
using FewBox.Service.Mail.Model.Services;

namespace FewBox.Service.Mail.Domain.Services
{
    public class ScopedMQService : IScopedMQService
    {
        private IMailService MailService { get; }
        private ISMTPService SMTPService { get; }

        public ScopedMQService(IMailService mailService, ISMTPService smtpService)
        {
            this.MailService = mailService;
            this.SMTPService = smtpService;
        }

        public void Process()
        {
            this.MailService.ReceiveOpsNotification((mailMessage) =>
            {
                this.SMTPService.SendOpsNotification(mailMessage.Name, mailMessage.Content, mailMessage.ToAddresses);
            });
        }
    }
}
