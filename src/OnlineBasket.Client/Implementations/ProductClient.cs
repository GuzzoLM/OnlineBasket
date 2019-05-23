namespace OnlineBasket.Client.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using OnlineBasket.Client.Interfaces;
    using OnlineBasket.Domain.DTO;

    public class ProductClient : BaseClient, IProductClient
    {
        public readonly string _baseAddress = "api/Product";

        public ProductClient(string apiEndpoint, HttpClient httpClient)
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

        public Task<ProductDTO> GetProduct(Guid id)
        {
            var url = "/" + id.ToString();
            return GetAsync<ProductDTO>(url);
        }

        public Task<IEnumerable<ProductDTO>> GetProducts(string name, decimal? price, int? stock)
        {
            var url = "";

            var stringParameters = "";

            if (!string.IsNullOrEmpty(name))
                stringParameters += "&name=" + name;

            if (price.HasValue)
                stringParameters += "&price=" + price.Value.ToString();

            if (stock.HasValue)
                stringParameters += "&stock=" + stock.Value.ToString();

            if (!string.IsNullOrEmpty(stringParameters))
                url = "?" + stringParameters.Substring(stringParameters.IndexOf("&") + 1);

            return GetAsync<IEnumerable<ProductDTO>>(url);
        }

        public async Task<Guid> Post(ProductDTO product)
        {
            var url = "/";
            var content = ByteContent(product);

            var response = await _httpClient.PostAsync(url, content);
            return await ReadAsAsync<Guid>(response.Content);
        }

        public Task Put(Guid id, ProductDTO product)
        {
            var url = "/" + id.ToString();
            var content = ByteContent(product);

            return _httpClient.PutAsync(url, content);
        }
    }
}