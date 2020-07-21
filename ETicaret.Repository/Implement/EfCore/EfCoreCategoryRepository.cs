using ETicaret.DataAccess.Base;
using ETicaret.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETicaret.Repositories.Implement.EfCore
{
    //EfCoreBaseRepository classını EfCoreCategoryRepository clasına kalıtıyoruz ve gerekli paremetreleri veriyoruz.
    //Bunun yanında ICategoryRepository interfacesinide aynı şekilde kalıtıyoruz. 
    //Bu şekilde hem EfCoreBaseRepository classındaki işlemlere hem de ICategoryRepository içindeki metodlara ulaşıyoruz.
    //Daha sonra ihtiyacımız olan metodları dolduruyoruz.
    public class EfCoreCategoryRepository : EfCoreBaseRepository<Category, ETicaretContext>, ICategoryRepository
    {
        //Kategoriden ürün silmek için gerekli sql sorgusunu çalıştırıyoruz.
        //Bunun için categoryId ve productId yi paremetre olarak gönderiyoruz.
        public void DeleteFromCategory(int categoryId, int productId)
        {
            using (var context = new ETicaretContext())
            {
                var cmd = @"delete from ProductCategory where ProductId=@p0 And CategoryId=@p1";
                context.Database.ExecuteSqlCommand(cmd, productId, categoryId);
            }
        }

        //Kategoriye ait ürünleri getiren fonksiyon.
        //categoryId ile Categories'e ulaşıyoruz. Oradan ProductCategories'e ulaşıp içindeki ürünleri categoryId'ye göre geri döndürüyoruz.
        public Category GetByIdWithProducts(int categoryId)
        {
            using (var context = new ETicaretContext())
            {
                return context.Categories
                    .Where(i => i.Id == categoryId)
                    .Include(i => i.ProductCategories)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefault();
            }
        }
    }
}
