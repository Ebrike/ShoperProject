using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Entities
{
    public class ProductPrice
    {
        public int PriceId { get; set; }
        public UnitPrice UnitPrice { get; set; }

        public int ProductId { get; set; } // hangi ürüne ait olduğunu belli etmek için
                                          //foreign key 
        public decimal Price { get; set; }
        public bool isValidFlag { get; set; } // ürünün fiyatının geçerli olup olmadığı

        public Product Product { get; set; } // bu fiyat bilgisi bir ürüne ait 



    }
    public enum UnitPrice
    {
        Dolar,
        Lira,
        Euro
    }
}
