using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReportService.Services
{
    public interface IEmployeeService
    {
        IEnumerable<Domain.Employee> GetEmployeesWithSalary(int year, int month);

        Task<IEnumerable<Domain.Employee>> GetEmployeesWithSalaryAsync(int year, int month);
    }
}
