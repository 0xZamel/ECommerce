using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using ECommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace ECommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> productList = [.. _unitOfWork.Product.GetAll()];
            return View(productList);
        }
        public IActionResult Create()
        {
            ProductVM productVM = new()
            {
                Product = new Product(),

                Categories = _unitOfWork.Category.GetAll()
           .Select(c => new SelectListItem
           {
               Text = c.Name,
               Value = c.Id.ToString()
           })
           .ToList()
            };

            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductVM productVM)
        {
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        Console.WriteLine(
                            $"{item.Key}: {error.ErrorMessage}"
                        );
                    }
                }
            }

            if (ModelState.IsValid)
            {
                    if (productVM.Image != null) {
                    string rootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(productVM.Image.FileName);
                    string productPath = Path.Combine(rootPath, "images","products");
                    if (!Directory.Exists(productPath)) { 
                        Directory.CreateDirectory(productPath);
                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        productVM.Image.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = "/images/products/" + fileName;
                }
                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Save();

                _unitOfWork.AuditLog.Add(new AuditLog
                {
                    UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!,
                    Action = "Create",
                    EntityName = "Product",
                    EntityId = productVM.Product.Id,
                    Details = $"Created product '{productVM.Product.Name}'"
                });
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            productVM.Categories = _unitOfWork.Category.GetAll()
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
                .ToList();

            return View(productVM);
        }

        public IActionResult Edit(int id)
        {
            var product = _unitOfWork.Product.Get(p => p.Id == id);

            if (product == null)
                return NotFound();

            ProductVM productVM = new ProductVM
            {
                Product = product,

                Categories = _unitOfWork.Category.GetAll()
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    })
                    .ToList()
            };

            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var productFromDb = _unitOfWork.Product.Get(
                    p => p.Id == productVM.Product.Id
                );

                if (productFromDb == null)
                {
                    return NotFound();
                }

                if (productVM.Image != null)
                {
                    string rootPath = _webHostEnvironment.WebRootPath;

                    // Delete old image
                    if (!string.IsNullOrEmpty(productFromDb.ImageUrl))
                    {
                        string oldImagePath = Path.Combine(
                            rootPath,
                            productFromDb.ImageUrl.TrimStart('/')
                        );

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Save new image
                    string productPath = Path.Combine(
                        rootPath,
                        "images",
                        "products"
                    );

                    if (!Directory.Exists(productPath))
                    {
                        Directory.CreateDirectory(productPath);
                    }

                    string fileName =
                        Guid.NewGuid().ToString() +
                        Path.GetExtension(productVM.Image.FileName);

                    string filePath = Path.Combine(
                        productPath,
                        fileName
                    );

                    using (var fileStream = new FileStream(
                        filePath,
                        FileMode.Create))
                    {
                        productVM.Image.CopyTo(fileStream);
                    }

                    productFromDb.ImageUrl =
                        "/images/products/" + fileName;
                }

                // Update the product properties
                productFromDb.Name = productVM.Product.Name;
                productFromDb.Description = productVM.Product.Description;
                productFromDb.Price = productVM.Product.Price;
                productFromDb.CategoryId = productVM.Product.CategoryId;
                productFromDb.StockQuantity = productVM.Product.StockQuantity;

                _unitOfWork.Product.Update(productFromDb);
                _unitOfWork.Save();

                _unitOfWork.AuditLog.Add(new AuditLog
                {
                    UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!,
                    Action = "Edit",
                    EntityName = "Product",
                    EntityId = productVM.Product.Id,
                    Details = $"Edited product '{productVM.Product.Name}'"
                });
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            productVM.Categories = _unitOfWork.Category.GetAll()
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
                .ToList();

            return View(productVM);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? productFromDb = _unitOfWork.Product.Get(p => p.Id == id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(p => p.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(obj.ImageUrl))
            {
                string rootPath = _webHostEnvironment.WebRootPath;

                string imagePath = Path.Combine(
                    rootPath,
                    obj.ImageUrl.TrimStart('/')
                );

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();


            _unitOfWork.AuditLog.Add(new AuditLog
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!,
                Action = "Edit",
                EntityName = "Product",
                EntityId = obj.Id,
                Details = $"Edited product '{obj.Name}'"
            });
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }

    }
}
