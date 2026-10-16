using ECommerce.DataAccess.Repository;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using ECommerce.Models.ViewModels;
using ECommerce.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ECommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        public UserController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            var users = _userService.GetAll();
            var userVMs = users.Select(u => new UserVM
            {
                Id = u.Id,
                UserName = u.Email!,
                Email = u.Email!,
                FullName = u.FullName,
                Address = u.Address,
                Role = _userService.GetRoles(u).FirstOrDefault()
            }).ToList();

            return View(userVMs);
        }

        public IActionResult Create()
        {
            UserVM model = new UserVM
            {
                Roles = _roleService.GetAll().Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                }).ToList()
            };
                
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = userVM.Email,
                    Email = userVM.Email,
                    FullName = userVM.FullName,
                    Address = userVM.Address,

                };
                var result = _userService.Create(user, userVM.Password!);

                if (result.Succeeded)
                {
                    _userService.AssignRole(user.Id, userVM.Role);
                    return RedirectToAction("Index");
                }

            }
            
            return View(userVM);
        }

        public IActionResult Edit(string id)
        {
            var user = _userService.Get(id);

            if (user == null)
                return NotFound();

            var model = new UserVM
            {
                Id = user.Id,
                UserName = user.Email!,
                Email = user.Email!,
                FullName = user.FullName!,
                Address = user.Address!,

                Role = _userService.GetRoles(user).FirstOrDefault(),
                Roles = _roleService
                .GetAll()
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                })
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserVM model)
        {

            if (!ModelState.IsValid)
            {
                model.Roles = _roleService.GetAll()
                    .Select(r => new SelectListItem
                    {
                        Text = r.Name,
                        Value = r.Name
                    });

                return View(model);
            }

            var user = _userService.Get(model.Id!);

            if (user == null)
            {
                return NotFound();
            }

            user.UserName = model.Email;
            user.Email = model.Email;
            user.FullName = model.FullName;
            user.Address = model.Address;

            var result = _userService.Update(user);
            if (result.Succeeded)
            {
                var currentRole = _userService.GetRoles(user).FirstOrDefault();

                if (currentRole != model.Role)
                {
                    if (!string.IsNullOrEmpty(currentRole))
                    {
                        _userService.RemoveRole(user.Id, currentRole);
                    }

                    if (!string.IsNullOrEmpty(model.Role))
                    {
                        _userService.AssignRole(user.Id, model.Role);
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            if (result.Succeeded)
                return RedirectToAction("Index");
            return View(model);
        }
        public IActionResult Delete(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            UserVM model = new UserVM
            {
                Id = user.Id,
                UserName = user.Email!,
                Email = user.Email!,
                FullName = user.FullName!,
                Address = user.Address!
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var result = _userService.Delete(id);

            if (result.Succeeded)
            {
                TempData["success"] = "User deleted successfully.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            var user = _userService.Get(id);

            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            UserVM model = new UserVM
            {
                Id = user.Id,
                UserName = user.Email!,
                Email = user.Email!,
                FullName = user.FullName!,
                Address = user.Address!
            };

            return View(model);
        }


    }
}
