using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.WebUI.Middlewares
{
    
    public static class ApplicationBuilderExtensions
    {
        //Middleware oluşturuyoruz
        public static IApplicationBuilder CustomStaticFiles(this IApplicationBuilder app)
        {
            //Pathimizi aldık dışarı açıcağımız path olarak "node_modules" kullandık.
            var path = Path.Combine(Directory.GetCurrentDirectory(), "node_modules");

            //Options oluşturuyoruz.
            //2 özellik tanımlamalıyız.
            //Özelliklerimizi gerekli yapıda dolduruyoruz.
            var options = new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(path),
                RequestPath = "/modules"
            };

            app.UseStaticFiles(options);
            return app;
        }
    }
}
