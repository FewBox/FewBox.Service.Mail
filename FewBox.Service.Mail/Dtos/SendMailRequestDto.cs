using System.Collections.Generic;

namespace FewBox.Service.Mail.Dtos
{
    public class SendMailRequestDto
    {
        public string FromAddress { get; set; }
        public string FromDisplayName { get; set; }
        public IList<string> ToAddresses { get; set; }
        public IList<string> CCAddresses { get; set; }
        public IList<string> BCCAddresses { get; set; }
        public bool IsBodyHtml { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IList<Header> Headers { get; set; }
    }

    public class Header
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}