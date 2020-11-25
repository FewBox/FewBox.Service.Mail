using System.Collections.Generic;

namespace FewBox.Service.Mail.Model.Configs
{
    public class TemplateConfig
    {
        public string FromAddress { get; set; }
        public string FromDisplayName { get; set; }
        public IList<string> ToAddresses { get; set; }
        public IList<string> CCAddresses { get; set; }
        public IList<string> BCCAddresses { get; set; }
        public IList<string> ReplyToAddresses { get; set; }
        public IList<HeaderConfig> Headers { get; set; }
    }
}