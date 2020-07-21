using ETicaret.DataAccess.Base;
using ETicaret.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETicaret.Repositories.Implement.EfCore
{
    //EfCoreBaseRepository classını EfCoreOrderRepository clasına kalıtıyoruz ve gerekli paremetreleri veriyoruz.
    //Bunun yanında IOrderRepository interfacesinide aynı şekilde kalıtıyoruz. 
    //Bu şekilde hem EfCoreBaseRepository classındaki işlemlere hem de IOrderRepository içindeki metodlara ulaşıyoruz.
    //Daha sonra ihtiyacımız olan metodları dolduruyoruz.
    public class EfCoreOrderRepository : EfCoreBaseRepository<Order, ETicaretContext>, IOrderRepository
    {
        //Siparişleri getiren fonksiyon. 
        //Bunu linq sorgusu ile gerçekleştiriyoruz.
        //Contextimizin içindeki Orders'dan OrderItems'e ulaşıoyruz. Daha sonra buradan Product'a ulaşıp AsQueryable() fonksiyonu ile değerleri seçiyoruz veritabanından. 
        //Daha sonra kullanıcının ıdsine ait sipariş varsa bunları liste olarak geri gönderiyoruz.
        public List<Order> GetOrders(string userId)
        {
            using (var context = new ETicaretContext())
            {
                var orders = context.Orders
                                .Include(i => i.OrderItems)
                                .ThenInclude(i => i.Product)
                                .AsQueryable();

                if (!string.IsNullOrEmpty(userId))
                {
                    orders = orders.Where(i => i.UserId == userId);
                }

                return orders.ToList();
            }
        }
    }
}
