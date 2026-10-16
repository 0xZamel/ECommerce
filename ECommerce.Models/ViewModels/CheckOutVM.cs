using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerce.Models.ViewModels
{
    public class CheckOutVM
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        public decimal Total { get; set; }
    }
}
