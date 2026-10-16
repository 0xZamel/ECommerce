using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECommerce.Models
{
    public class Category
    {

        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Category Name")]
        [MaxLength(30)]
        public string Name { get; set; } = null!;
        [Display(Name = "Display Order")]
        [Range(1, 100)]
        public int DisplayOrder { get; set; }

    }
}
