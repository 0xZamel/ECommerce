using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Models.ViewModels
{
    public class LowStockProductVM
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = null!;
        public int StockQuantity { get; set; }
    }
}
