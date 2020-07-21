using ETicaret.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.WebUI.Models
{
    //Uygulamada kullanılan verileri temsil eder ve verilerin işlenme mantığının saklandığı kısımdır. Verilerin validasyonu burada yapıyoruz.
    //Ürün Detayları için gerekli model
    public class ProductDetailsModel
    {
        public Product Product { get; set; }

        public List<Category> Categories { get; set; }


    }
}
