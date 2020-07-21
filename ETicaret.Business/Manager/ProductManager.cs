using ETicaret.Business.Base;
using ETicaret.DataAccess.Base;
using ETicaret.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaret.Business.Manager
{
    //Sepet İşlemleri Service
    //IProductService'i implement edip ordaki metodları kullanmaktadır.
    public class ProductManager:IProductService
    {
        private IProductRepository _productRepository;

        //Öncelikle Constructor inject yapıyoruz.
        //Bunun nedeni veritabanı işlemleri ile service işlemlerimizi ayırdık. Ayırmamızın nedeni ise SOLID kurallarından olan Dependency Injection ile olabildiğince geliştirmeye açık programlama yapabilmek. 
        //Buradan veritabanına bir işlem göndermek için repositorydeki metoda ulaşmalıyız. Bunun içinde gerekli repositoryden nesne üretip bu nesneyi inject ediyoruz.
        public ProductManager(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        //**********************************************************
        //Aşağıdaki metodlarda gerekli logic işlemleri yapıyoruz.
        //Eğer bir logiç işlem deilde veri tabanı işlemi gerekiyorsa
        //repositorydeki gerekli metodumuzu çağırıyoruz.
        //**********************************************************

        //Id'si verilen ürünü getir
        public Product GetById(int id)
        {
            return _productRepository.GetById(id);
        }

        //Bütün ürünleri getir
        public List<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        //Ürün oluştur
        public void Create(Product entity)
        {
            _productRepository.Create(entity);
        }

        //Ürün sil
        public void Delete(Product entity)
        {
            _productRepository.Delete(entity);
        }

        //Ürünün detaylarını getir
        public Product GetProductDetails(int id)
        {
            return _productRepository.getProductDetails(id);
        }

        //Kategoriye ait ürünleri getir
        public List<Product> GetProductsByCategory(string category, int page,int pageSize)
        {
            return _productRepository.GetProductByCategory(category, page, pageSize);
        }

        //Kategorideki ürünlerin sayısını getir
        public int GetCountByCategory(string category)
        {
            return _productRepository.GetCountByCategory(category);
        }

        //Id'si verilen ürünün kategorilerini getir
        public Product GetByIdWithCategories(int id)
        {
            return _productRepository.GetByIdWithCategories(id);
        }

        //Ürünü güncelle kategorisiz
        public void Update(Product entity)
        {
            _productRepository.Update(entity);

        }

        //Ürün güncelle kategorili
        public void Update(Product entity, int[] categoryIds)
        {
            _productRepository.Update(entity, categoryIds);
        }
    }
}
