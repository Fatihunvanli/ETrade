using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETrade.Models;

namespace ETrade.Repository.IRepository
{
    public interface IProductRepository:IRepository<Product>
    {
        List<Product> GetByCategory(int categoryId);
    }
}
