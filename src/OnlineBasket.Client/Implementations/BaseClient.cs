namespace OnlineBasket.Client.Implementations
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public abstract class BaseClient
    {
        protected HttpClient _httpClient;

        protected async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            return await ReadAsAsync<T>(response.Content);
        }

        protected async Task<T> ReadAsAsync<T>(HttpContent content)
        {
            var stringContent = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(stringContent);
        }

        protected ByteArrayContent ByteContent<T>(T content)
        {
            var jsonContent = JsonConvert.SerializeObject(content);
            var buffer = System.Text.Encoding.UTF8.GetBytes(jsonContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return byteContent;
        }
    }
}