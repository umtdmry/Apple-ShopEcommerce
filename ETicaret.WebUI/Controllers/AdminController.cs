using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ETicaret.Business.Base;
using ETicaret.Entities;
using ETicaret.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebUI.Controllers
{
    //Login olan kullanıcının rolü admin olması şartıyla bu kısıma giriş izni veriyoruz.
    [Authorize(Roles ="admin")]
    //Controller Katmanı MVC yapısının client ile bağlı olan kısmıdır.
    //Gelen isteği karşılar. İsteğe göre gerekli ActionResult çalıştırılır.
    //Buradan direkt cliente geri cevap dönülebilir (sayfa açılabilir), gerekiyorsa service veri göndürülüp gerekli işlemlerin yapılması sağlanabilir.
    public class AdminController : Controller
    {

        private IProductService _productService;
        private ICategoryService _categoryService;

        //Burada bir IProductService ve ICategoryService nesnesi kullanılabilmek için constructor injection yapıyoruz.
        public AdminController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;

        }

        //Ürün işlemleri sayfasına yönlendiriyoruz
        public IActionResult ProductList()
        {
            return View(new ProductListModel
            {
                Products = _productService.GetAll()
            });
        }

        //Ürün ekle sayfasına yönlendiriyoruz
        public IActionResult CreateProduct()
        {
            return View(new ProductModel());
        }

        [HttpPost]
        //Ürünü ekleme işlemini gerçekleştiriyoruz.
        //Kullanıcıyı işlem başarılıysa Ürün İşlemleri sayfasına yönlendiriyoruz.
        public IActionResult CreateProduct(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new Product()
                {
                    Name = model.Name,
                    Price = (decimal)model.Price,
                    Description = model.Description,
                    ImageUrl = model.ImageUrl
                };

                _productService.Create(entity);

                return RedirectToAction("ProductList");
            }
            return View(model);
        }

        //Ürün güncelle sayfasına yönlendiriyoruz
        public IActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _productService.GetByIdWithCategories((int)id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                Id = entity.Id,
                Price = (int)entity.Price,
                Name = entity.Name,
                Description = entity.Description,
                ImageUrl = entity.ImageUrl,
                SelectedCategories=entity.ProductCategories.Select(i=>i.Category).ToList()

            };

            ViewBag.Categories = _categoryService.GetAll();

            return View(model);
        }

        //HttpPost attributesinin verilmesinin sebebi bir post işlemi yapılıyor.
        [HttpPost]
        //Ürünü güncelleme işlemini gerçekleştiriyoruz.
        //Kullanıcıyı işlem başarılıysa Ürün İşlemleri sayfasına yönlendiriyoruz.
        public async Task<IActionResult> EditProduct(ProductModel model, int[] categoryIds, IFormFile file)
        {
            if(ModelState.IsValid)
            {
                var entity = _productService.GetById(model.Id);

                if (entity == null)
                {
                    return NotFound();
                }
                entity.Name = model.Name;
                entity.Price = (decimal)model.Price;
                entity.Description = model.Description;

                if (file != null)
                {
                    entity.ImageUrl = file.FileName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                _productService.Update(entity, categoryIds);

                return RedirectToAction("ProductList");
            }
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
            
        }

        //HttpPost attributesinin verilmesinin sebebi bir post işlemi yapılıyor.
        [HttpPost]
        //Ürünü silme işlemini gerçekleştiriyoruz.
        public IActionResult DeleteProduct(int productId)
        {
            var entity = _productService.GetById(productId);

            if (entity != null)
            {
                _productService.Delete(entity);
            }
            return RedirectToAction("ProductList");
        }

        //Kategori işlemleri sayfasına yönlendiriyoruz
        public IActionResult CategoryList()
        {
            return View(new CategoryListModel
            {
                Categories = _categoryService.GetAll()
            });
        }

        //Kategori güncelle sayfasına yönlendiriyoruz
        public IActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _categoryService.GetByIdWithProducts((int)id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new CategoryModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Products = entity.ProductCategories.Select(p => p.Product).ToList()

            };
            return View(model);
        }

        //HttpPost attributesinin verilmesinin sebebi bir post işlemi yapılıyor.
        [HttpPost]
        //Kategori güncelleme işlemini gerçekleştiriyoruz.
        //Kullanıcıyı işlem başarılıysa Kategori İşlemleri sayfasına yönlendiriyoruz.
        public IActionResult EditCategory(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _categoryService.GetById(model.Id);

                if (entity == null)
                {
                    return NotFound();
                }

                entity.Name = model.Name;
                _categoryService.Update(entity);

                return RedirectToAction("CategoryList");
            }
            return View(model);
        }

        //Kategori ekle sayfasına yönlendiriyoruz
        public IActionResult CreateCategory()
        {
            return View(new CategoryModel());
        }

        //HttpPost attributesinin verilmesinin sebebi bir post işlemi yapılıyor.
        [HttpPost]
        //Kategori ekleme işlemini gerçekleştiriyoruz.
        //Kullanıcıyı işlem başarılıysa Kategori İşlemleri sayfasına yönlendiriyoruz.
        public IActionResult CreateCategory(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new Category()
                {
                    Name = model.Name,
                };

                _categoryService.Create(entity);

                return RedirectToAction("CategoryList");
            }
            return View(model);
        }

        //HttpPost attributesinin verilmesinin sebebi bir post işlemi yapılıyor.
        [HttpPost]
        //Kategoriyi silme işlemini gerçekleştiriyoruz.
        //Kullanıcıyı işlem başarılıysa Kategori İşlemleri sayfasına yönlendiriyoruz.
        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = _categoryService.GetById(categoryId);

            if (entity != null)
            {
                _categoryService.Delete(entity);
            }
            return RedirectToAction("CategoryList");
        }

        //HttpPost attributesinin verilmesinin sebebi bir post işlemi yapılıyor.
        [HttpPost]
        //Kategoriden ürün silme işlemi gerçekleştiriyoruz.
        //İşlem başarılıysa Kategori Güncelle sayfasına geri yönlendiriyoruz.
        public IActionResult DeleteFromCategory(int categoryId, int productId)
        {
            _categoryService.DeleteFromCategory(categoryId, productId);
            return Redirect("/admin/editcategory/" + categoryId);
        }

    }
}