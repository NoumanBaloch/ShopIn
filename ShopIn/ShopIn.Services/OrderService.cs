using ShopIn.Core.Contracts;
using ShopIn.Core.Models;
using ShopIn.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ShopIn.Services
{
    public class OrderService : IOrderService
    {
        IRepository<Order> _orderContext;
        public OrderService(IRepository<Order> orderContext)
        {
            _orderContext = orderContext;
        }

        public void CreateOrder(Order baseOrder, List<BasketItemViewModel> basketItems)
        {
            foreach(var item in basketItems)
            {
                baseOrder.OrderItems.Add(new OrderItem()
                {
                    ProductId = item.Id,
                    Image = item.Image,
                    Price = item.Price,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity
                });
            }

            _orderContext.Insert(baseOrder);
            _orderContext.Commit();
        }
        
    }
}
