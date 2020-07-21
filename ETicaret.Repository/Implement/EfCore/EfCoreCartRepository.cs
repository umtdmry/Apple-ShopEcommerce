using ETicaret.Entities;
using ETicaret.Repositories.Implement.EfCore;
using ETicaret.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETicaret.Repository.Implement.EfCore
{
    //EfCoreBaseRepository classını EfCoreCartRepository clasına kalıtıyoruz ve gerekli paremetreleri veriyoruz.
    //Bunun yanında ICartRepository interfacesinide aynı şekilde kalıtıyoruz. 
    //Bu şekilde hem EfCoreBaseRepository classındaki işlemlere hem de ICartRepository içindeki metodlara ulaşıyoruz.
    //Daha sonra ihtiyacımız olan metodları dolduruyoruz.
    public class EfCoreCartRepository : EfCoreBaseRepository<Cart, ETicaretContext>, ICartRepository
    {
        //Sepetteki ürünleri güncelleyen metodumuz.
        public override void Update(Cart entity)
        {
            using (var context = new ETicaretContext())
            {
                context.Carts.Update(entity);
                context.SaveChanges();
            }
        }

        //Sepetteki ürünleri userId'ye göre getiren metodumuz.
        //Aşağıdaki işlem bir linq sorgusudur. Carts'dan Include metoduyla CartItems'e ulaşıyoruz. CartItems sayesinde Productlara ulaşabiliyoruz. Ordan da uygun userId'li kullanıcıyı metodumuza sonuş olarak geri döndürüyoruz.
        public Cart GetByUserId(string userId)
        {
            using (var context = new ETicaretContext())
            {
                return context
                            .Carts
                            .Include(i => i.CartItems)
                            .ThenInclude(i => i.Product)
                            .FirstOrDefault(i => i.UserId == userId);
            }
        }

        //Sepetten ürün silmeye yarıyan fonksiyon.
        //Aşağıdaki işlem bir sql sorgusudur. CartItem tablosundan CartId ve ProductId parametrelerine uyan ürünü siliyoruz.
        public void DeleteFromCart(int cartId, int productId)
        {
            using (var context = new ETicaretContext())
            {
                var cmd = @"delete from CartItem where CartId=@p0 And ProductId=@p1";
                context.Database.ExecuteSqlCommand(cmd, cartId, productId);
            }
        }

        //Sepetti tamamen temizleyen fonksiyon.
        //Gönderilen cartId'yi veri tabanından sql sorgusuyla silme işlemi gerçekleştirir.
        public void ClearCart(string cartId)
        {
            using (var context = new ETicaretContext())
            {
                var cmd = @"delete from CartItem where CartId=@p0";
                context.Database.ExecuteSqlCommand(cmd, cartId);
            }
        }
    }
}
