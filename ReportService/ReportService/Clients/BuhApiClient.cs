using System.Net.Http;
using System.Threading.Tasks;

namespace ReportService.Clients
{
    public class BuhApiClient : IBuhApiClient
    {
        /// <summary>
        /// Возвращает код сотрудника из сервиса кадровиков.
        /// </summary>
        /// <param name="inn"></param>
        /// <returns></returns>
        public Task<string> GetBuhCodeByInnAsync(string inn)
        {
            using (HttpClient client = new HttpClient())
            {
                return client.GetStringAsync("http://buh.local/api/inn/" + inn);
            }
        }
    }
}
