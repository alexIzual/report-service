using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Npgsql;
using ReportService.DAL;
using ReportService.Domain;

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private const string CACHE_KEY = "_mihalych";

        private readonly IMemoryCache _cache;
        private readonly IEmployeeRepository _employeeRepository;

        public ReportController(IEmployeeRepository employeeRepository, IMemoryCache memoryCache)
        {
            this._employeeRepository = employeeRepository;
            this._cache = memoryCache;
        }

        [HttpGet]
        [Route("{year}/{month}")]
        public async Task<IActionResult> Download(int year, int month)
        {
            if (month < 1 || month > 12) return new BadRequestResult();

            if (year < 1 || year > 9999) return new BadRequestResult();

            try
            {
                var cacheEntry = await _cache.GetOrCreateAsync(CACHE_KEY + month + year, async (entry) =>
                {
                    var employees = await _employeeRepository.GetEmployeesAsync();

                    entry.Value = employees;
                    entry.SlidingExpiration = TimeSpan.FromDays(30);

                    return (IEnumerable<Domain.Employee>)entry.Value;
                });
                
                var buff = await new ReportBuilder(year, month).MakeReportAsync(cacheEntry);

                return File(buff, "application/octet-stream", "report.txt");
            }
            catch (Exception ex)
            {
                // Log(ex);
                return StatusCode(500);
            }
        }
    }
}
