using ShopIn.Core.Contracts;
using ShopIn.Core.Models;
using ShopIn.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopIn.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {
        IRepository<ProductCategory> _context;
        public ProductCategoryManagerController(IRepository<ProductCategory> productCategoryContext)
        {
            _context = productCategoryContext;
        }
        
        // GET: ProductManager
        public ActionResult Index()
        {
            IEnumerable<ProductCategory> productCategories = _context.Collection().ToList();
            return View(productCategories);
        }

        public ActionResult Create()
        {
            ProductCategory productCategory = new ProductCategory();
            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(productCategory);
            }
            _context.Insert(productCategory);
            _context.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string Id)
        {
            ProductCategory productCategory = _context.Find(Id);

            if(productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory, string Id)
        {
            ProductCategory productCategoryToEdit = _context.Find(Id);
            if(productCategoryToEdit == null)
            {
                return HttpNotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(productCategory);
            }


            productCategoryToEdit.Category = productCategory.Category;

            _context.Update(productCategoryToEdit);
            _context.Commit();

            return RedirectToAction("Index");
        }

        public ActionResult Delete(string Id)
        {
            ProductCategory productCategoryToDelete = _context.Find(Id);
            if(productCategoryToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productCategoryToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            ProductCategory productCategoryToDelete = _context.Find(Id);

            if(productCategoryToDelete == null)
            {
                return HttpNotFound();
            }

            _context.Delete(Id);

            _context.Commit();

            return RedirectToAction("Index");
        }
    }
}