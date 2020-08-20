using EmployeeManagement.Models;
using EmployeeManagement.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment hostingEnvironment;
        private IEmployeeRepository _employeeRepository;
        public HomeController(IEmployeeRepository employeeRepository, IHostingEnvironment hostingEnvironment)
        {
            _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
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
        public IActionResult Create(EmployeeCreateViewModel employees)
        {
            string UniqueFileName = string.Empty;
            if (ModelState.IsValid)
            {             
                if(employees.Photo != null)
                {
                    string UploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "Images");
                    UniqueFileName = Guid.NewGuid().ToString() + "_" + employees.Photo.FileName;
                    string FilePath = Path.Combine(UploadFolder, UniqueFileName);
                    employees.Photo.CopyTo(new FileStream(FilePath, FileMode.Create));
                }
                Employees emp = new Employees()
                {
                    Name=employees.Name,
                    Email = employees.Email,
                    Department =employees.Department,
                    PhotoPath = UniqueFileName
                };
                    _employeeRepository.Add(emp);
                return RedirectToAction("Details", new { id = emp.Id });
            }
            return View();
        }
    }
}
