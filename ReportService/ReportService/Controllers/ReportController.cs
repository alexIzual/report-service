using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReportService.Services;
using System;
using System.Threading.Tasks;

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private readonly ILogger<ReportController> _logger;
        private readonly IReportHelperService _reportService;

        public ReportController(IReportHelperService reportService, ILogger<ReportController> logger)
        {
            this._reportService = reportService;
            this._logger = logger;
        }

        [HttpGet]
        [Route("{year}/{month}")]
        public async Task<IActionResult> Download(int year, int month)
        {
            if (month < 1 || month > 12) return new BadRequestResult();

            if (year < 1 || year > 9999) return new BadRequestResult();

            try
            {
                var buff = await _reportService.MakeReportAsync(year, month);

                string fileName = $"report_{month}_{year}.txt";

                return File(buff, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, " ");

                return StatusCode(500);
            }
        }
    }
}
