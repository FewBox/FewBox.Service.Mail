using System.Collections.Generic;

namespace FewBox.Service.Mail.Dtos
{
    public class OpsNotificationDto
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public IList<string> ToAddresses { get; set; }
    }
}