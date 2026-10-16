using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerce.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
        public List<OrderItem> OrderItems { get; set; } = [];
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public bool IsPaid { get; set; } = false;

        public PaymentMethod PaymentMethod { get; set; }
        public string? PaymentTransactionId { get; set; }

    }
}
