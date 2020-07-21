using ETicaret.Business.Base;
using ETicaret.DataAccess.Base;
using ETicaret.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaret.Business.Manager
{
    //Sepet İşlemleri Service
    //IOrderService'i implement edip ordaki metodları kullanmaktadır.
    public class OrderManager : IOrderService
    {
        private IOrderRepository _orderRepository;

        //Öncelikle Constructor inject yapıyoruz.
        //Bunun nedeni veritabanı işlemleri ile service işlemlerimizi ayırdık. Ayırmamızın nedeni ise SOLID kurallarından olan Dependency Injection ile olabildiğince geliştirmeye açık programlama yapabilmek. 
        //Buradan veritabanına bir işlem göndermek için repositorydeki metoda ulaşmalıyız. Bunun içinde gerekli repositoryden nesne üretip bu nesneyi inject ediyoruz.
        public OrderManager(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        //**********************************************************
        //Aşağıdaki metodlarda gerekli logic işlemleri yapıyoruz.
        //Eğer bir logiç işlem deilde veri tabanı işlemi gerekiyorsa
        //repositorydeki gerekli metodumuzu çağırıyoruz.
        //**********************************************************

        //Sipariş oluştur
        public void Create(Order entity)
        {
            _orderRepository.Create(entity);
        }

        //Bütün siparişleri getir
        public List<Order> GetOrders(string userId)
        {
            return _orderRepository.GetOrders(userId);
        }
    }
}
