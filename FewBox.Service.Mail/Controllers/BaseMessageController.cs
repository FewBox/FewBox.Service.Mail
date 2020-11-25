using FewBox.Service.Mail.Model.Services;
using Microsoft.AspNetCore.Mvc;

namespace FewBox.Service.Mail.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public abstract class BaseMessageController : ControllerBase
    {
        protected ISMTPService SMTPService { get; set; }

        public BaseMessageController(ISMTPService smtpService)
        {
            this.SMTPService = smtpService;
        }
    }
}