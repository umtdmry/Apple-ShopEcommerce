using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaret.Entities
{
    //Ürün ve Kategori arasındaki bağlantıyı sağlamak amaçlı oluşturduğumuz entity sınıfıdır.
    //Yaptığımız bu entity sınıfı ile bir ürün birden fazla kategori ile 1 e 1 bağlanır.
    //Bu şekilde bir ürün aynı kategoriye iki defa atanamamış oluyor.
    //Bu işlem için ProductId ve CategoryId PK olarak belirlenmelidir.
    //Bunu sağlamak için Repository katmanına context kısmında bunu OnModelCreating fonksiyonunda HasKey metodunun içinde bunu belirtmeliyiz.
    public class ProductCategory
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
