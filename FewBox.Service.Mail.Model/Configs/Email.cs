using System.Collections.Generic;

namespace FewBox.Service.Mail.Model.Configs
{
    public class Email
    {
        public string FromAddress { get; set; }
        public string FromDisplayName { get; set; }
        public IList<string> ToAddresses { get; set; }
        public IList<string> CCAddresses { get; set; }
        public IList<string> BCCAddresses { get; set; }
        public IList<string> ReplyToAddresses { get; set; }
        public IList<EmailHeader> Headers { get; set; }
        public Template Template { get; set; }
        public Smtp Smtp { get; set; }
    }
}