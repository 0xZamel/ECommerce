using ECommerce.DataAccess.Repository;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using ECommerce.Models.ViewModels;
using ECommerce.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            var roles = _roleService.GetAll();

            var model = roles.Select(r => new RoleVM
            {
                Id = r.Id,
                Name = r.Name!
            }).ToList();

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RoleVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var role = new IdentityRole
            {
                Name = model.Name
            };

            var result = _roleService.Create(role);

            if (result.Succeeded)
            {
                TempData["success"] = "Role created successfully.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        public IActionResult Edit(string id)
        {
            var role = _roleService.Get(id);

            if (role == null)
            {
                return NotFound();
            }

            RoleVM model = new RoleVM
            {
                Id = role.Id,
                Name = role.Name!
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RoleVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var role = new IdentityRole
            {
                Id = model.Id!,
                Name = model.Name
            };

            var result = _roleService.Update(role);

            if (result.Succeeded)
            {
                TempData["success"] = "Role updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        public IActionResult Delete(string id)
        {
            var role = _roleService.Get(id);

            if (role == null)
            {
                return NotFound();
            }

            RoleVM model = new RoleVM
            {
                Id = role.Id,
                Name = role.Name!
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeletePost(string id)
        {
            var result = _roleService.Delete(id);

            if (result.Succeeded)
            {
                TempData["success"] = "Role deleted successfully.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Delete), new { id });
        }

    }
}
