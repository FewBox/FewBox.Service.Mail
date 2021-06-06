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
        private SmtpConfig SmtpConfig { get; set; }
        private TemplateConfig TemplateConfig { get; set; }
        private NotificationTemplateConfig NotificationTemplateConfig { get; set; }
        private ILogger Logger { get; set; }
        public SMTPService(SmtpConfig smtpConfig, TemplateConfig templateConfig, NotificationTemplateConfig notificationTemplateConfig, ILogger<SMTPService> logger)
        {
            this.SmtpConfig = smtpConfig;
            this.TemplateConfig = templateConfig;
            this.NotificationTemplateConfig = notificationTemplateConfig;
            this.Logger = logger;
        }

        public void SendNotification(string fromAddress, string fromDisplayName, IList<string> toAddresses, IList<string> ccAddresses, IList<string> bccAddresses,
        IList<string> replyToAddresses, IList<Header> headers, string subject, string body, bool isBodyHtml)
        {
            using (SmtpClient smtpClient = new SmtpClient())
            {
                this.Logger.LogDebug("Smtp Config: {0}", JsonUtility.Serialize<SmtpConfig>(this.SmtpConfig));
                var encoding = Encoding.UTF8;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Host = this.SmtpConfig.Host;
                smtpClient.Port = this.SmtpConfig.Port;
                smtpClient.Timeout = this.SmtpConfig.Timeout;
                smtpClient.EnableSsl = this.SmtpConfig.EnableSsl;
                smtpClient.Credentials = new NetworkCredential
                {
                    UserName = this.SmtpConfig.Username,
                    Password = this.SmtpConfig.Password
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
                string subject = String.Format(this.NotificationTemplateConfig.SubjectWapper, name);
                string body = String.Format(Base64Utility.Deserialize(this.NotificationTemplateConfig.BodyWapper), content);
                this.SendNotification(this.TemplateConfig.FromAddress, this.TemplateConfig.FromDisplayName, toAddresses != null ? toAddresses : this.TemplateConfig.ToAddresses, this.TemplateConfig.CCAddresses, this.TemplateConfig.BCCAddresses,
                this.TemplateConfig.ReplyToAddresses, this.TemplateConfig.Headers != null ? this.TemplateConfig.Headers.Select(h => new Header { Name = h.Name, Value = h.Value }).ToList() : null, subject, body, true);
            }
            catch (Exception exception)
            {
                this.Logger.LogError(exception, exception.Message);
            }
        }
    }
}