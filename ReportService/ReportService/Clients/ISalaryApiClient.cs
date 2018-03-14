using ReportService.Domain;
using System.Threading.Tasks;

namespace ReportService.Clients
{
    public interface ISalaryApiClient
    {
        /// <summary>
        /// Возвращает зарплату сотрудника из веб-сервиса бухгалтерского отдела.
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        Task<decimal> GetSalaryAsync(Employee employee, int year, int month);
    }
}
