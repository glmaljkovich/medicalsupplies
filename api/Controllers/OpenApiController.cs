using System.Collections.Generic;
using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using ArqNetCore.Services;
using ArqNetCore.DTOs.User;
using ArqNetCore.DTOs.Account;
using ArqNetCore.DTOs.Auth;

namespace ArqNetCore.Controllers
{
    [ApiController]
    [Route("open-api")]
    public class OpenApiController : ControllerBase
    {
        IHostingEnvironment _hostingEnvironment;
        public OpenApiController(IHostingEnvironment hostingEnvironment) {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api.yaml")]
        public IActionResult BannerImage()
        {
            var file = Path.Combine(
                _hostingEnvironment.ContentRootPath, 
                "api.yaml"
            );
            return PhysicalFile(file, "application/x-yaml");
        }

    }
}
