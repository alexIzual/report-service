using System.Threading.Tasks;

namespace ReportService.Clients
{
    public interface IBuhApiClient
    {
        Task<string> GetBuhCodeByInn(string inn);
    }
}
