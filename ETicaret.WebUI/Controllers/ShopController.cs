using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETicaret.Business.Base;
using ETicaret.Entities;
using ETicaret.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebUI.Controllers
{
    //Controller Katmanı MVC yapısının client ile bağlı olan kısmıdır.
    //Gelen isteği karşılar. İsteğe göre gerekli ActionResult çalıştırılır.
    //Buradan direkt cliente geri cevap dönülebilir (sayfa açılabilir), gerekiyorsa service veri göndürülüp gerekli işlemlerin yapılması sağlanabilir.
    public class ShopController : Controller
    {
        private IProductService _productService;

        //Burada bir IProductService nesnesi kullanılabilmek için constructor injection yapıyoruz.
        public ShopController(IProductService productService)
        {
            _productService = productService;

        }

        //Details sayfasına yönlendiriliyor kullanıcı
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = _productService.GetProductDetails((int)id);

            if (product == null)
            {
                return NotFound();
            }

            return View(new ProductDetailsModel()
            {
                Product = product,
                Categories = product.ProductCategories.Select(i => i.Category).ToList()
            }) ;
        }

        //List sayfasına yönlendiriliyor kullanıcı
        public IActionResult List(string category, int page=1)
        {
            const int pageSize = 9;
            return View(new ProductListModel()
            {
                Products = _productService.GetProductsByCategory(category, page, pageSize),
                PageInfo = new PageInfo() {
                    TotalItems = _productService.GetCountByCategory(category),
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    CurrentCategory = category
                },
            }) ;
        }
    }
}
