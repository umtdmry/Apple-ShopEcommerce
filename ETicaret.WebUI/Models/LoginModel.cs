using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.WebUI.Models
{
    //Uygulamada kullanılan verileri temsil eder ve verilerin işlenme mantığının saklandığı kısımdır. Verilerin validasyonu burada yapıyoruz.
    //Giriş yapmak için gerekli model
    public class LoginModel
    {
        //İnputun boş olmaması gerektiğini belirttiğimiz attribute
        [Required(ErrorMessage = "Epostanızı giriniz !")]
        //İnputun içindeki verinin tipinin nasıl olması gerektiğini belirttiğimiz attribute
        [DataType(DataType.EmailAddress, ErrorMessage = "Lütfen geçerli bir eposta adresi giriniz !")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifrenizi giriniz !")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }


    }
}
