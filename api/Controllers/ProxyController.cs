using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using ArqNetCore.Services;
using ArqNetCore.DTOs.User;
using ArqNetCore.DTOs.Account;
using ArqNetCore.DTOs.Auth;
using System;

namespace ArqNetCore.Controllers
{
    [ApiController]
    [Route("user")]
    public class ProxyController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private IMapper _mapper;

        private IHttpClientFactory _clientFactory;
        
        public ProxyController(
            ILogger<UserController> logger,
            IMapper mapper,
            IHttpClientFactory clientFactory
        )
        {
            _logger = logger;
            _mapper = mapper;
            _clientFactory = clientFactory;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("send")]
        public async Task<ProxyPipeResponseDTO> ProxyPipe(ProxyPipeRequestDTO proxyPipeRequestDTO)
        { 
            _logger.LogInformation("Proxy Pipe:" + proxyPipeRequestDTO.Id);
            HttpClient client = _clientFactory.CreateClient();
            string CLIENT_HOST = Environment.GetEnvironmentVariable("CLIENT_HOST");
            string endpoint = $"{CLIENT_HOST}/api";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,endpoint);
            request.Headers.Add("Accept", "application/vnd.github.v3+json");
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");
            HttpResponseMessage response = await client.SendAsync(request);
            return new ProxyPipeResponseDTO();
        }
    }
}
