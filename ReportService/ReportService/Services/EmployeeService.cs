using Microsoft.Extensions.Caching.Memory;
using ReportService.Clients;
using ReportService.DAL;
using ReportService.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

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

        /// <summary>
        /// Получает и кэширует список сотрудников с информацией по отделу из БД employee, 
        /// получает зарплату сотрудников из веб-сервиса бухгалтерского отдела по коду сотрудника из сервиса кадровиков.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public Task<IEnumerable<Employee>> GetEmployeesWithSalaryAsync(int year, int month)
        {
            // Кэширование объекта или возврат объекта из кэша по ключу.
            return _cache.GetOrCreateAsync(CACHE_KEY + month + year, async (entry) =>
            {
                // Получение списка всех сотрудников.
                var employees = await _employeeRepository.GetEmployeesAsync();

                // Параметры, используемые для настройки обработки, выполняемой блоками.
                var blockOptions = new ExecutionDataflowBlockOptions()
                {
                    MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded
                };

                // Блок получения кода сотрудника.
                var getInnBlock = new TransformBlock<Employee, Employee>(async emp =>
                {
#if DEBUG
                    await Task.Delay(5000); // Эмулируем задержку ответа сервиса.
#endif
                    emp.BuhCode = await _buhApiClient.GetBuhCodeByInnAsync(emp.Inn);

                    return emp;

                }, blockOptions);

                // Блок обработки получения зарплаты сотрудника.
                var getSalaryBlock = new ActionBlock<Employee>(async emp =>
                {
#if DEBUG
                    await Task.Delay(5000); // Эмулируем задержку ответа сервиса.
#endif
                    emp.Salary = await _salaryApiClient.GetSalaryAsync(emp, year, month);

                }, blockOptions);
                
                // Параметры, импользуемые для настройки связи между блоками.
                var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

                // Определение связи блоков.
                getInnBlock.LinkTo(getSalaryBlock, linkOptions);

                // Инициализация обработки.
                foreach (var employee in employees)
                    getInnBlock.Post(employee);

                // Оповещение блока о завершении инициализации обработок.
                getInnBlock.Complete();

                // Ожидание завершения обработки данных.
                getSalaryBlock.Completion.Wait();
                
                // Добавляем сотрудников в кэш.
                entry.Value = employees;

                // Срок хранения объекта в кэше.
                entry.SlidingExpiration = TimeSpan.FromHours(10);

                return employees;
            });
        }
    }
}
