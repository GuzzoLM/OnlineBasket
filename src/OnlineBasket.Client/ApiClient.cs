namespace OnlineBasket.Client
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using OnlineBasket.Client.Configurations;
    using OnlineBasket.Client.Implementations;
    using OnlineBasket.Client.Interfaces;

    public class ApiClient
    {
        private readonly string _apiEndpoint;

        public readonly IBasketClient BasketClient;
        public readonly IProductGroupClient ProductGroupClient;
        public readonly IProductClient ProductClient;
        public readonly ILoginClient LoginClient;

        public ApiClient(HttpClient httpClient, IConfiguration configuration)
        {
            var apiConfigurations = new ApiConfigurations();
            new ConfigureFromConfigurationOptions<ApiConfigurations>(configuration.GetSection("ApiConfigurations"))
                    .Configure(apiConfigurations);

            _apiEndpoint = apiConfigurations.BaseURL;

            if (!_apiEndpoint.EndsWith("/"))
            {
                _apiEndpoint += "/";
            }

            httpClient.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", apiConfigurations.Token);

            BasketClient = new BasketClient(_apiEndpoint, httpClient);
            ProductGroupClient = new ProductGroupClient(_apiEndpoint, httpClient);
            ProductClient = new ProductClient(_apiEndpoint, httpClient);
            LoginClient = new LoginClient(_apiEndpoint, httpClient);
        }
    }
}