using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employees> _employeesList;
        public MockEmployeeRepository()
        {
            _employeesList = new List<Employees>()
        {
            new Employees() { Id = 1, Name = "Mary", Department = Dept.HR, Email = "mary@pragimtech.com" },
            new Employees() { Id = 2, Name = "John", Department = Dept.IT, Email = "john@pragimtech.com" },
            new Employees() { Id = 3, Name = "Sam", Department = Dept.Payroll, Email = "sam@pragimtech.com" },
        };
        }

        public Employees Add(Employees employees)
        {
            employees.Id = _employeesList.Max(e => e.Id) + 1;
            _employeesList.Add(employees);
            return employees;
        }

        public IEnumerable<Employees> GetAllEmployee()
        {
            return _employeesList;
        }

        public Employees GetEmployees(int id)
        {
            return this._employeesList.FirstOrDefault(e => e.Id == id);
        }
    }
}
