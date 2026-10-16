using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.Models.ViewModels
{
    public class UserVM
    {
        public string? Id { get; set; }
        public string? UserName { get; set; } 

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]

        [Display(Name = "Full Name")]
        public string FullName { get; set; } = null!;
        
        [DataType(DataType.Password)]
        public string? Password { get; set; } 

        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Display(Name = "Role")]
        public string? Role { get; set; }
        public IEnumerable<SelectListItem>? Roles { get; set; }
    }
}
