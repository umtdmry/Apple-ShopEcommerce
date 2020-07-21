using ETicaret.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaret.DataAccess.Base
{
    //Repository Katmanı bizim veri tabanı işlemlerimizi gerçekleştirdiğimiz katmandır.
    //IOrderRepository'e Base repositoryi kalıttık ve içinde o metodları kullanmamızıda sağladık.
    //Parametre olarak Order Entitysini gönderdik.
    //Ekstra gerekli metodlarımızı buraya yazdık.
    public interface IOrderRepository : IBaseRepository<Order>
    {
        List<Order> GetOrders(string userId);
    }
}
