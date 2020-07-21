using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.WebUI.Models
{
    //Uygulamada kullanılan verileri temsil eder ve verilerin işlenme mantığının saklandığı kısımdır. Verilerin validasyonu burada yapıyoruz.
    //Sipariş için gerekli model
    public class OrderModel
    {
        //İnputun boş olmaması gerektiğini belirttiğimiz attribute
        [Required(ErrorMessage = "Sipariş Sahibinin Adını Giriniz !")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Sipariş Sahibinin Soyadını Giriniz !")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Sipariş Gönderilceği Adresi Giriniz !")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Sipariş Gönderilceği Adresin Hangi Şehirde Olduğunu Giriniz !")]
        public string City { get; set; }

        //İnputun içindeki verinin sadece sayı olması gerektiğini belirttiğimiz attribute
        [RegularExpression("([0-9]+)", ErrorMessage = "Buraya Sadece Rakam Giriniz")]
        [Required(ErrorMessage = "Sipariş Sahibinin Telefon Numarasını Giriniz !")]
        //İnputun boyutunu belirttiğimiz attribute
        [StringLength(11, MinimumLength = 10, ErrorMessage = "Telefon Numaranızı Doğru Giriniz !")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Epostanızı Giriniz !")]
        //İnputun içindeki verinin tipinin nasıl olması gerektiğini belirttiğimiz attribute
        [DataType(DataType.EmailAddress, ErrorMessage = "Lütfen Geçerli Bir Eposta Adresi Giriniz !")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lütfen Kart Sahibinin İsmini Giriniz !")]
        public string CardName { get; set; }

        [RegularExpression("([0-9]+)", ErrorMessage = "Buraya Sadece Rakam Giriniz")]
        [Required(ErrorMessage = "Lütfen Kart Numarasını Giriniz !")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Lütfen Kart Numaranızı Eksiksiz Giriniz !")]
        public string CardNumber { get; set; }

        [RegularExpression("([0-9]+)", ErrorMessage = "Buraya Sadece Rakam Giriniz")]
        [Required(ErrorMessage = "Lütfen Kartınızın Son Kullanım Ayını Giriniz !")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Lütfen Kart Numaranızı Eksiksiz Giriniz !")]
        public string ExpirationMonth { get; set; }

        [RegularExpression("([0-9]+)", ErrorMessage = "Buraya Sadece Rakam Giriniz")]
        [Required(ErrorMessage = "Lütfen Kartınızın Son Kullanım Yılını Giriniz !")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Lütfen Kart Numaranızı Eksiksiz Giriniz !")]
        public string ExpirationYear { get; set; }

        [RegularExpression("([0-9]+)",ErrorMessage ="Buraya Sadece Rakam Giriniz")]
        [Required(ErrorMessage = "Lütfen Kartınızın CVV'sini Giriniz !")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Lütfen Kart CVV'nizi Eksiksiz Giriniz !")]
        public string Cvv { get; set; }

        public CartModel CartModel { get; set; }
    }
}
