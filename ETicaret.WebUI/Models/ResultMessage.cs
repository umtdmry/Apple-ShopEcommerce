using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.WebUI.Models
{
    //Bilgi kutucuğunun key kısmı için oluşturduğumuz model
    public class ResultMessage
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Css { get; set; } //success, warning, danger

    }
}
