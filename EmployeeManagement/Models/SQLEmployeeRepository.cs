using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext context;
        private readonly ILogger<SQLEmployeeRepository> logger;

        public SQLEmployeeRepository(AppDbContext context, ILogger<SQLEmployeeRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        public Employees Add(Employees employees)
        {
            context.employee.Add(employees);
            context.SaveChanges();
            return employees;
        }

        public Employees Delete(int Id)
        {
            Employees employees = context.employee.Find(Id);
           if(employees != null)
            {
                context.employee.Remove(employees);
                context.SaveChanges();                
            }
            return employees;
        }
        public IEnumerable<Employees> GetAllEmployee()
        {
            return context.employee;
        }

        public Employees GetEmployees(int id)
        {
            logger.LogTrace("log trace message...!");
            logger.LogDebug("LogDebug message...!");
            logger.LogInformation("LogInformation message...!");
            logger.LogWarning("LogWarning message...!");
            logger.LogError("LogError message...!");
            return context.employee.Find(id);
        }

        public Employees Update(Employees employeeChanges)
        {
            var employee = context.employee.Attach(employeeChanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return employeeChanges;
        }
    }
}
