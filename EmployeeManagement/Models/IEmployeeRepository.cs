using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
  public  interface IEmployeeRepository
    {
        Employees GetEmployees(int id);
        IEnumerable<Employees> GetAllEmployee();
        Employees Add(Employees employees);
        Employees Update(Employees employeeChanges);
        Employees Delete(int Id);
    }
}
