using ShopIn.Core.Contracts;
using ShopIn.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopIn.WebUI.Controllers
{
    public class BasketController : Controller
    {
        IBasketService _basketService;
        IOrderService _orderService;
        IRepository<Customer> _customer;
        public BasketController(IBasketService basketService, IOrderService orderService, IRepository<Customer> customer)
        {
            this._basketService = basketService;
            this._orderService = orderService;
            this._customer = customer;
        }
        // GET: Basket
        public ActionResult Index()
        {
            var model = _basketService.GetBasketItems(this.HttpContext);
            return View(model);
        }

        public ActionResult AddToBasket(string Id)
        {
            _basketService.AddToBasket(this.HttpContext, Id);

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromBasket(string Id)
        {
            _basketService.RemoveFromBasket(this.HttpContext, Id);

            return RedirectToAction("Index");
        }

        public PartialViewResult BasketSummary()
        {
            var basketSummary = _basketService.GetBasketSummary(this.HttpContext);
            return PartialView(basketSummary);
        }

        [Authorize]
        public ActionResult Checkout()
        {
            Customer customer = _customer.Collection().FirstOrDefault(x => x.Email == User.Identity.Name);

            if (customer != null)
            {
                Order order = new Order()
                {
                    Email = customer.Email,
                    City = customer.City,
                    State = customer.State,
                    FirstName = customer.FirstName,
                    SurName = customer.LastName,
                    ZipCode = customer.ZipCode,
                    Street = customer.Street
                };
                return View(order);
            }
            return RedirectToAction("Error");
            
        }

        public ActionResult ThankYou(string OrderId)
        {
            ViewBag.OrderId = OrderId;
            return View();
        }

        [HttpPost]
        public ActionResult CheckOut(Order order)
        {
            
            var basketItems = _basketService.GetBasketItems(this.HttpContext);
            order.OrderStatus = "Order Created";
            order.Email = User.Identity.Name;

            //Payment Processing

            order.OrderStatus = "Payment Proceesed";
            _orderService.CreateOrder(order, basketItems);
            _basketService.ClearBasket(this.HttpContext);

            return RedirectToAction("ThankYou", new { OrderId = order.Id });
        }
    }
}