using ETicaret.Business.Base;
using ETicaret.DataAccess.Base;
using ETicaret.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaret.Business.Manager
{
    //Sepet İşlemleri Service
    //ICategoryService'i implement edip ordaki metodları kullanmaktadır.
    public class CategoryManager : ICategoryService
    {

        private ICategoryRepository _categoryRepository;

        //Öncelikle Constructor inject yapıyoruz.
        //Bunun nedeni veritabanı işlemleri ile service işlemlerimizi ayırdık. Ayırmamızın nedeni ise SOLID kurallarından olan Dependency Injection ile olabildiğince geliştirmeye açık programlama yapabilmek. 
        //Buradan veritabanına bir işlem göndermek için repositorydeki metoda ulaşmalıyız. Bunun içinde gerekli repositoryden nesne üretip bu nesneyi inject ediyoruz.
        public CategoryManager(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        //**********************************************************
        //Aşağıdaki metodlarda gerekli logic işlemleri yapıyoruz.
        //Eğer bir logiç işlem deilde veri tabanı işlemi gerekiyorsa
        //repositorydeki gerekli metodumuzu çağırıyoruz.
        //**********************************************************

        //Kategori Oluşur
        public void Create(Category entity)
        {
            _categoryRepository.Create(entity);
        }

        //Kategori sil
        public void Delete(Category entity)
        {
            _categoryRepository.Delete(entity);

        }

        //Kategoriden ürünü sil
        public void DeleteFromCategory(int categoryId, int productId)
        {
            _categoryRepository.DeleteFromCategory(categoryId, productId);
        }

        //Kategorilerin hepsini getir
        public List<Category> GetAll()
        {
            return _categoryRepository.GetAll();
        }

        //Id'si verilen kategoriyi getir
        public Category GetById(int id)
        {
            return _categoryRepository.GetById(id);

        }

        //Ürünün kategorilerini getir
        public Category GetByIdWithProducts(int id)
        {
            return _categoryRepository.GetByIdWithProducts(id);
        }

        //Kategoriyi güncelle
        public void Update(Category entity)
        {
            _categoryRepository.Update(entity);

        }
    }
}
