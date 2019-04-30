﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using FewBox.Core.Web.Dto;
using FewBox.Service.Mail.Configs;
using FewBox.Service.Mail.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FewBox.Service.Mail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private SmtpConfig SmtpConfig { get; set; }

        public MailController(SmtpConfig smtpConfig)
        {
            this.SmtpConfig = smtpConfig;
        }

        [HttpPost]
        public MetaResponseDto SendMail(SendMailRequestDto sendMailRequestDto)
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
                mailMessage.From = new MailAddress(sendMailRequestDto.FromAddress, sendMailRequestDto.FromDisplayName, encoding);
                if(sendMailRequestDto.ToAddresses != null)
                {
                    foreach(string to in sendMailRequestDto.ToAddresses)
                    {
                        mailMessage.To.Add(to);
                    }
                }
                if(sendMailRequestDto.CCAddresses != null)
                {
                    foreach(string cc in sendMailRequestDto.CCAddresses)
                    {
                        mailMessage.CC.Add(cc);
                    }
                }
                if(sendMailRequestDto.BCCAddresses != null)
                {
                    foreach(string bcc in sendMailRequestDto.BCCAddresses)
                    {
                        mailMessage.Bcc.Add(bcc);
                    }
                }
                if(sendMailRequestDto.ReplyToAddresses != null)
                {
                    foreach(string reply in sendMailRequestDto.ReplyToAddresses)
                    {
                        mailMessage.ReplyToList.Add(reply);
                    }
                }
                if(sendMailRequestDto.Headers != null)
                {
                    mailMessage.HeadersEncoding = encoding;
                    foreach(Header header in sendMailRequestDto.Headers)
                    {
                        mailMessage.Headers.Add(header.Name, header.Value);
                    }
                }
                mailMessage.SubjectEncoding = encoding;
                mailMessage.Subject = sendMailRequestDto.Subject;
                mailMessage.BodyEncoding = encoding;
                mailMessage.Body = sendMailRequestDto.Body;
                mailMessage.IsBodyHtml = sendMailRequestDto.IsBodyHtml;
                smtpClient.Send(mailMessage);
            }
            return new MetaResponseDto();
        }
    }
}
