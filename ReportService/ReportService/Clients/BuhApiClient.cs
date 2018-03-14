using System.Net.Http;
using System.Threading.Tasks;

namespace ReportService.Clients
{
    public class BuhApiClient : BaseClient, IBuhApiClient
    {
        /// <summary>
        /// Возвращает код сотрудника из сервиса кадровиков.
        /// </summary>
        /// <param name="inn"></param>
        /// <returns></returns>
        public Task<string> GetBuhCodeByInnAsync(string inn)
        {
            return HttpClient.GetStringAsync("http://buh.local/api/inn/" + inn);
        }
    }
}
