using ETicaret.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaret.DataAccess.Base
{
    //Repository Katmanı bizim veri tabanı işlemlerimizi gerçekleştirdiğimiz katmandır.
    //ICategoryRepository'e Base repositoryi kalıttık ve içinde o metodları kullanmamızıda sağladık.
    //Parametre olarak Category Entitysini gönderdik.
    //Ekstra gerekli metodlarımızı buraya yazdık.
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Category GetByIdWithProducts(int id);
        void DeleteFromCategory(int categoryId, int productId);
    }
}
