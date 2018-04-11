using Newtonsoft.Json.Linq;
using ReportService.Domain;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Clients
{
    public class SalaryApiClient : ISalaryApiClient
    {
        /// <summary>
        /// Возвращает зарплату сотрудника из веб-сервиса бухгалтерского отдела.
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public async Task<decimal> GetSalaryAsync(Employee employee, int year, int month)
        {
            // UNDONE: не нашел использования даты при получении зарплаты, предположительно должна передаваться вместе с кодом сотрудника.

            JObject jsonObject = JObject.FromObject(new { employee.BuhCode });

            var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                var response = await client.PostAsync("http://salary.local/api/empcode/" + employee.Inn, content);

                string responseAsString = await response.Content.ReadAsStringAsync();

                return decimal.Parse(responseAsString);
            }
        }
    }
}
