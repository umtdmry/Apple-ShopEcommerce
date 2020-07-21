using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.WebUI.Models
{
    //Uygulamada kullanılan verileri temsil eder ve verilerin işlenme mantığının saklandığı kısımdır. Verilerin validasyonu burada yapıyoruz.
    //Üye olmak için gerekli model
    public class RegisterModel
    {
        //İnputun boş olmaması gerektiğini belirttiğimiz attribute
        [Required(ErrorMessage = "İsminizi giriniz !")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Kullanıcı adınızı giriniz !")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Şifrenizi giriniz !"), MinLength(6, ErrorMessage = "Şifreniz minimum 6 karakterden oluşmalı"),]
        //İnputun içindeki verinin tipinin nasıl olması gerektiğini belirttiğimiz attribute
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifrenizi tekrar giriniz !"), ]
        [DataType(DataType.Password)]

        //Password alanı ile aynı texte sahip mi kontrolü için gerekli attribute
        [Compare("Password",ErrorMessage ="Password alanı ile RePassword alanı eşleşmeli. Kontrol ediniz !")]
        public string RePassword { get; set; }

        [Required(ErrorMessage = "E posta adresinizi giriniz !")]
        [DataType(DataType.EmailAddress, ErrorMessage ="Lütfen geçerli bir email adresi giriniz !")]
        public string Email { get; set; }


    }
}
