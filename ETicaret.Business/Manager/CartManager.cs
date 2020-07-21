using ETicaret.Business.Services;
using ETicaret.Entities;
using ETicaret.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaret.Business.Manager
{
    //Sepet İşlemleri Service
    //ICartService'i implement edip ordaki metodları kullanmaktadır.
    public class CartManager : ICartService
    {

        private ICartRepository _cartRepository;

        //Öncelikle Constructor inject yapıyoruz.
        //Bunun nedeni veritabanı işlemleri ile service işlemlerimizi ayırdık. Ayırmamızın nedeni ise SOLID kurallarından olan Dependency Injection ile olabildiğince geliştirmeye açık programlama yapabilmek. 
        //Buradan veritabanına bir işlem göndermek için repositorydeki metoda ulaşmalıyız. Bunun içinde gerekli repositoryden nesne üretip bu nesneyi inject ediyoruz.
        public CartManager(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        //**********************************************************
        //Aşağıdaki metodlarda gerekli logic işlemleri yapıyoruz.
        //Eğer bir logiç işlem deilde veri tabanı işlemi gerekiyorsa
        //repositorydeki gerekli metodumuzu çağırıyoruz.
        //**********************************************************

        //Kullanıcının sepetini getir
        public Cart GetCartByUserId(string userId)
        {
            return _cartRepository.GetByUserId(userId);
        }

        //Kullanıcıya sepet üret
        public void InitializeCart(string userId)
        {
            _cartRepository.Create(new Cart() { UserId=userId });
        }

        //Sepete ürünü ekle
        public void AddToCart(string userId, int productId, int quantity)
        {
            var cart = GetCartByUserId(userId);
            if (cart != null)
            {
                var index = cart.CartItems.FindIndex(i => i.ProductId == productId);

                if (index < 0)
                {
                    cart.CartItems.Add(new CartItem()
                    {
                        ProductId = productId,
                        Quantity = quantity,
                        CartId = cart.Id
                    });
                }
                else
                {
                    cart.CartItems[index].Quantity += quantity;
                }

                _cartRepository.Update(cart);
            }
        }

        //Sepeten ürünü sil
        public void DeleteFromCart(string userId, int productId)
        {
            var cart = GetCartByUserId(userId);
            if (cart != null)
            {
                _cartRepository.DeleteFromCart(cart.Id, productId);
            }
        }

        //Sepeti temizle
        public void ClearCart(string cartId)
        {
            _cartRepository.ClearCart(cartId);
        }
    }
}
