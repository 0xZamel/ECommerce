using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Models.ViewModels
{
    public class OrderVM
    {
        public int Id { get; set; }

        public string UserName { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public DateTime OrderDate { get; set; }

        public decimal Total { get; set; }

        public List<OrderItemVM> OrderItems { get; set; } = new();
        public OrderStatus Status { get; set; }
        public bool IsPaid { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
    }
}
