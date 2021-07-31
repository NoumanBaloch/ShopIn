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
    public class BasketService : IBasketService
    {
        IRepository<Product> _productContext;
        IRepository<Basket> _basketContext;

        public const string BasketSessionName = "ShopIneComBasket";
        public BasketService(IRepository<Product> productContext, IRepository<Basket> basketContext)
        {
            this._productContext = productContext;
            this._basketContext = basketContext;
        }

        private Basket GetBasket(HttpContextBase httpContext, bool createIfNull)
        {
           HttpCookie cookie =  httpContext.Request.Cookies.Get(BasketSessionName);
            Basket basket = new Basket();
            if(cookie != null)
            {
                string basketID = cookie.Value;
                if (!string.IsNullOrWhiteSpace(basketID))
                {
                    basket = _basketContext.Find(basketID);
                }
                else
                {
                    if (createIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }
            }
            else
            {
                if (createIfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }

            return basket;
        }

        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            Basket basket = new Basket();
            _basketContext.Insert(basket);
            _basketContext.Commit();

            HttpCookie cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(1);
            httpContext.Response.Cookies.Add(cookie);

            return basket;
        }

        public void AddToBasket(HttpContextBase httpContext, string productId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem  item = basket.BasketItems.FirstOrDefault(x => x.ProductId == productId);

            if(item == null)
            {
                item = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1
                };

                basket.BasketItems.Add(item);
            }
            else
            {
                item.Quantity += 1;
            }

            _basketContext.Commit();
        }

        public void RemoveFromBasket(HttpContextBase httpContext, string itemId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(x => x.Id == itemId);

            if(item != null)
            {
                basket.BasketItems.Remove(item);
                _basketContext.Commit();
            }

        }

        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);

            if(basket != null)
            {
                var result = (from b in basket.BasketItems
                              join p in _productContext.Collection() on
                              b.ProductId equals p.Id
                              select new BasketItemViewModel()
                              {
                                  Id = b.Id,
                                  Quantity = b.Quantity,
                                  ProductName = p.Name,
                                  Image = p.Image,
                                  Price = p.Price
                             }).ToList();
                return result;
            }
            else
            {
                return new List<BasketItemViewModel>();
            }
        }

        public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            BasketSummaryViewModel model = new BasketSummaryViewModel(0, 0);

            if (basket == null)
            {
                return model;
               
            }
            int? basketCount = (from item in basket.BasketItems
                                select item.Quantity).Sum();

            decimal? basketTotal = (from item in basket.BasketItems
                                    join p in _productContext.Collection()
                                    on item.ProductId equals p.Id
                                    select item.Quantity * p.Price).Sum();

            model.BasketCount = basketCount ?? 0;
            model.BasketTotal = basketTotal ?? 0;

            return model;

        }

        public void ClearBasket(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            basket.BasketItems.Clear();
            _basketContext.Commit();
        }
    }
}
