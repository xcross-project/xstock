using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xstock.lib.schema.dto;

namespace xpay.lib.comp.webapi.controllers
{
    [ApiController]
    [Route("test")]
    public class test : ControllerBase
    {
        [HttpGet]
        [Route("hello")]
        public string hello() => new stdgram<string>("success", "hello", 0x01).tostring();
    }
}
