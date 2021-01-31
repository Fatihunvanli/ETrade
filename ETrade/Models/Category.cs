using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETrade.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}