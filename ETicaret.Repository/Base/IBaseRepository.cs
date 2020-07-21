using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ETicaret.DataAccess.Base
{
    //Repository Katmanı bizim veri tabanı işlemlerimizi gerçekleştirdiğimiz katmandır.
    //BaseRepository interfacemiz.
    //İnterfacelerimizde ortak kulancağımız metodlarımızı buraya yazıp bunu BaseRepositoryi diğerlerine kalıtıcağız.
    //Böylece kod tekrarını engelleyip daha güvenli kod yazmamızı sağlayacağız.
    //Kalıttığımız repository sınıflarımıza parametre olarak entity sınıflarımızı göndermemiz yeterli olucak.
    public interface IBaseRepository<TEntity>
    {
        TEntity GetById(int id);
        TEntity GetOne(Expression<Func<TEntity, bool>> filter);
        List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null);

        void Create(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
    }
}
