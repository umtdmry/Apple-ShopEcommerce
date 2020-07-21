using ETicaret.DataAccess.Base;
using ETicaret.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ETicaret.Repository.Base
{
    //Repository Katmanı bizim veri tabanı işlemlerimizi gerçekleştirdiğimiz katmandır.
    //ICartRepository'e Base repositoryi kalıttık ve içinde o metodları kullanmamızıda sağladık.
    //Parametre olarak Cart Entitysini gönderdik.
    //Ekstra gerekli metodlarımızı buraya yazdık.
    public interface ICartRepository : IBaseRepository<Cart>
    {
        Cart GetByUserId(string userId);
        void DeleteFromCart(int cartId, int productId);
        void ClearCart(string cartId);
    }
}
