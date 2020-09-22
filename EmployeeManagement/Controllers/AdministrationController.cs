using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        public AdministrationController(RoleManager<IdentityRole> roleManager,
                                        UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel createRoleViewModel)
        {
            
            if(ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = createRoleViewModel.RoleName
                };
            IdentityResult Result = await roleManager.CreateAsync(identityRole);
                if(Result.Succeeded)
                {
                 return   RedirectToAction("Index", "Home");
                }
                foreach (var error in Result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            
            return View();
        }
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

       

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
         var role = await roleManager.FindByIdAsync(id);
            if(role==null)
            {
                ViewBag.ErrorMessage = $"Record not found against role id {id}";
                return View("Not found");
            }
            var model = new EditRoleViewModel
            {
                Id = role.Id,
               RoleName = role.Name
            };

            foreach(var user in userManager.Users.ToList())
            {
                
                if (await userManager.IsInRoleAsync(user, role.Name))                
                {
                    model.Users.Add(user.UserName);     
                }
            }
            return View(model);          
        }

        [HttpPost]
        public async Task<IActionResult> EditRole (EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Record not found against role id {model.Id}";
                return View("Not found");
            }
            else
            {
                role.Name = model.RoleName;
               var result  = await roleManager.UpdateAsync(role);
                if(result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }                   
        }

        public async Task<IActionResult> DeleteRole(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorMessage = $"Record not found against role id {id}";
                return View("Not found");
            }

            else
            {
                IdentityRole identityRole = await roleManager.FindByIdAsync(id);
                var result = await roleManager.DeleteAsync(identityRole);
                if(result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

               return View();
            }          
        }
        [HttpGet]
        public async Task<IActionResult> EditUserInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role  = await roleManager.FindByIdAsync(roleId);
            if (role ==  null)
            {
                ViewBag.ErrorMessage = $"Record not found against role id {roleId}";
                return View("Not found");
            }
            var model = new List<UserRoleViewModel>();
            foreach(var users in userManager.Users.ToList())
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserID = users.Id,
                    UserName = users.UserName
                };
                if(await userManager.IsInRoleAsync(users, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }
               return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserID);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }
    }
}
