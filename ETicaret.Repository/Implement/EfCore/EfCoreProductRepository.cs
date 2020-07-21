using ETicaret.DataAccess.Base;
using ETicaret.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETicaret.Repositories.Implement.EfCore
{
    //EfCoreBaseRepository classını EfCoreProductRepository clasına kalıtıyoruz ve gerekli paremetreleri veriyoruz.
    //Bunun yanında IProductRepository interfacesinide aynı şekilde kalıtıyoruz. 
    //Bu şekilde hem EfCoreBaseRepository classındaki işlemlere hem de IProductRepository içindeki metodlara ulaşıyoruz.
    //Daha sonra ihtiyacımız olan metodları dolduruyoruz.
    public class EfCoreProductRepository : EfCoreBaseRepository<Product, ETicaretContext>, IProductRepository
    {
        //Ürünün kategorilerine ulaşmamızı sağlayan fonksiyon.
        //Linq sorgusu ile Products'ın içinden productId ye sahip ürünün sahip olan Category'e ProductCategories üzerinden erişiyoruz.
        public Product GetByIdWithCategories(int productId)
        {
            using (var context = new ETicaretContext())
            {
                return context.Products
                    .Where(i => i.Id == productId)
                    .Include(i => i.ProductCategories)
                    .ThenInclude(i => i.Category)
                    .FirstOrDefault();
            }
        }

        //Kategorideki ürünlerin sayısını bulan fonksiyon.
        //Pageable sayfa yapısı için gerekli ürün sayısını bulmamız gerektiği için yazılmış fonksiyondur.
        //Linq sorgusu ile gelen kategori ismini bütün ProductCategories içinde aratıp bir pruducts değişkenine atıyoruz.
        //Daha sonra bu değişkenin içindeki eleman sayısını geri çağırıldığı yere döndürüyoruz.
        public int GetCountByCategory(string category)
        {
             using (var context = new ETicaretContext())
             {
                 var products = context.Products.AsQueryable();
             
                 if (!string.IsNullOrEmpty(category))
                 {
                     products = products
                         .Include(i => i.ProductCategories)
                         .ThenInclude(i => i.Category)
                         .Where(i => i.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));
                 }
             
                 return products.Count();
             }
        }

        //Kategoriye göre ürünleri getiren fonksiyon.
        //Menü kısmında kategori seçtiğimizde ya da sayfa değiştirdiğimizde gelen category değeri ile sayfaya pagesize kadar ürünü döndüren fonksiyon. Burada kaçıncı sayfada oldumuzu tutan değişken ise pagetir.
        //Linq sorgusu ile gelen kategori ismini bütün ProductCategories içinde aratıp bir pruducts değişkenine atıyoruz.
        //Daha sonra bu değişkeni geri çağırıldığı yere döndürüyoruz.
        public List<Product> GetProductByCategory(string category, int page, int pageSize)
        {
            using(var context =new ETicaretContext())
            {
                var products = context.Products.AsQueryable();

                if (!string.IsNullOrEmpty(category))
                {
                    products = products
                                .Include(i => i.ProductCategories)
                                .ThenInclude(i => i.Category)
                                .Where(i => i.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));
                }

                return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        //Ürün detaylarını getiren fonksiyon.
        //Linq sorgusu ile  gelen ürün id ile detaylara ulaşıyoruz.
        public Product getProductDetails(int id)
        {
            using (var context = new ETicaretContext())
            {
                return context.Products.Where(i => i.Id == id).Include(i => i.ProductCategories).ThenInclude(i => i.Category).FirstOrDefault();
            }
        }

        //Ürünü güncelleyen fonksiyonumuz.
        //Ürünün önceki değerlerini yeni gelen değerler ile değiştiriyoruz.
        //Daha sonra SaveChanges() metodu ile veri tabanımızda güncel hali ile güncelliyoruz.
        public void Update(Product entity, int[] categoryIds)
        {
            using (var context = new ETicaretContext())
            {
                var product = context.Products
                    .Include(i => i.ProductCategories)
                    .FirstOrDefault(i => i.Id == entity.Id);

                if (product!=null)
                {
                    product.Name = entity.Name;
                    product.Description = entity.Description;
                    product.ImageUrl = entity.ImageUrl;
                    product.Price = entity.Price;

                    product.ProductCategories = categoryIds.Select(catid => new ProductCategory()
                    {
                        CategoryId = catid,
                        ProductId = entity.Id
                    }).ToList();

                    context.SaveChanges();
                }
            }
        }
    }
}
