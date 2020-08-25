using EmployeeManagement.Models;
using EmployeeManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IHostingEnvironment hostingEnvironment;
        private IEmployeeRepository _employeeRepository;
        private ILogger logger;
        public HomeController(IEmployeeRepository employeeRepository,
                              IHostingEnvironment hostingEnvironment,
                              ILogger<HomeController> logger)
        {
            _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.logger = logger;
        }
        [AllowAnonymous]
        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllEmployee();
            return View(model);          
        }     
        [AllowAnonymous]
        public ViewResult Details(int? id)        
        {
            logger.LogTrace("log trace message...!");
            logger.LogDebug("LogDebug message...!");
            logger.LogInformation("LogInformation message...!");
            logger.LogWarning("LogWarning message...!");
            logger.LogError("LogError message...!");
            Employees _employees = _employeeRepository.GetEmployees(id.Value);
            if(_employees == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                employees = _employees,
                PageTitle = "Employee Details"
        };
            return View(homeDetailsViewModel);          
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
                    if (employees.Photo != null)
                    {
                        UniqueFileName = ProcessUploadingPhoto(employees);
                    }
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
       [HttpGet]
        public ViewResult Edit(int id)
        {
            Employees employees = _employeeRepository.GetEmployees(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel()
            {
                Id = employees.Id,
                Name = employees.Name,
                Department = employees.Department,
                Email = employees.Email,
                ExistingPhotoPath= employees.PhotoPath              
            };
            return View(employeeEditViewModel);
        }
        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel employees)
        {
            string UniqueFileName = string.Empty;
            if (ModelState.IsValid)
            {
                Employees _emp = _employeeRepository.GetEmployees(employees.Id);
                _emp.Name = employees.Name;
                _emp.Email = employees.Email;
                _emp.Department = employees.Department;

                if (employees.Photo != null)
                {
                    if(employees.ExistingPhotoPath != null)
                    {
                        string filepath = Path.Combine(hostingEnvironment.WebRootPath,"images",employees.ExistingPhotoPath);
                        System.IO.File.Delete(filepath);
                    }
                    _emp.PhotoPath = ProcessUploadingPhoto(employees);
                }
               
                _employeeRepository.Update(_emp);
                return RedirectToAction("Index");
            }
            return View();
        }

        private string ProcessUploadingPhoto(EmployeeCreateViewModel employees)
        {
            string UniqueFileName;
            string UploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "Images");
            UniqueFileName = Guid.NewGuid().ToString() + "_" + employees.Photo.FileName;
            string FilePath = Path.Combine(UploadFolder, UniqueFileName);
            using (var filestream = new FileStream(FilePath, FileMode.Create))
            {
                employees.Photo.CopyTo(filestream);
            }
           
            return UniqueFileName;
        }

        public ViewResult Delete(int id)
        {
            _employeeRepository.Delete(id);
            var employees = _employeeRepository.GetAllEmployee();
            return View("Index", employees);
        }
    }
}
