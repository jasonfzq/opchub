using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpcHub.Da.Contract;

namespace OpcHub.Da.Client.Services
{
    public class ReadService : IReadService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _webApiUri;

        public ReadService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            string serviceIp = configuration["opcda:service"];
            _webApiUri = $"http://{serviceIp}:9010/api/datahub/read";
        }

        public async Task<ReadCommandResult> Read(ReadCommandRequest request)
        {
            HttpClient http = _httpClientFactory.CreateClient();
            var response = await http.PostAsync(_webApiUri, new JsonContent(request));
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            ReadCommandResult commandResult = JsonConvert.DeserializeObject<ReadCommandResult>(json);

            return commandResult;
        }
    }

    public class JsonContent : StringContent
    {
        public JsonContent(object obj)
            : base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
        {
        }
    }
}