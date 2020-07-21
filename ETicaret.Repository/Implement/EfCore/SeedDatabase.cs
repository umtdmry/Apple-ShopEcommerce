using ETicaret.Entities;
using ETicaret.Repositories.Implement.EfCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETicaret.Repository.Implement.EfCore
{
    public static class SeedDatabase
    {
        public static void Seed()
        {
            var context = new ETicaretContext();

            //Bekleyen migration yoksa test belleğini databaseye yükle
            if(context.Database.GetPendingMigrations().Count()==0)
            {
                //Database boşsa eklesin
                if(context.Categories.Count()==0)
                {
                    context.Categories.AddRange(Categories);
                }

                if (context.Products.Count() == 0)
                {
                    context.Products.AddRange(Products);
                    context.AddRange(ProductCategory);
                }

                context.SaveChanges();
            }
        }

        //Admin işlemlerini yazmadan önce kullanıyorduk !!!
        //*************************************************

        //Category Test Datalarımız
        private static Category[] Categories =
        {
            //new Category() {Name="Telefon"},
            //new Category() {Name="Bilgisayar"},
            //new Category() {Name="Elektronik"}


        };

        //Ürün Test Datalarımız
        private static Product[] Products =
        {
            //new Product(){ Name = "Samsung S6",ImageUrl = "1.jpg",Price = 1000, Description ="<p>güzel telefon</p>"},        
            //new Product(){ Name = "Samsung S7",ImageUrl = "2.jpg",Price = 2000, Description ="<p>güzel telefon</p>"},        
            //new Product(){ Name = "Samsung S8",ImageUrl = "3.jpg",Price = 3000, Description ="<p>güzel telefon</p>"},        
            //new Product(){ Name = "Samsung S9",ImageUrl = "4.jpg",Price = 4000, Description ="<p>güzel telefon</p>"},        
            //new Product(){ Name = "Samsung S5",ImageUrl = "5.jpg",Price = 5000, Description ="<p>güzel telefon</p>"},        
            //new Product(){ Name = "IPhone 6",  ImageUrl = "6.jpg",Price = 6000, Description ="<p>güzel telefon</p>"},
            //new Product(){ Name = "IPhone 7",  ImageUrl = "7.jpg",Price = 7000, Description ="<p>güzel telefon</p>"},
            //new Product(){ Name = "IPhone 8",  ImageUrl = "8.jpg",Price = 8000, Description ="<p>güzel telefon</p>"}
        };

        //Kategoriye Ürün Eklemek Test
        private static ProductCategory[] ProductCategory =
        {
            //new ProductCategory() { Product=Products[0],Category=Categories[0]},
            //new ProductCategory() { Product=Products[0],Category=Categories[2]},
            //new ProductCategory() { Product=Products[1],Category=Categories[0]},
            //new ProductCategory() { Product=Products[1],Category=Categories[1]},
            //new ProductCategory() { Product=Products[2],Category=Categories[0]},
            //new ProductCategory() { Product=Products[2],Category=Categories[2]},
            //new ProductCategory() { Product=Products[3],Category=Categories[1]},
            //new ProductCategory() { Product=Products[3],Category=Categories[2]},

        };
    }
}
