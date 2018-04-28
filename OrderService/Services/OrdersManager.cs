using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrderService.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Flurl.Http;



namespace OrderService.Services
{
    public class OrdersManager : IRepository<Order>
    {
        private readonly ShoppingCartDbContext _dbContext;
        private readonly IOptions<StkSetting> _stkSettings;
        private readonly IOptions<ShoppingCartStkPushKey> _shoppingCartStkPushKey;      
        public OrdersManager(ShoppingCartDbContext dbContext, IOptions<StkSetting> stkSettings, IOptions<ShoppingCartStkPushKey> shoppingCartStkPushKey)
        {
            _dbContext = dbContext;
            _stkSettings = stkSettings;
            _shoppingCartStkPushKey = shoppingCartStkPushKey;
        }
        public bool Add(Order order)
        {
            try
            {
                this._dbContext.Orders.Add(order);
                this._dbContext.SaveChanges();
                return true;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public IEnumerable<Order> GetAll()
        {
            var orders = this._dbContext.Orders.ToList();
            return orders;
        }

        public Order GetById(Guid id)
        {
            var order = this._dbContext.Orders.Find(id);
            return order;
        }

        public bool Remove(Guid id)
        {
            try
            {
                var order = this._dbContext.Orders.Find(id);
                this._dbContext.Remove(order);
                this._dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal List<Product> getOrdersItems(Guid id)
        {
            var orderItemsByOrderId = (from orderitems in this._dbContext.OrderItems
                                       join products in this._dbContext.Products
                                       on orderitems.ProductId equals products.ProductId
                                       where orderitems.OrderId == id
                                       select new
                                       {
                                           ProductId = products.ProductId,
                                           ProductCategory = products.ProductCategory,
                                           ProductManufacturer = products.ProductManufacturer,
                                           Price = products.Price,
                                           ProductMediaFile = products.ProductMediaFile,
                                           ProductName = products.ProductName,
                                           ProductSku = products.ProductSku,
                                           ShopperReview = products.ShopperReview
                                       }).Select(product => new Product
                                       {
                                           Price = product.Price,
                                           ProductId = product.ProductId,
                                           ProductMediaFile = product.ProductMediaFile,
                                           ProductCategory = product.ProductCategory,
                                           ProductName = product.ProductName,
                                           ProductSku = product.ProductSku,
                                           ProductManufacturer = product.ProductManufacturer,
                                           ShopperReview = product.ShopperReview
                                       }).ToList();
            return orderItemsByOrderId;
        }

        public bool CreateNewOrder(CustomerOrder customerOrder, string customerId)
        {
            try
            {
                customerOrder.CustomerId = customerId;
                var order = new Order()
                {
                    CustomerId = customerId,
                    Email = customerOrder.Email,
                    NotifyShopper = customerOrder.NotifyShopper,
                    OrderDate = DateTime.Now,
                    OrderId = Guid.NewGuid(),
                    PaymentMethodId = customerOrder.PaymentMethodId,
                    ShipmentMethodId = customerOrder.ShipmentMethodId,
                    Status = "New Order",
                };

                var billingInfo = new BillingInfo()
                {
                    City = customerOrder.BillingInfo?.City,
                    CompanyName = customerOrder.BillingInfo?.CompanyName,
                    FirstName = customerOrder.BillingInfo?.FirstName,
                    LastName = customerOrder.BillingInfo?.LastName,
                    OrderId = order.OrderId,
                    PostalCode = customerOrder.BillingInfo?.PostalCode,
                    Address = customerOrder.BillingInfo?.Address,
                    PhoneNumber = customerOrder.BillingInfo?.PhoneNumber,
                };
                var orderItems = new List<OrderItem>();
                foreach (var orderitem in customerOrder.OrderItems)
                {
                    orderItems.Add(new OrderItem()
                    {
                        OrderId = order.OrderId,
                        Price = orderitem.Price,
                        ProductId = orderitem.ProductId,
                        Qty = orderitem.Qty,
                        Total = orderitem.Total,
                    });

                }
                this._dbContext.Orders.Add(order);
                this._dbContext.BillingInfos.Add(billingInfo);
                this._dbContext.OrderItems.AddRange(orderItems);
                this._dbContext.SaveChanges();
                OrderPlacedHandler.PublishOrderPlaced(customerOrder);
                SendStkPushNotifaction(order);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Order> getCustomerOrders(string userId)
        {
            var customersOrderList = (from orders in this._dbContext.Orders
                                      where orders.CustomerId == userId
                                      select new
                                      {
                                          OrderTotal = this._dbContext.OrderItems.Where(x => x.OrderId == orders.OrderId).Select(x => x.Total).Sum(),
                                          OrderDate = orders.OrderDate,
                                          OrderNo = orders.OrderNo,
                                          OrderId = orders.OrderId,
                                      }).ToList().Select(x => new Order
                                      {
                                          OrderNo = x.OrderNo,
                                          OrderDate = x.OrderDate,
                                          OrderId = x.OrderId,
                                          OrderTotal = x.OrderTotal,
                                      }).ToList();
            return customersOrderList;
        }
        private void SendStkPushNotifaction(Order order)
        {
            ShoppingCartApiAccessToken shoppingCartApiAccessToken = GetAuthToken();
            var result = "https://sandbox.safaricom.co.ke/mpesa/stkpush/v1/processrequest"
                .WithOAuthBearerToken(shoppingCartApiAccessToken.AccessToken)
                .PostJsonAsync(this._stkSettings.Value)
                .ReceiveString().Result;
        }


        private ShoppingCartApiAccessToken GetAuthToken()
        {

            var result = _shoppingCartStkPushKey.Value.Url
                .WithBasicAuth(_shoppingCartStkPushKey.Value.ConsumerKey, _shoppingCartStkPushKey.Value.ConsumerSecret)
                .GetStringAsync().Result;
            ShoppingCartApiAccessToken shoppingCartApiAccessToken = JsonConvert.DeserializeObject<ShoppingCartApiAccessToken>(result);
            Debug.Write(JsonConvert.SerializeObject(this._stkSettings.Value));
            return shoppingCartApiAccessToken;
        }

    }


}
