using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using ETrade.Contexts;
using ETrade.Repository.IRepository;

namespace ETrade.Models
{
    public class ProductRepository:IProductRepository
    {
        private ETradeContext _eTradeContext;
        public ProductRepository(ETradeContext eTradeContext)
        {
            _eTradeContext = eTradeContext;
        }
        public List<Product> GetList(Expression<Func<Product, bool>> filter = null)
        {
            return filter == null
                ? _eTradeContext.Products.Include(x => x.Category).ToList()
                : _eTradeContext.Products.Include(x => x.Category).Where(filter).ToList();
        }

        public Product Get(Expression<Func<Product, bool>> filter)
        {
            return _eTradeContext.Products.Include(x => x.Category).FirstOrDefault(filter);
        }

        public void Add(Product entity)
        {
            var addedEntity = _eTradeContext.Entry(entity);
            addedEntity.State = EntityState.Added;
            _eTradeContext.SaveChanges();
        }

        public void Update(Product entity)
        {
            var updatedEntity = _eTradeContext.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            _eTradeContext.SaveChanges();
        }

        public void Delete(Product entity)
        {
            var deletedEntity = _eTradeContext.Entry(entity);
            deletedEntity.State = EntityState.Deleted;
            _eTradeContext.SaveChanges();
        }

        public List<Product> GetByCategory(int categoryId)
        {
            return _eTradeContext.Products.Include(x => x.Category).Where(x => x.CategoryId == categoryId).ToList();
        }
    }
}