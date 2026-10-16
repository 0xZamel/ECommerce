using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerce.Models.ViewModels
{
    public class RoleVM
    {
        public string? Id { get; set; }
        [Required]
        [Display(Name = "Role Name")]
        public string Name { get; set; } = null!;
    }
}
