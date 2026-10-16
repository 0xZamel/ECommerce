using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using ECommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderController(IUnitOfWork unitOfWork , UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {

            var orders = _unitOfWork.Order
                .GetAll()
                .OrderBy(o=>o.OrderDate) 
                .ToList();

            var users = _userManager.Users.ToList();

            var orderVMs = new List<OrderVM>();
            
            foreach(var order in orders)
            {
                var user = users.FirstOrDefault(u => u.Id == order.UserId);
                orderVMs.Add(new OrderVM
                {
                    Id = order.Id,
                    UserName = user?.UserName ?? "Unknown",
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

        public IActionResult Details(int id)
        {
            var order = _unitOfWork.Order.Get(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            var user = _userManager.Users
                .FirstOrDefault(u => u.Id == order.UserId);

            var orderItems = _unitOfWork.OrderItem
                .GetAll()
                .Where(oi => oi.OrderId == order.Id)
                .ToList();

            var products = _unitOfWork.Product
                .GetAll()
                .ToList();

            var orderVM = new OrderVM
            {
                Id = order.Id,
                UserName = user?.UserName ?? "Unknown",
                Name = order.Name,
                Address = order.Address,
                PhoneNumber = order.PhoneNumber,
                OrderDate = order.OrderDate,
                Total = order.Total
            };

            foreach (var item in orderItems)
            {
                var product = products
                    .FirstOrDefault(p => p.Id == item.ProductId);

                if (product == null)
                    continue;

                orderVM.OrderItems.Add(new OrderItemVM
                {
                    ProductName = product.Name,
                    Price = item.Price,
                    Quantity = item.Quantity
                });
            }

            return View(orderVM);
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
        {
            var order = _unitOfWork.Order.Get(o => o.Id == id);

            if (order == null)
                return NotFound();

            if (order.Status != status)
            {
                order.Status = status;

                _unitOfWork.Order.Update(order);

                _unitOfWork.AuditLog.Add(new AuditLog
                {
                    UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!,
                    Action = "UpdateStatus",
                    EntityName = "Order",
                    EntityId = order.Id,
                    Details = $"Changed order '{order.Name}' status to '{order.Status}'"
                });

                _unitOfWork.Save();

                var user = await _userManager.FindByIdAsync(order.UserId);

                if (user != null && !string.IsNullOrEmpty(user.Email))
                {
                    await _emailSender.SendEmailAsync(
                        user.Email,
                        $"Order #{order.Id} Status Updated",
                        $@"
                    <h2>Order Status Updated</h2>

                    <p>Hello {order.Name},</p>

                    <p>
                        Your order <strong>#{order.Id}</strong>
                        status has been changed to:
                    </p>

                    <h3>{order.Status}</h3>

                    <p>Thank you for shopping with us.</p>
                "
                    );
                }
            }

            return RedirectToAction("Index");
        }
    }
}
