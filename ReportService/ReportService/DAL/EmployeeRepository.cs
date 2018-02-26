using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

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

        public async Task<IEnumerable<Domain.Employee>> GetEmployeesAsync()
        {
            string sql = @"SELECT e.name AS Name, 
                               e.inn AS Inn, 
                               d.name AS Department 
                        FROM emps e 
                        LEFT JOIN deps d ON e.departmentid = d.id AND d.active = true";
            
            return await base.Db.QueryAsync<Domain.Employee>(sql);
        }
    }
}
