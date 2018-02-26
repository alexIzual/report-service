using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.DAL
{
    public interface IEmployeeRepository : IBaseRepository
    {
        IEnumerable<Domain.Employee> GetEmployees();

        Task<IEnumerable<Domain.Employee>> GetEmployeesAsync();
    }
}
