using ReportService.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReportService.DAL
{
    public interface IEmployeeRepository : IBaseRepository
    {
        /// <summary>
        /// Возвращает список всех сотрудников.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Employee>> GetEmployeesAsync();
    }
}
