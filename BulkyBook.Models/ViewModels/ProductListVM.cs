using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.Models.ViewModels
{
    public class ProductListVM
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public string Category { get; set; }
        public string SearchString { get; set; }
        public string Author { get; set; }

    }
}
