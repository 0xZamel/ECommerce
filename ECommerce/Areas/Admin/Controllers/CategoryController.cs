using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork db) { _unitOfWork = db; }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();


                _unitOfWork.AuditLog.Add(new AuditLog
                {
                    UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!,
                    Action = "Create",
                    EntityName = "Product",
                    EntityId = obj.Id,
                    Details = $"Created Category '{obj.Name}'"
                });
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? catFromDb = _unitOfWork.Category.Get(c => c.Id == id);
            if (catFromDb == null)
            {
                return NotFound();
            }
            return View(catFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();


                _unitOfWork.AuditLog.Add(new AuditLog
                {
                    UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!,
                    Action = "Edit",
                    EntityName = "Category",
                    EntityId = obj.Id,
                    Details = $"Edited category '{obj.Name}'"
                });
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? catFromDb = _unitOfWork.Category.Get(c => c.Id == id);
            if (catFromDb == null)
            {
                return NotFound();
            }
            return View(catFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(c => c.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();

            _unitOfWork.AuditLog.Add(new AuditLog
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!,
                Action = "Delete",
                EntityName = "Category",
                EntityId = obj.Id,
                Details = $"Deleted category '{obj.Name}'"
            });
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }

    }
}
