using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.WebUI.Models
{
    //Uygulamada kullanılan verileri temsil eder ve verilerin işlenme mantığının saklandığı kısımdır. Verilerin validasyonu burada yapıyoruz.
    //Şifre Sıfırlama sayfası için gerekli model
    public class ResetPasswordModel
    {
        //İnputun boş olmaması gerektiğini belirttiğimiz attribute
        [Required]
        public string Token { get; set; }

        [Required(ErrorMessage = "E posta adresinizi giriniz !")]
        //İnputun içindeki verinin tipinin nasıl olması gerektiğini belirttiğimiz attribute
        [DataType(DataType.EmailAddress, ErrorMessage = "Lütfen geçerli bir email adresi giriniz !")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifrenizi giriniz !")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
