using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using ETrade.Contexts;
using ETrade.Models;
using ETrade.Repository.IRepository;

namespace ETrade.Repository
{
    public class CategoryRepository:ICategoryRepository
    {
        private ETradeContext _eTradeContext;
        public CategoryRepository(ETradeContext eTradeContext)
        {
            _eTradeContext = eTradeContext;
        }

        public List<Category> GetList(Expression<Func<Category, bool>> filter = null)
        {
            return filter == null
                ? _eTradeContext.Categories.Include(x=>x.Products).ToList()
                : _eTradeContext.Categories.Include(x => x.Products).Where(filter).ToList();
        }

        public Category Get(Expression<Func<Category, bool>> filter)
        {
            return _eTradeContext.Categories.FirstOrDefault(filter);
        }

        public void Add(Category entity)
        {
            var addedEntity = _eTradeContext.Entry(entity);
            addedEntity.State = EntityState.Added;
            _eTradeContext.SaveChanges();
        }

        public void Update(Category entity)
        {
            var updatedEntity = _eTradeContext.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            _eTradeContext.SaveChanges();
        }

        public void Delete(Category entity)
        {
            var deletedEntity = _eTradeContext.Entry(entity);
            deletedEntity.State = EntityState.Deleted;
            _eTradeContext.SaveChanges();
        }
    }
}