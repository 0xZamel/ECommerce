using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; } = new();

        public List<SelectListItem> Categories { get; set; } = [];
        public IFormFile? Image { get; set; }
    }
}
