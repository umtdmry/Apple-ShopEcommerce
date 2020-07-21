using ETicaret.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.WebUI.Models
{
    //Uygulamada kullanılan verileri temsil eder ve verilerin işlenme mantığının saklandığı kısımdır. Verilerin validasyonu burada yapıyoruz.
    //Kategori eklememiz için gerekli model
    public class CategoryModel
    {
        public int Id { get; set; }

        //İnputun boş olmaması gerektiğini belirttiğimiz attribute
        [Required(ErrorMessage = "Kategori ismi giriniz !")]
        //İnputun boyutunu belirttiğimiz attribute
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Kategorinin ismi minimum 2 karekter, maksimum 50 karekter olmalıdır !")]
        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}
