using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModel
{
    public class HomeDetailsViewModel
    {
        public Employees employees { get; set; }
        public string PageTitle { get; set; }
    }
}
