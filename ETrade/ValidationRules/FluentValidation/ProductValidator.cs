using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETrade.Models;
using FluentValidation;

namespace ETrade.ValidationRules.FluentValidation
{
    public class ProductValidator:AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("Ürün adı boş bırakılamaz");
            RuleFor(x => x.ProductDescription).NotEmpty().WithMessage("Ürün açıklaması boş bırakılamaz");
            RuleFor(x => x.StockCount).NotEmpty().WithMessage("Ürün stok miktarı giriniz");
            RuleFor(x => x.UnitPrice).NotEmpty().WithMessage("Ürün fiyatı giriniz");
        }
    }
}