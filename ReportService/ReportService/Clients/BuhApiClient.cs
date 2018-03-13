using System.Net.Http;
using System.Threading.Tasks;

namespace ReportService.Clients
{
    public class BuhApiClient : IBuhApiClient
    {
        public Task<string> GetBuhCodeByInn(string inn)
        {
            var client = new HttpClient();

            return client.GetStringAsync("http://buh.local/api/inn/" + inn);
        }
    }
}
