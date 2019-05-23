namespace OnlineBasket.Client.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using OnlineBasket.Client.Interfaces;
    using OnlineBasket.Domain.DTO;
    using OnlineBasket.Domain.Enums;
    using OnlineBasket.Security.Model;

    public class LoginClient : BaseClient, ILoginClient
    {
        public readonly string _baseAddress = "api/Login";

        public LoginClient(string apiEndpoint, HttpClient httpClient)
        {
            _baseAddress = apiEndpoint + _baseAddress;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_baseAddress);
        }

        public async Task<AuthenticatedToken> Login(string username, string password)
        {
            var url = "";
            var content = ByteContent(new { username, password });
            var response = await _httpClient.PostAsync(url, content);
            return await ReadAsAsync<AuthenticatedToken>(response.Content);
        }

        
    }
}