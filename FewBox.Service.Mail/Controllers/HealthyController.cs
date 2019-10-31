using System;
using FewBox.Core.Web.Dto;
using FewBox.Service.Mail.Configs;
using FewBox.Service.Mail.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FewBox.Service.Mail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthyController : ControllerBase
    {
        private HealthyConfig HealthyConfig { get; set; }

        public HealthyController(HealthyConfig healthyConfig)
        {
            this.HealthyConfig = healthyConfig;
        }

        [HttpGet]
        public PayloadResponseDto<HealthyDto> Get()
        {
            return new PayloadResponseDto<HealthyDto>
            {
                Payload = new HealthyDto
                {
                    Version = $"{this.HealthyConfig.Version}-{Environment.MachineName}"
                }
            };
        }
    }
}