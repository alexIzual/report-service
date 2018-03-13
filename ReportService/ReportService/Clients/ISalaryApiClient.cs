using ReportService.Domain;
using System.Threading.Tasks;

namespace ReportService.Clients
{
    public interface ISalaryApiClient
    {
        Task<decimal> GetSalary(Employee employee);
    }
}
