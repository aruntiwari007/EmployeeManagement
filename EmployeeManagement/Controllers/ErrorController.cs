using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {          
           this.logger = logger;
        }
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statuscode)
        {
            switch(statuscode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, The resources you requested  could not be found";
                    break;
            }
            return View("NotFound");
        }
        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptiondetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewBag.ExceptionalPath = exceptiondetails.Path;
            ViewBag.ExceptionMessage = exceptiondetails.Error.Message;
            ViewBag.ExceptionStackTrace = exceptiondetails.Error.StackTrace;
            logger.LogError($"error occured duirng operation and error" +
                $" message is:  {exceptiondetails.Error} ");
           return View("Error");
        }
    }
}
