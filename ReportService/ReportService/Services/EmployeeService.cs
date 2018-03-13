using Microsoft.Extensions.Caching.Memory;
using ReportService.Clients;
using ReportService.DAL;
using ReportService.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReportService.Services
{
    public class EmployeeService : IEmployeeService
    {
        private const string CACHE_KEY = "_mihalych";

        private readonly IMemoryCache _cache;
        private readonly ISalaryApiClient _salaryApiClient;
        private readonly IBuhApiClient _buhApiClient;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IMemoryCache memoryCache, ISalaryApiClient salaryApiClient, IBuhApiClient buhApiClient)
        {
            this._employeeRepository = employeeRepository;
            this._salaryApiClient = salaryApiClient;
            this._buhApiClient = buhApiClient;
            this._cache = memoryCache;
        }

        public IEnumerable<Employee> GetEmployeesWithSalary(int year, int month)
        {
            return GetEmployeesWithSalaryAsync(year, month).Result;
        }

        public Task<IEnumerable<Employee>> GetEmployeesWithSalaryAsync(int year, int month)
        {
            return _cache.GetOrCreateAsync(CACHE_KEY + month + year, async (entry) =>
            {
                var employees = await _employeeRepository.GetEmployeesAsync();

                List<Task> taskList = new List<Task>();
                foreach (var employee in employees)
                {
                    var task = _buhApiClient.GetBuhCodeByInn(employee.Inn).ContinueWith(async (t) =>
                        {
                            employee.BuhCode = t.Result;
                            employee.Salary = await _salaryApiClient.GetSalary(employee);
                        });
                    taskList.Add(task);
                }

                Task.WaitAll(taskList.ToArray());

                entry.Value = employees;
                entry.SlidingExpiration = TimeSpan.FromDays(30);

                return employees;
            });
        }
    }
}
