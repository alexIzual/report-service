using ReportService.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReportService.Services
{
    public interface IEmployeeService
    {
        /// <summary>
        /// Получает и кэширует список сотрудников с информацией по отделу из БД employee, 
        /// получает зарплату сотрудников из веб-сервиса бухгалтерского отдела по коду сотрудника из сервиса кадровиков.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        Task<IEnumerable<Employee>> GetEmployeesWithSalaryAsync(int year, int month);
    }
}
