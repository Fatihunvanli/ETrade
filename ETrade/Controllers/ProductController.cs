using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.Mvc;
using ETrade.Contexts;
using ETrade.Models;
using ETrade.Models.ViewModels;
using ETrade.Repository;
using ETrade.Repository.IRepository;

namespace ETrade.Controllers
{
    public class ProductController:Controller
    {
        private IProductRepository _productRepository;
        private ICategoryRepository _categoryRepository;
        public ProductController()
        {
            _productRepository = new ProductRepository(new ETradeContext());
            _categoryRepository = new CategoryRepository(new ETradeContext());
        }
        /// <summary>
        /// Kategoriler çekildi partial döndürüldü
        /// </summary>
        /// <param name="category">Tüm kategorileri çekmek için deger sıfır 0 verildi</param>
        /// <returns>Oluşturulan Partial a  çekilen kategoriler verilerek gönderildi</returns>
        public ActionResult Index(int category=0)
        {
            var model = new ProductListViewModel()
            {
                Products = category > 0 ? _productRepository.GetByCategory(category) : _productRepository.GetList()
            };

            return PartialView("_ProductListPartial",model);
        }
        /// <summary>
        /// Tüm ürünler db den çekildi. Doldurulan model view e gönderildi.
        /// </summary>
        /// <returns>Model doldurularak view döndürüldü</returns>
        public ActionResult ProductManager()
        {
            var model = new ProductListViewModel()
            {
                Products = _productRepository.GetList()
            };

            return View(model);
        }
        /// <summary>
        /// Gelen ürün id değerine göre ürün detay sayfasına yönlendirme.
        /// </summary>
        /// <param name="id">Ürün id</param>
        /// <returns>Model doldurularak view döndürüldü</returns>
        public ActionResult ProductDetail(int id)
        {
            if (id == 0 || id ==null)
            {
                TempData["Danger"] = "Bir Hata Oluştu.";
                return RedirectToAction("ProductManager", "Product");
            }
            var model = _productRepository.Get(p => p.Id == id);

            return View(model);
        }
        /// <summary>
        /// Gelen ürün idsine göre ürün silme işlemi yapıldı. Ürün silme işleminin başarılı veya başarısız olması durumuna göre TempData ile alert verildi.
        /// </summary>
        /// <param name="product">Gelen ürün bilgileri</param>
        /// <returns>Belirlenen action döndürüldü</returns>
        public ActionResult ProductDelete(Product product)
        {
            if (!ModelState.IsValid)
            {
                TempData["Danger"] = "Bir Hata Oluştu.";
                return RedirectToAction("ProductManager", "Product");
            }

            _productRepository.Delete(product);

            TempData["Success"]="Ürün Silindi.";

            return RedirectToAction("ProductManager", "Product");
        }
        /// <summary>
        ///  Ürün bilgileri düzenleme metodu
        /// </summary>
        /// <param name="id">Gelen ürün id</param>
        /// <returns>İlgili actiona gidildi.</returns>
        [HttpGet]
        public ActionResult ProductEdit(int id)
        {

            if (id == 0 || id == null)
            {
                TempData["Danger"] = "Bir Hata Oluştu.";
                return RedirectToAction("ProductManager", "Product");
            }

            var model = _productRepository.Get(p => p.Id == id);

            var categories = _categoryRepository.GetList();

            List<SelectListItem> categorySelectList = new List<SelectListItem>();

            foreach (var item in categories)
            {
                categorySelectList.Add(new SelectListItem() { Text = item.CategoryName, Value = item.Id.ToString(), Selected = (item.Id == model.CategoryId ? true : false), });
            }
            ViewBag.Cat = categorySelectList;

            return View(model);
        }
        /// <summary>
        /// Gelen ürüne göre ürün bilgilerini değiştirdiğimiz metod
        /// </summary>
        /// <param name="product">Gelen ürün</param>
        /// <param name="file">Fotoğraf değişikliği</param>
        /// <returns>Gelen verilerin doğruluğuna göre ilgili action çalışıyor</returns>
        [HttpPost]
        public ActionResult ProductEdit(Product product, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                TempData["Danger"] = "Bir Hata Oluştu.";
                return RedirectToAction("ProductManager", "Product");
            }

            if (file != null && file.ContentLength > 0)
            {
                
                if (file.ContentType == "image/jpeg" || file.ContentType == "image/jpg" || file.ContentType == "image/png" || file.ContentType == "image/gif")
                {
                var fi = new FileInfo(file.FileName);
                var fileName = Path.GetFileName(file.FileName);
                //fileName = Guid.NewGuid().ToString() + fi.Extension;
                var path = Path.Combine(Server.MapPath("~/Content/img/"), fileName);
                product.ProductPicture = fileName;
                file.SaveAs(path);
                }
            }
            
            _productRepository.Update(product);

            TempData["Success"] = "Ürün Güncellendi.";

            return RedirectToAction("ProductEdit", "Product",product.Id);
        }
        /// <summary>
        /// Ürün ekleme metodu
        /// </summary>
        /// <returns>View döndürüldü</returns>
        [HttpGet]
        public ActionResult ProductAdd()
        {

            var categories = _categoryRepository.GetList();

            List<SelectListItem> categorySelectList = new List<SelectListItem>();

            foreach (var item in categories)
            {
                categorySelectList.Add(new SelectListItem() { Text = item.CategoryName, Value = item.Id.ToString(), Selected = (item.Id == 1 ? true : false), });
            }

            ViewBag.Catagories = categorySelectList;

            return View(new Product());
        }
        /// <summary>
        /// Gelen ürünün eklenmesi
        /// </summary>
        /// <param name="product">Kullanıcının girdiği ürün bilgisi</param>
        /// <param name="file">Kullanıcının yüklediği resim bilgisi</param>
        /// <returns>Tüm ürünlerin bulunduğu view e gidildi</returns>
        [HttpPost]
        public ActionResult ProductAdd(Product product, HttpPostedFileBase file)
        {

            var categories = _categoryRepository.GetList();

            List<SelectListItem> categorySelectList = new List<SelectListItem>();

            foreach (var item in categories)
            {
                categorySelectList.Add(new SelectListItem() { Text = item.CategoryName, Value = item.Id.ToString(), Selected = (item.Id == 1 ? true : false), });
            }

            ViewBag.Catagories = categorySelectList;

            if (!ModelState.IsValid)
            {
                return View("ProductAdd",product);
            }

            if (file != null && file.ContentLength > 0)
            {

                if (file.ContentType == "image/jpeg" || file.ContentType == "image/jpg" || file.ContentType == "image/png" || file.ContentType == "image/gif")
                {
                    var fi = new FileInfo(file.FileName);
                    var fileName = Path.GetFileName(file.FileName);
                    //fileName = Guid.NewGuid().ToString() + fi.Extension;
                    var path = Path.Combine(Server.MapPath("~/Content/img/"), fileName);
                    product.ProductPicture = fileName;
                    file.SaveAs(path);
                }
            }

            _productRepository.Add(product);

            TempData["Success"] = "Ürün Eklendi.";

            return RedirectToAction("Index","Home");
        }

    }
}