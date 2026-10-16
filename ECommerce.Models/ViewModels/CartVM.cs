using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerce.Models.ViewModels
{
    public class CartVM
    {
        public List<CartItemVM> CartItems { get; set; } = new();

        public decimal Total { get; set; }
    }
}
