using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ECommerce.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set;  }
        [Required]
        public int CartId { get; set; }

        [ForeignKey("CartId")]
        public Cart Cart { get; set; } = null!;
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; } = 0;

        public decimal Price { get; set; } = 0;


    }
}
