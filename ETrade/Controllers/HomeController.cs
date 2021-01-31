using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETrade.Contexts;
using ETrade.Models.ViewModels;
using ETrade.Repository;
using ETrade.Repository.IRepository;

namespace ETrade.Controllers
{
    public class HomeController : Controller
    {
        //TODO:LazyLoad , Pagination , RenderScript, CDN Or Bundle
        private ICategoryRepository _categoryRepository;
        public HomeController()
        {
            _categoryRepository = new CategoryRepository(new ETradeContext());
        }

        // GET: Home
        public ActionResult Index()
        {
            CategoryListViewModel model = new CategoryListViewModel()
            {
                Categories = _categoryRepository.GetList()
            };

            int totalcat=0;
            foreach (var item in model.Categories)
            {
                totalcat += item.Products.Count;
            }

            ViewBag.totalCategory = totalcat.ToString();


            return View(model);
        }
    }
}