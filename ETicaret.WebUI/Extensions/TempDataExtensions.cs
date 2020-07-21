using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ETicaret.WebUI.Extensions
{
    //Bu sınıf TempData ile sayfa değiştirdiğimizde bilgi kutuçuğu göstermemiz amacı ile oluşturulmuştur.
    //Key value mantığı ile çalışmaktadır.
    //Dökümantasyonundan alınarak kullanılmıştır.
    public static class TempDataExtensions
    {
        //Put fonksiyonu ile keylerin valuelerini vererek gerekli
        //ResultMassage içindeki değişkenler keylerimiz olmaktadır.
        //Put ile bir değer koyduğumuzda ve işlem gerçekleştirip başka bir sayfaya geçiş yaptığımızda bilgi kutucuğu çıkartmaktayız.
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T:class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }

        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object o;
            tempData.TryGetValue(key, out o);
            return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
        }

    }
}
