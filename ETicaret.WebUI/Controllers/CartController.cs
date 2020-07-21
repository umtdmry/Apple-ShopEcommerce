using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETicaret.Business.Base;
using ETicaret.Business.Services;
using ETicaret.Entities;
using ETicaret.WebUI.Extensions;
using ETicaret.WebUI.Identity;
using ETicaret.WebUI.Models;
using IyzipayCore;
using IyzipayCore.Model;
using IyzipayCore.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebUI.Controllers
{

    //Buraya bu attribute ile Loginsiz giriş izni vermedimizi belirtiyoruz.
    [Authorize]
    //Controller Katmanı MVC yapısının client ile bağlı olan kısmıdır.
    //Gelen isteği karşılar. İsteğe göre gerekli ActionResult çalıştırılır.
    //Buradan direkt cliente geri cevap dönülebilir (sayfa açılabilir), gerekiyorsa service veri göndürülüp gerekli işlemlerin yapılması sağlanabilir.
    public class CartController : Controller
    {
        private ICartService _cartService;
        private IOrderService _orderService;
        private UserManager<ApplicationUser> _userManager;

        //Burada bir UserManager, IOrderService, ICartService nesnelerini kullanılabilmek için constructor injection yapıyoruz.
        public CartController(IOrderService orderService, ICartService cartService, UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _cartService = cartService;
            _userManager = userManager;
        }

        //Sepete yönlendirme yapıyoruz.
        public IActionResult Index()
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));
            return View(new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId = i.Id,
                    ProductId = i.Product.Id,
                    Name = i.Product.Name,
                    Price = (decimal)i.Product.Price,
                    ImageUrl = i.Product.ImageUrl,
                    Quantity = i.Quantity
                }).ToList()
            });
        }

        //Sepete ürün ekleme işlemini gerçekleştiriyoruz.
        //İşlem tamamlanınca Index() sayfasına yönlendiriyoruz
        //HttpPost attributesinin verilmesinin sebebi bir post işlemi yapılıyor. Sepete ürün ekleniyor olduğu için.
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            _cartService.AddToCart(_userManager.GetUserId(User), productId, quantity);
            return RedirectToAction("Index");
        }

        //Sepeten ürün silme işlemini gerçekleştiriyoruz.
        //İşlem tamamlanınca Index() sayfasına yönlendiriyoruz
        //HttpPost attributesinin verilmesinin sebebi bir post işlemi yapılıyor. Sepeten ürün silindiği için.
        [HttpPost]
        public IActionResult DeleteFromCart(int productId)
        {
            _cartService.DeleteFromCart(_userManager.GetUserId(User), productId);
            return RedirectToAction("Index");
        }

        //Ödeme sayfasına yönlendiriyoruz.
        //Bu işlemi yaparken sepetteki ürün bilgilerini order modele taşıyoruz.
        public IActionResult Checkout()
        {

            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));

            var orderModel = new OrderModel();

            orderModel.CartModel = new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId = i.Id,
                    ProductId = i.Product.Id,
                    Name = i.Product.Name,
                    Price = (decimal)i.Product.Price,
                    ImageUrl = i.Product.ImageUrl,
                    Quantity = i.Quantity
                }).ToList()
            };

            return View(orderModel);
        }

        //Ödeme işlemini gerçekleştiriyoruz.
        //Ödeme başarılı olursa Siparişlerim sayfasına, Başarısız olursa Sepet sayfasına yönlendiriyoruz.
        [HttpPost]
        public IActionResult Checkout(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var cart = _cartService.GetCartByUserId(userId);

                model.CartModel = new CartModel()
                {
                    CartId = cart.Id,
                    CartItems = cart.CartItems.Select(i => new CartItemModel()
                    {
                        CartItemId = i.Id,
                        ProductId = i.Product.Id,
                        Name = i.Product.Name,
                        Price = (decimal)i.Product.Price,
                        ImageUrl = i.Product.ImageUrl,
                        Quantity = i.Quantity
                    }).ToList()
                };

                var payment = PaymentProcess(model);

                if (payment.Status == "success")
                {
                    SaveOrder(model, payment, userId);
                    ClearCart(cart.Id.ToString());
                    TempData.Put("message", new ResultMessage()
                    {
                        Title = "Sipariş İşlemi Başarıyla Tamamlandı !",
                        Message = "Siparişiniz İşleme Alınmıştır !",
                        Css = "success"
                    });
                    return RedirectToAction("GetOrders", "Cart");
                }
                else
                {
                    TempData.Put("message", new ResultMessage()
                    {
                        Title = "Sipariş İşlemi Tamamlanamadı !",
                        Message = "Kart Bilgileriniz Yanlış Girilmiştir !",
                        Css = "warning"
                    });
                    return RedirectToAction("Index", "Cart");
                }
            }

            return View(model);
        }

        //Siparişi kaydediyoruz.
        private void SaveOrder(OrderModel model, Payment payment, string userId)
        {
            var order = new Order();

            order.OrderNumber = new Random().Next(111111, 999999).ToString();
            order.OrderState = EnumOrderState.Completed;
            order.PaymentTypes = EnumPaymentTypes.CreditCart;
            order.PaymentId = payment.PaymentId;
            order.ConversationId = payment.ConversationId;
            order.OrderDate = new DateTime();
            order.FirstName = model.FirstName;
            order.LastName = model.LastName;
            order.Email = model.Email;
            order.Phone = model.Phone;
            order.Address = model.Address;
            order.UserId = userId;

            foreach (var item in model.CartModel.CartItems)
            {
                var orderitem = new OrderItem()
                {
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId
                };
                order.OrderItems.Add(orderitem);
            }
            _orderService.Create(order);
        }

        //Sepeti temizliyoruz.
        private void ClearCart(string cartId)
        {
            _cartService.ClearCart(cartId);
        }

        //Ödeme için gerekli doldurulması gerekli veriler.
        //iyzico dökümantasyonunda bunları veriyor.
        //Biz kendi kullandıklarımızı burda modelden alıp güncelliyoruz.
        //Geri kalanları dökümantasyondaki şekliyle kullanıyoruz.
        //İşlem gerçekleştiğinde ödeme sağlanmış oluyor
        private Payment PaymentProcess(OrderModel model)
        {
            Options options = new Options();
            //Bize atanan ApiKey
            options.ApiKey = "sandbox-SpU5qI2tI7bcAZq5IT63q4Ve02TKFhX9";
            //Bize atanan SecretKey
            options.SecretKey = "sandbox-TYmE8hAPJbbe2tWcuVfrAZDopQOI4Gva";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = Guid.NewGuid().ToString();
            request.Price = model.CartModel.TotalPrice().ToString().Split(",")[0]; ;
            request.PaidPrice = model.CartModel.TotalPrice().ToString().Split(",")[0]; ;
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = model.CartModel.CartId.ToString();
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = model.CardName;
            paymentCard.CardNumber = model.CardNumber;
            paymentCard.ExpireMonth = model.ExpirationMonth;
            paymentCard.ExpireYear = model.ExpirationYear;
            paymentCard.Cvc = model.Cvv;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            //******************************************************
            //Test için iyziconun bize verdiği kredi kartı bilgileri
            //paymentCard.CardHolderName = "John Doe";
            //paymentCard.CardNumber = "5528790000000008";
            //paymentCard.ExpireMonth = "12";
            //paymentCard.ExpireYear = "2030";
            //paymentCard.Cvc = "123";
            //******************************************************

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = "John";
            buyer.Surname = "Doe";
            buyer.GsmNumber = "+905350000000";
            buyer.Email = "email@email.com";
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem basketItem;

            foreach (var item in model.CartModel.CartItems)
            {
                basketItem = new BasketItem();
                basketItem.Id = item.ProductId.ToString();
                basketItem.Name = item.Name;
                basketItem.Category1 = "Phone";
                basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                basketItem.Price = item.Price.ToString().Split(",")[0];

                basketItems.Add(basketItem);
            }

            request.BasketItems = basketItems;

            return Payment.Create(request, options);

        }

        //Siparişlerim sayfasın yönlendiriyoruz.
        public IActionResult GetOrders()
        {
            var orders = _orderService.GetOrders(_userManager.GetUserId(User));
            var orderListModel = new List<OrderListModel>();
            OrderListModel orderModel;

            foreach (var order in orders)
            {
                orderModel = new OrderListModel();
                orderModel.OrderId = order.Id;
                orderModel.OrderNumber = order.OrderNumber;
                orderModel.OrderDate = order.OrderDate;
                orderModel.OrderNote = order.OrderNote;
                orderModel.Phone = order.Phone;
                orderModel.FirstName = order.FirstName;
                orderModel.LastName = order.LastName;
                orderModel.Email = order.Email;
                orderModel.Address = order.Address;
                orderModel.City = order.City;

                orderModel.OrderItems = order.OrderItems.Select(i => new OrderItemModel()
                {
                    OrderItemId = i.Id,
                    Name = i.Product.Name,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    ImageUrl = i.Product.ImageUrl
                }).ToList();

                orderListModel.Add(orderModel);

            }

            return View(orderListModel);
        }

    }
}