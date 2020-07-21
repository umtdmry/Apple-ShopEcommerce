using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaret.Entities
{
    //Sepet içindeki ürünleri bilgimizi tuttuğumuz entity sınıfımızdır.
    public class CartItem
    {
        public int Id { get; set; }

        public Product Product { get; set; }
        public int ProductId { get; set; }

        public Cart Cart { get; set; }
        public int CartId { get; set; }

        public int Quantity { get; set; }

    }
}
