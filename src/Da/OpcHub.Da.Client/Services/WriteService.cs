using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpcHub.Da.Contract;

namespace OpcHub.Da.Client.Services
{
    public class WriteService : IWriteService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _webApiUri;

        public WriteService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            string serviceIp = configuration["opcda:service"];
            _webApiUri = $"http://{serviceIp}:9010/api/datahub/write";
        }

        public async Task<WriteCommandResult> Write(WriteCommandRequest request)
        {
            HttpClient http = _httpClientFactory.CreateClient();
            var response = await http.PostAsync(_webApiUri, new JsonContent(request));
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            WriteCommandResult commandResult = JsonConvert.DeserializeObject<WriteCommandResult>(json);
            return commandResult;
        }
    }
}