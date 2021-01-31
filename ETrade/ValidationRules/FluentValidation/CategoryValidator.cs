using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETrade.Models;
using FluentValidation;

namespace ETrade.ValidationRules.FluentValidation
{
    public class CategoryValidator:AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.CategoryName).NotEmpty().WithName("Kategori gerekli");
        }
    }
}