using ETicaret.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaret.DataAccess.Base
{
    //Repository Katmanı bizim veri tabanı işlemlerimizi gerçekleştirdiğimiz katmandır.
    //IProductRepository'e Base repositoryi kalıttık ve içinde o metodları kullanmamızıda sağladık.
    //Parametre olarak Product Entitysini gönderdik.
    //Ekstra gerekli metodlarımızı buraya yazdık.
    public interface IProductRepository : IBaseRepository<Product>
    {
        List<Product> GetProductByCategory(string category, int page, int pageSize);
        Product getProductDetails(int id);
        int GetCountByCategory(string category);
        Product GetByIdWithCategories(int id);
        void Update(Product entity, int[] categoryIds);
    }
}
