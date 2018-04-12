using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Services
{
    public class ReportHelperService : IReportHelperService
    {
        private readonly IEmployeeService _employeeService;

        public ReportHelperService(IEmployeeService employeeService)
        {
            this._employeeService = employeeService;
        }

        public async Task<byte[]> MakeReportAsync(int year, int month)
        {
            var employees = await _employeeService.GetEmployeesWithSalaryAsync(year, month);

            byte[] buff = await new ReportBuilder(year, month).BuildReportAsync(employees);

            return buff;
        }
    }
}
