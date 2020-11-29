/*using System;
using System.Collections.Generic;
using System.Net.Mail;
using FewBox.Service.Mail.Configs;
using FewBox.Service.Mail.Controllers;
using FewBox.Service.Mail.Dtos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FewBox.Service.Mail.UnitTest
{
    [TestClass]
    public class MailControllerUnitTest
    {
        private MailController MailController { get; set; }
        private MailController SSLMailController { get; set; }
        private SendMailRequestDto SendMailRequestDto { get; set; }
        private bool IsEmptyPassword { get; set; }

        [TestInitialize]
        public void Init()
        {
            string host = "smtp.fewbox.com";
            string username = "test@fewbox.com";
            string password = "";
            this.IsEmptyPassword = String.IsNullOrEmpty(password);
            this.MailController = new MailController(new SmtpConfig{
                Host = host,
                Port = 25,
                Timeout = 8000,
                EnableSsl = false,
                Username = username,
                Password = password
            });
            this.SSLMailController = new MailController(new SmtpConfig{
                Host = host,
                Port = 465,
                Timeout = 5000,
                EnableSsl = true,
                Username = username,
                Password = password
            });
            this.SendMailRequestDto = new SendMailRequestDto{
                FromAddress = username,
                ToAddresses = new List<string>{ username },
                IsBodyHtml = true,
                Subject = "Testing Service",
                Body = "<div style='background-color: red;' >Hello FewBox!</div>"
            };
        }

        //[TestMethod]
        public void TestSendMail()
        {
            if(this.IsEmptyPassword)
            {
                Assert.ThrowsException<SmtpException>(()=>this.MailController.Send(this.SendMailRequestDto));
            }
            else
            {
                var response = this.MailController.Send(this.SendMailRequestDto);
                Assert.IsTrue(response.IsSuccessful);
            }
        }

        //TestMethod]
        public void TestSendSSLMail()
        {
            if(this.IsEmptyPassword)
            {
                Assert.ThrowsException<SmtpException>(()=>this.MailController.Send(this.SendMailRequestDto));
            }
            else
            {
                var response = this.SSLMailController.Send(this.SendMailRequestDto);
                Assert.IsTrue(response.IsSuccessful);
            }
        }
    }
}
*/