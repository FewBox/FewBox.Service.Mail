using System.Collections.Generic;

namespace FewBox.Service.Mail.Model.Services
{
    public interface ISMTPService
    {
        void SendNotification(string fromAddress, string fromDisplayName, IList<string> toAddresses, IList<string> ccAddresses, IList<string> bccAddresses,
        IList<string> replyToAddresses, IList<Header> headers, string subject, string body, bool isBodyHtml);
        void SendOpsNotification(string name, string content, IList<string> toAddresses = null);
    }
}
