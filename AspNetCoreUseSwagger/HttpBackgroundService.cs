using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreUseSwagger
{
    public class HttpBackgroundService : BackgroundService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HttpBackgroundService> _logger;

        public HttpBackgroundService(IHttpClientFactory httpClientFactory, ILogger<HttpBackgroundService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                var httpClient = _httpClientFactory.CreateClient("default");
                await httpClient.GetAsync("https://bidianqing-aspnetcoreuseswagger.azurewebsites.net/swagger/index.html");

                await Task.Delay(new TimeSpan(0, 0, 30), stoppingToken);
            }
        }
    }
}
