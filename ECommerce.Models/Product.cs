using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ECommerce.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; } = null!;
        public string? ImageUrl { get; set; }

        public int StockQuantity { get; set; }
        public List<Review> Reviews { get; set; } = [];
    }
}
