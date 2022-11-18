using System.Collections.Generic;
using FewBox.Service.Mail.Model.Services;
using FewBox.Service.Mail.Model.Configs;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System;
using FewBox.Core.Utility.Formatter;
using Microsoft.Extensions.Logging;

namespace FewBox.Service.Mail.Domain.Services
{
    public class SMTPService : ISMTPService
    {
        private Email Email { get; set; }
        private ILogger Logger { get; set; }
        public SMTPService(Smtp smtpConfig, Email email, ILogger<SMTPService> logger)
        {
            this.Email = email;
            this.Logger = logger;
        }

        public void SendNotification(string fromAddress, string fromDisplayName, IList<string> toAddresses, IList<string> ccAddresses, IList<string> bccAddresses,
        IList<string> replyToAddresses, IList<Header> headers, string subject, string body, bool isBodyHtml)
        {
            using (SmtpClient smtpClient = new SmtpClient())
            {
                this.Logger.LogDebug("Smtp Config: {0}", JsonUtility.Serialize<Smtp>(this.Email.Smtp));
                var encoding = Encoding.UTF8;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Host = this.Email.Smtp.Host;
                smtpClient.Port = this.Email.Smtp.Port;
                smtpClient.Timeout = this.Email.Smtp.Timeout;
                smtpClient.EnableSsl = this.Email.Smtp.EnableSsl;
                smtpClient.Credentials = new NetworkCredential
                {
                    UserName = this.Email.Smtp.Username,
                    Password = this.Email.Smtp.Password
                };

                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(fromAddress, fromDisplayName, encoding);
                if (toAddresses != null)
                {
                    foreach (string to in toAddresses)
                    {
                        mailMessage.To.Add(to);
                    }
                }
                if (ccAddresses != null)
                {
                    foreach (string cc in ccAddresses)
                    {
                        mailMessage.CC.Add(cc);
                    }
                }
                if (bccAddresses != null)
                {
                    foreach (string bcc in bccAddresses)
                    {
                        mailMessage.Bcc.Add(bcc);
                    }
                }
                if (replyToAddresses != null)
                {
                    foreach (string reply in replyToAddresses)
                    {
                        mailMessage.ReplyToList.Add(reply);
                    }
                }
                if (headers != null)
                {
                    mailMessage.HeadersEncoding = encoding;
                    foreach (Header header in headers)
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
                this.Logger.LogDebug("Send email finished!");
            }
        }

        public void SendOpsNotification(string name, string content, IList<string> toAddresses = null)
        {
            try
            {
                string subject = String.Format(this.Email.Template.SubjectWapper, name);
                string body = String.Format(Base64Utility.Deserialize(this.Email.Template.BodyWapper), content);
                this.SendNotification(this.Email.FromAddress, this.Email.FromDisplayName, toAddresses != null ? toAddresses : this.Email.ToAddresses, this.Email.CCAddresses, this.Email.BCCAddresses,
                this.Email.ReplyToAddresses, this.Email.Headers != null ? this.Email.Headers.Select(h => new Header { Name = h.Name, Value = h.Value }).ToList() : null, subject, body, true);
            }
            catch (Exception exception)
            {
                this.Logger.LogError(exception, exception.Message);
            }
        }
    }
}