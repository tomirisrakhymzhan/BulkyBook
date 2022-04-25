using System;
using System.Collections.Generic;
namespace BulkyBook.Models.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ListCart { get; set; }
        public double CartTotal { get; set; }
    }
}
