using ShopIn.Core.Contracts;
using ShopIn.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopIn.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IRepository<Product> _productContext;
        IRepository<ProductCategory> _productCategoryContext;

        public HomeController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            this._productContext = productContext;
            this._productCategoryContext = productCategoryContext;
        }
        public ActionResult Index()
        {
            List<Product> products = _productContext.Collection().ToList();
            return View(products);
        }

        public ActionResult Details(string Id)
        {
            Product product = _productContext.Find(Id);

            if(product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}