using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Models.ViewModels
{
    public class BestSellingProductVM
    {
        public string ProductName { get; set; } = null!;
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
    }
}
