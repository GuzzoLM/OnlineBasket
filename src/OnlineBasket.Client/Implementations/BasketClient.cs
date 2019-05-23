﻿namespace OnlineBasket.Client.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using OnlineBasket.Client.Interfaces;
    using OnlineBasket.Domain.DTO;
    using OnlineBasket.Domain.Enums;

    public class BasketClient : IBasketClient
    {
        public readonly string _baseAddress = "api/Basket";
        private readonly HttpClient _httpClient;

        public BasketClient(string apiEndpoint, HttpClient httpClient)
        {
            _baseAddress = apiEndpoint + _baseAddress;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_baseAddress);
        }

        public Task Delete(Guid id)
        {
            var url = "/" + id.ToString();
            return _httpClient.DeleteAsync(url);
        }

        public Task<BasketDTO> GetBasket(Guid id)
        {
            var url = "/" + id.ToString();
            return GetAsync<BasketDTO>(url);
        }

        public Task<IEnumerable<BasketDTO>> GetBaskets(BasketStatus? status)
        {
            var url = (status.HasValue) ? "/?status=" + ((int)status.Value).ToString() : "/";
            return GetAsync<IEnumerable<BasketDTO>>(url);
        }

        public async Task<Guid> Post()
        {
            var url = "/";
            var response = await _httpClient.PostAsync(url, null);
            return await ReadAsAsync<Guid>(response.Content);
        }

        private async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            return await ReadAsAsync<T>(response.Content);
        }

        private async Task<T> ReadAsAsync<T>(HttpContent content)
        {
            var stringContent = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(stringContent);
        }
    }
}