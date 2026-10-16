using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Models.ViewModels
{
    public class AdminDashboardVM
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalProducts { get; set; }

        public int PendingOrders { get; set; }
        public int OutOfStockProducts { get; set; }

        public List<BestSellingProductVM> BestSellingProducts { get; set; } = [];
        public List<LowStockProductVM> LowStockProducts { get; set; } = [];
    }
}
