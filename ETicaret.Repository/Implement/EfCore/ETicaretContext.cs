using ETicaret.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaret.Repositories.Implement.EfCore
{
    //Projemizin Context sınıfıdır.
    //DbContext sınıfını implement eder.
    //Veri tabanı bağlantısı , veri tabanı işlemleri, değişlik yönetimi, caching, transaction yönetimi gibi görevleri vardır.
    public class ETicaretContext : DbContext
    {
        //Yapılandırma fonksiyonumuzdur.
        //Sql bağlantımız için verdiğimiz connection string bu fonksiyonda verilir.
        //Eğer veritabanı bağlatısı ile ilgili sıkıntımız varsa muhtemelen burası yanlış ayarlanmıştır.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=eticaret;integrated security=true;");
        }

        //ProductCategory Entityimizdeki ProductId'yi ve CategoryId'yi Priamry Key yapmamıza yarıyan fonksiyondur.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>().HasKey(c => new { c.CategoryId, c.ProductId });
        }

        //Entitylerimizi Contextte oluşturuyoruz.
        //Context işlemlerini bunlar üzerinden yapıyoruz.
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }



    }
}
