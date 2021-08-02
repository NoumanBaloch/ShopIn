using ShopIn.Core.Contracts;
using ShopIn.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopIn.WebUI.Controllers
{
    public class OrderManagerController : Controller
    {
        IOrderService _orderService;

        public OrderManagerController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        // GET: OrderManager
        public ActionResult Index()
        {
            List<Order> orders = _orderService.GetOrderList();

            return View(orders);
        }
        

        public ActionResult UpdateOrder(string id)
        {
            ViewBag.StatusList = new List<string>()
            {
                "Order Created",
                "Payment Processed",
                "Order Shipped",
                "Order Complete"
            };
            Order order = _orderService.GetOrder(id);
            return View(order);
        }

        [HttpPost]
        public ActionResult UpdateOrder(Order updatedOrder, string id)
        {
            Order order = _orderService.GetOrder(id);

            if(order != null)
            {
                order.OrderStatus = updatedOrder.OrderStatus;
                _orderService.UpdateOrder(order);
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
           

        }
    }
}