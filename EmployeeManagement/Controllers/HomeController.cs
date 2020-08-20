using EmployeeManagement.Models;
using EmployeeManagement.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private IEmployeeRepository _employeeRepository;
        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllEmployee();
            return View(model);
                //Json(new { id = 1, name = "arun" });
        }
        public ViewResult Details(int? id)        
        {           
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                employees = _employeeRepository.GetEmployees(id??1),
                PageTitle = "Employee Details"
        };
            return View(homeDetailsViewModel);
            //Json(new { id = 1, name = "arun" });
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employees employees)
        {
            if (ModelState.IsValid)
            {
                Employees emp = _employeeRepository.Add(employees);
                return RedirectToAction("Details", new { id = emp.Id });
            }
            return View();
        }
    }
}
