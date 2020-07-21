using ETicaret.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.WebUI.Models
{
    //Uygulamada kullanılan verileri temsil eder ve verilerin işlenme mantığının saklandığı kısımdır. Verilerin validasyonu burada yapıyoruz.
    //Ürün eklemek için gerekli model
    public class ProductModel
    {
        public int Id { get; set; }

        //İnputun boş olmaması gerektiğini belirttiğimiz attribute
        [Required(ErrorMessage = "Ürün ismi giriniz !")]
        //İnputun boyutunu belirttiğimiz attribute
        [StringLength (60, MinimumLength =5,ErrorMessage ="Ürünün ismi minimum 5 karekter, maksimum 60 karekter olmalıdır !")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Ürün resmi giriniz !")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Ürün açıklaması giriniz !")]
        [StringLength(2500, MinimumLength = 10, ErrorMessage = "Ürünün açıklaması minimum 10 karekter, maksimum 2500 karekter olmalıdır !")]
        public string Description { get; set; }

        [Required (ErrorMessage ="Fiyat belirtiniz !")]
        //Sayısının büyüklüğünü belirttiğimiz attribute
        [Range(1,250000)]
        public int? Price { get; set; }

        public List<Category> SelectedCategories { get; set; }

    }
}
