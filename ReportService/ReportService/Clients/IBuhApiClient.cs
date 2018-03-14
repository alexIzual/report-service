using System.Threading.Tasks;

namespace ReportService.Clients
{
    public interface IBuhApiClient
    {
        /// <summary>
        /// Возвращает код сотрудника из сервиса кадровиков.
        /// </summary>
        /// <param name="inn"></param>
        /// <returns></returns>
        Task<string> GetBuhCodeByInnAsync(string inn);
    }
}
