using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using ECommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public DashboardController(IUnitOfWork db) { _unitOfWork = db; }
        public IActionResult Index()
        {
            var orders = _unitOfWork.Order.GetAll();
            var orderItems = _unitOfWork.OrderItem.GetAll(includeProperties: "Order,Product");
            var products = _unitOfWork.Product.GetAll();

            AdminDashboardVM obj = new AdminDashboardVM();

            obj.TotalProducts = products.Count();

            obj.OutOfStockProducts = products
                .Count(p => p.StockQuantity <= 0);

            obj.LowStockProducts = products
                .Where(p => p.StockQuantity > 0 && p.StockQuantity <= 5)
                .Select(p => new LowStockProductVM
                {
                    Id = p.Id,
                    ProductName = p.Name,
                    StockQuantity = p.StockQuantity
                })
                .ToList();

            obj.TotalOrders = orders.Count();

            obj.PendingOrders = orders
                .Count(o => o.Status == OrderStatus.Pending);

            obj.TotalRevenue = orders
                .Where(o => o.Status == OrderStatus.Completed)
                .Sum(o => o.Total);

            obj.TotalCustomers = orders
                .Select(o => o.UserId)
                .Distinct()
                .Count();

            obj.BestSellingProducts = orderItems
                .Where(i => i.Order.Status == OrderStatus.Completed)
                .GroupBy(i => i.ProductId)
                .Select(i => new BestSellingProductVM
                {
                    ProductName = i.First().Product.Name,
                    QuantitySold = i.Sum(oi => oi.Quantity),
                    Revenue = i.Sum(oi => oi.Quantity * oi.Price)
                })
                .OrderByDescending(i => i.QuantitySold)
                .Take(10)
                .ToList();


            return View(obj);
        }
    }
}
