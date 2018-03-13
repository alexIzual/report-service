using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReportService.DAL
{
    public class EmployeeRepository : BaseRepository, IEmployeeRepository
    {
        public EmployeeRepository(string connectionString)
            : base(connectionString)
        {
        }


        public IEnumerable<Domain.Employee> GetEmployees()
        {
            return this.GetEmployeesAsync().Result;
        }

        public Task<IEnumerable<Domain.Employee>> GetEmployeesAsync()
        {
            string sql = @"SELECT e.name AS Name, 
                               e.inn AS Inn, 
                               d.name AS Department 
                        FROM emps e 
                        LEFT JOIN deps d ON e.departmentid = d.id AND d.active = true";
            
            return base.Db.QueryAsync<Domain.Employee>(sql);
        }
    }
}
