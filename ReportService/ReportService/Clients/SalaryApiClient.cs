using Newtonsoft.Json.Linq;
using ReportService.Domain;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Clients
{
    public class SalaryApiClient : ISalaryApiClient
    {
        public async Task<decimal> GetSalary(Employee employee)
        {
            var client = new HttpClient();

            JObject jsonObject = JObject.FromObject(new { employee.BuhCode });

            var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://salary.local/api/empcode/" + employee.Inn, content);

            string responseAsString = await response.Content.ReadAsStringAsync();

            return decimal.Parse(responseAsString);
        }
    }
}
