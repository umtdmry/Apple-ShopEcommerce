using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETicaret.Business.Services;
using ETicaret.WebUI.Extensions;
using ETicaret.WebUI.Identity;
using ETicaret.WebUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ETicaret.WebUI.Controllers
{
    //CSRF saldırısı denilen sahte token üretip kullanıcı bilgilerine ulaşmaya yarayan bir saldırı çeşidini engelleyen bir attribute.
    [AutoValidateAntiforgeryToken]
    //Controller Katmanı MVC yapısının client ile bağlı olan kısmıdır.
    //Gelen isteği karşılar. İsteğe göre gerekli ActionResult çalıştırılır.
    //Buradan direkt cliente geri cevap dönülebilir (sayfa açılabilir), gerekiyorsa service veri göndürülüp gerekli işlemlerin yapılması sağlanabilir.
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IEmailSender _emailSender;
        private ICartService _cartService;

        //Burada bir UserManager, SignInManager, IEmailSender, ICartService nesnelerini kullanılabilmek için constructor injection yapıyoruz.
        public AccountController(ICartService cartService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cartService = cartService;
            _emailSender = emailSender;

        }

        //Giriş sayfasına yönlendiriyoruz.
        public IActionResult Login(string ReturnUrl = null)
        {
            return View(new LoginModel()
            {
                ReturnUrl = ReturnUrl
            });
        }

        //HttpPost attributesinin verilmesinin sebebi bir post işlemi yapılıyor.
        //Login işlemi gerçekleştiriliyor.
        //Metodun async tipinde olması asenkron çalıştığını göstermektedir.
        //Task ibareside asyncden dolaylı eklenmiştir.
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {

            //Doğrulama?
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Bu epostaya ait kullanıcı bulunamadı !");
                return View(model);
            }

            //Eposta Doğrulama?
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Lütfen hesabınızı epostanıza gelecek olan mail ile onaylayınız !");
                return View(model);

            }

            //Buradaki await PasswordSignInAsync işleminden cevap gelmeden projeyi devam ettirmeyeceğini belirtmektedir.
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

            //Giriş?
            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl ?? "~/");
            }

            ModelState.AddModelError("", "Eposta ya da şifreniz hatalı!");
            return View(model);
        }

        //Üye ol sayfasına yönlendiriyoruz.
        public IActionResult Register()
        {
            return View(new RegisterModel());
        }

        //HttpPost attributesinin verilmesinin sebebi bir post işlemi yapılıyor.
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var callbackUrl = Url.Action("ConfirmEmail", "Account", new
                {

                    userId = user.Id,
                    token = code
                });

                await _emailSender.SendEmailAsync(model.Email, "Hesabınızı Onaylayınız. !", $"Lütfen üyeliğinizi aktifleştirmek için linke <a href='http://localhost:54502{callbackUrl}'>tıklayınız.</a>");


                TempData.Put("message", new ResultMessage()
                {
                    Title = "Hesabınız oluşturuldu !",
                    Message = "Hesabınızı kullanabilmeniz için epostanıza gelen linkten aktifleştirmeniz gerekmektedir !",
                    Css = "success"
                });
                return RedirectToAction("Login", "Account");

            }

            ModelState.AddModelError("", "Bilinmeyen Bir Hata Oluştu !");
            return View(model);

        }

        //Üye çıkışı yapıyoruz.
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            TempData.Put("message", new ResultMessage()
            {
                Title = "Hesaptan çıkış yapıldı !",
                Message = "Hesabınız güvenli bir şekilde sonlandırıldı !",
                Css = "success"
            });
            return Redirect("~/");

        }

        //Giriş reddedildi sayfasına yönlendirme.
        public IActionResult Accessdenied()
        {

            return View();
        }

        //Hesap onaylandı sayfasına yönlendirme.
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {

            if (userId==null || token==null)
            {
                TempData["message"] = "Geçersiz Token !";

                return View();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user!=null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {

                    _cartService.InitializeCart(userId);

                    TempData.Put("message", new ResultMessage()
                    {
                        Title = "Hesabınız Onaylandı !",
                        Message = "Hesabınız kullanıma hazırdır !",
                        Css = "success"
                    });

                    return RedirectToAction("Login", "Account");


                }
            }
           
            //TempData["message"] = "Hesabınız onaylanmadı !";

            return View();
        }
        
        //Şifremi unutum sayfasına yönlendirme.
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //HttpPost attributesinin verilmesinin sebebi bir post işlemi yapılıyor.
        //Şifremi unuttum linkinin maile gönderilme işlemi.
        //İşlem sonunda Logine yönlendir.
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            //Email?
            if (string.IsNullOrEmpty(Email))
            {
                
                return View();
            }

            var user =await _userManager.FindByEmailAsync(Email);

            //user?
            if (user==null)
            {

                return View();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            //kodu üret
            var callbackUrl = Url.Action("ResetPassword", "Account", new
            {
                token = code
            });

            //maile gerekli linki yolla
            await _emailSender.SendEmailAsync(Email, "Şifre Sıfırlama !", $"Lütfen şifrenizi sıfırlamak için linke <a href='http://localhost:54502{callbackUrl}'>tıklayınız.</a>");

            TempData.Put("message", new ResultMessage()
            {
                Title = "Şifre sıfırlama isteği alındı !",
                Message = "Hesabınıza şifre sıfılama linki gönderildi !",
                Css = "success"
            });
            return RedirectToAction("Login", "Account");

        }

        //Password yenileme sayfasına yönlendirme
        public IActionResult ResetPassword( string token)
        {
            if (token==null)
            {
                return RedirectToAction("Home", "Index");
            }

            var model = new ResetPasswordModel { Token = token };

            return View(model);
        }

        //HttpPost attributesinin verilmesinin sebebi bir post işlemi yapılıyor.
        //Password Yenileme işlemi.
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return RedirectToAction("Home", "Index");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }
    }
}