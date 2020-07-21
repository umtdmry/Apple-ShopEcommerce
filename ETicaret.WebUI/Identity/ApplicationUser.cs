using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.WebUI.Identity
{
    //User Tablomuzu oluşturan class.
    //Identity içinden gelen değerlere ek olarak FullName değişkenini kullanıcı tablomuza eklemekteyiz.
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
