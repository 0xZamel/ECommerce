using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public WishlistController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var wishlist = _unitOfWork.Wishlist.GetAll(
                w => w.UserId == userId,
                includeProperties: "Product"
            );

            return View(wishlist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existing = _unitOfWork.Wishlist.Get(
                w => w.UserId == userId && w.ProductId == productId);

            if (existing == null)
            {
                _unitOfWork.Wishlist.Add(new Wishlist
                {
                    UserId = userId!,
                    ProductId = productId
                });

                _unitOfWork.Save();
            }

            return RedirectToAction(
                "Details",
                "Product",
                new { area = "Customer", id = productId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var wishlist = _unitOfWork.Wishlist.Get(
                w => w.Id == id && w.UserId == userId);

            if (wishlist != null)
            {
                _unitOfWork.Wishlist.Remove(wishlist);
                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MoveToCart(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var wishlist = _unitOfWork.Wishlist.Get(
                w => w.Id == id && w.UserId == userId
            );

            if (wishlist == null)
            {
                return NotFound();
            }

            var product = _unitOfWork.Product.Get(
                p => p.Id == wishlist.ProductId
            );

            if (product == null || product.StockQuantity <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var cart = _unitOfWork.Cart.Get(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId!
                };

                _unitOfWork.Cart.Add(cart);
                _unitOfWork.Save();
            }

            var cartItem = _unitOfWork.CartItem.Get(
                c => c.CartId == cart.Id &&
                     c.ProductId == product.Id
            );

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = product.Id,
                    Quantity = 1
                };

                _unitOfWork.CartItem.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }

            _unitOfWork.Wishlist.Remove(wishlist);

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}
