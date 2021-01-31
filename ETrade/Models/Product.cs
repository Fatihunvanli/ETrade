using ETrade.ValidationRules.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace ETrade.Models
{
    [FluentValidation.Attributes.Validator(typeof(ProductValidator))]
    public class Product
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductPicture { get; set; }
        public decimal UnitPrice { get; set; }
        public int StockCount { get; set; }
        public virtual Category Category { get; set; }
    }
}