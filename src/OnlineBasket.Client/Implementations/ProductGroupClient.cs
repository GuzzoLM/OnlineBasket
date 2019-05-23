namespace OnlineBasket.Client.Implementations
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using OnlineBasket.Client.Interfaces;
    using OnlineBasket.Domain.DTO;

    public class ProductGroupClient : BaseClient, IProductGroupClient
    {
        public readonly string _baseAddress = "api/{basketId}/ProductGroup";

        public ProductGroupClient(string apiEndpoint, HttpClient httpClient)
        {
            _baseAddress = apiEndpoint + _baseAddress;
            _httpClient = httpClient;
        }

        public Task Delete(Guid bid, Guid? id)
        {
            var url = (id.HasValue)
                ? AddresWithBasketId(bid) + "/" + id.ToString()
                : AddresWithBasketId(bid);

            return _httpClient.DeleteAsync(url);
        }

        public Task Put(Guid bid, ProductGroupDTO productGroup)
        {
            var url = AddresWithBasketId(bid);
            var byteContent = ByteContent(productGroup);

            return _httpClient.PutAsync(url, byteContent);
        }

        private string AddresWithBasketId(Guid bid)
        {
            return _baseAddress.Replace("{basketId}", bid.ToString());
        }
    }
}