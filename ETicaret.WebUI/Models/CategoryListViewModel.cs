using ETicaret.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.WebUI.Models
{
    //Uygulamada kullanılan verileri temsil eder ve verilerin işlenme mantığının saklandığı kısımdır. Verilerin validasyonu burada yapıyoruz.
    //Seçilen kategorinin saklanması için kullandığımız model
    public class CategoryListViewModel
    {
        public List<Category> Categories { get; set; }

        public string SelectedCategory { get; set; }
    }
}
