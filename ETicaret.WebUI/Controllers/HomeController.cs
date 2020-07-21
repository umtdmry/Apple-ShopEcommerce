using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETicaret.Business.Base;
using ETicaret.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebUI.Controllers
{
    //Controller Katmanı MVC yapısının client ile bağlı olan kısmıdır.
    //Gelen isteği karşılar. İsteğe göre gerekli ActionResult çalıştırılır.
    //Buradan direkt cliente geri cevap dönülebilir (sayfa açılabilir), gerekiyorsa service veri göndürülüp gerekli işlemlerin yapılması sağlanabilir.
    //Genel olarak ilk Controller MVC de HomeController olarak adlandırılmaktadır. Bu bir MVC standartıdır. Bu projede bizde ilk giriş sayfasını bu şekilde yönlendirmesini yaptık.
    public class HomeController : Controller
    {
        private IProductService _productService;

        //Burada bir IProductService nesnesi kullanılabilmek için constructor injection yapıyoruz.
        public HomeController(IProductService productService)
        {
            _productService = productService;

        }

        //Ana Sayfaya yönlendiriyor
        public IActionResult Index()
        {
            return View(new ProductListModel(){
                Products = _productService.GetAll()
        });
        }
    }
}