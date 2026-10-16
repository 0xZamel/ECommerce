using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using ECommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProductController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index(int? categoryId)
        {
            var products = _unitOfWork.Product.GetAll();

            if (categoryId.HasValue)
            {
                products = [.. products.Where(p => p.CategoryId == categoryId.Value)];
            }

            ViewBag.Categories = _unitOfWork.Category.GetAll();

            return View(products.ToList());
        }
        public IActionResult Details(int id)
        {
            var product = _unitOfWork.Product.Get(
                p => p.Id == id, "Category,Reviews.User"
            );

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(ReviewVM reviewVM)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(
                    nameof(Details),
                    new { id = reviewVM.ProductId });
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var product = _unitOfWork.Product.Get(p => p.Id == reviewVM.ProductId);
            if (product == null)
            {
                return NotFound();
            }

            var review = new Review
            {
                ProductId = reviewVM.ProductId,
                UserId = user.Id,
                Rating = reviewVM.Rating,
                Comment = reviewVM.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWork.Review.Add(review);
            _unitOfWork.Save();

            return RedirectToAction(
                nameof(Details),
                new { id = reviewVM.ProductId });
        }
    }
}
