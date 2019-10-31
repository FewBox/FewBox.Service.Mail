using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using FewBox.Service.Mail.Configs;
using FewBox.Service.Mail.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FewBox.Service.Mail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseMessageController : ControllerBase
    {
        private SmtpConfig SmtpConfig { get; set; }

        public BaseMessageController(SmtpConfig smtpConfig)
        {
            this.SmtpConfig = smtpConfig;
        }

        protected void SendMessage(string fromAddress, string fromDisplayName, IList<string> toAddresses, IList<string> ccAddresses, IList<string> bccAddresses,
        IList<string> replyToAddresses, IList<HeaderDto> headers, string subject, string body, bool isBodyHtml)
        {
            using(SmtpClient smtpClient = new SmtpClient())
            {
                var encoding = Encoding.UTF8;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Host = this.SmtpConfig.Host;
                smtpClient.Port = this.SmtpConfig.Port;
                smtpClient.Timeout = this.SmtpConfig.Timeout;
                smtpClient.EnableSsl = this.SmtpConfig.EnableSsl;
                smtpClient.Credentials = new NetworkCredential{
                    UserName = this.SmtpConfig.Username,
                    Password = this.SmtpConfig.Password
                };
                
                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(fromAddress, fromDisplayName, encoding);
                if(toAddresses != null)
                {
                    foreach(string to in toAddresses)
                    {
                        mailMessage.To.Add(to);
                    }
                }
                if(ccAddresses != null)
                {
                    foreach(string cc in ccAddresses)
                    {
                        mailMessage.CC.Add(cc);
                    }
                }
                if(bccAddresses != null)
                {
                    foreach(string bcc in bccAddresses)
                    {
                        mailMessage.Bcc.Add(bcc);
                    }
                }
                if(replyToAddresses != null)
                {
                    foreach(string reply in replyToAddresses)
                    {
                        mailMessage.ReplyToList.Add(reply);
                    }
                }
                if(headers != null)
                {
                    mailMessage.HeadersEncoding = encoding;
                    foreach(HeaderDto header in headers)
                    {
                        mailMessage.Headers.Add(header.Name, header.Value);
                    }
                }
                mailMessage.SubjectEncoding = encoding;
                mailMessage.Subject = subject;
                mailMessage.BodyEncoding = encoding;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = isBodyHtml;
                smtpClient.Send(mailMessage);
            }
        }
    }
}