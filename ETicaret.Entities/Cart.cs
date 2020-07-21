using System.Collections.Generic;

namespace ETicaret.Entities
{
    //Sepet bilgimizi tuttuğumuz entity sınıfımızdır.
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public List<CartItem> CartItems { get; set; }
    }
}
