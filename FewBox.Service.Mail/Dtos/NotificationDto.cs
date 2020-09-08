using System.Collections.Generic;

namespace FewBox.Service.Mail.Dtos
{
    public class NotificationDto
    {
        public string FromAddress { get; set; }
        public string FromDisplayName { get; set; }
        public IList<string> ToAddresses { get; set; }
        public IList<string> CCAddresses { get; set; }
        public IList<string> BCCAddresses { get; set; }
        public IList<string> ReplyToAddresses { get; set; }
        public bool IsBodyHtml { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IList<HeaderDto> Headers { get; set; }
    }

    public class HeaderDto
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}