using ETicaret.DataAccess.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ETicaret.Repositories.Implement.EfCore
{
    //Genel olarak baktığımızda Repository katmanı bizim veri tabanı işlemlerimizi yapmaktadır.
    //Bunlar kaydetme, silme, güncelleme, Id'ye göre arama fitreye göre bütün verileri ya da bir veriyi getirme gibi işlemlerdir.
    //Gerekli interfaceleri bu kısıma kalıtarak fonksiyonlarımızı burada yazıyoruz.
    //Bu class bizim base repository clasımızdır. Buradaki metodları tekrar tekrar yazmamak için gerekli düzenlemeleri yapıyoruz.
    //where ile parametrelerin ne alıcağını belrliyoruz. Bunu yapma nedenimiz tip güvenli çalışabilmek.
    //using ile her metodda context oluşturuyoruz. Bu contexte göre işlem yapıyoruz. Bunun nedeni using bloğundan çıkılır çıkılmaz GC(Garbage Collector)’ye devredilir ve hemen silinirler. Performans amaçlı yapılmıştır.
    public class EfCoreBaseRepository<TEntity, TContext> : IBaseRepository<TEntity>
        where TEntity : class
        where TContext : DbContext, new()
    {
        public virtual TEntity GetById(int id)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().Find(id);
            }
        }

        public virtual TEntity GetOne(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().Where(filter).SingleOrDefault();
            }
        }

        public virtual List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            using (var context = new TContext())
            {
                return filter == null
                    ? context.Set<TEntity>().ToList() //true
                    : context.Set<TEntity>().Where(filter).ToList(); //false
            }
        }

        public virtual void Create(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Set<TEntity>().Add(entity);
                context.SaveChanges();
            }
        }

        public virtual void Delete(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Set<TEntity>().Remove(entity);
                context.SaveChanges();
            }
        }

        public virtual void Update(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
