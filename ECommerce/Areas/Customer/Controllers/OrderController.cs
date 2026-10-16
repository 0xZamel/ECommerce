using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using ECommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = _unitOfWork.Order.GetAll(
                o => o.UserId == userId
            ).ToList();
            var orderVMs = new List<OrderVM>();
            foreach (var order in orders) {
                orderVMs.Add(new OrderVM
                {
                    Id = order.Id,
                    Name = order.Name,
                    PhoneNumber = order.PhoneNumber,
                    Address = order.Address,
                    OrderDate = order.OrderDate,
                    Total = order.Total,
                    Status = order.Status,
                    IsPaid = order.IsPaid,
                    PaymentMethod = order.PaymentMethod

                });
            }
            return View(orderVMs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = _unitOfWork.Order.Get(
                o => o.Id == id && o.UserId == userId
            );

            if (order == null)
                return NotFound();

            if (order.Status != OrderStatus.Pending)
                return BadRequest();

            order.Status = OrderStatus.Cancelled;

            _unitOfWork.Order.Update(order);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Details(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = _unitOfWork.Order.Get(
                o => o.Id == id && o.UserId == userId,
                includeProperties: "OrderItems.Product"
            );

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
