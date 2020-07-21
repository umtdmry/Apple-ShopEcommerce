using ETicaret.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.WebUI.Models
{
    //Uygulamada kullanılan verileri temsil eder ve verilerin işlenme mantığının saklandığı kısımdır. Verilerin validasyonu burada yapıyoruz.
    //Pageable yapı için gerekli bilgileri tuttuğumuz model
    public class PageInfo
    {
        //toplam ürün sayısı
        public int TotalItems { get; set; }
        //her sayfada kaç ürün olucak
        public int ItemPerPage { get; set; }
        //şuan kaçıncı sayfadayız
        public int CurrentPage { get; set; }
        //şuan hangi kategorideyiz
        public string CurrentCategory { get; set; }

        public int TotalPages()
        {
            return (int)Math.Ceiling((decimal)TotalItems / ItemPerPage);
        }
    }

    public class ProductListModel
    {
        public PageInfo PageInfo { get; set; }
        public List<Product> Products { get; set; }
    }
}
