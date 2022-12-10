using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shoper.Entities
{
    public class ProductPrice
    {
        [Display(Name = "Fiyat No")]
        public int PriceId { get; set; }

        [Display(Name = "Fiyat Birimi")]
        public UnitPrice UnitPrice { get; set; }

        [Display(Name = "Ürün Kodu ")]
        public int ProductId { get; set; } // hangi ürüne ait olduğunu belli etmek için
                                           //foreign key 
        [Display(Name = "Ürün Fiyatı ")]
        public decimal Price { get; set; }
        [Display(Name = "Geçerli Fiyat")]
        public bool isValidFlag { get; set; } // ürünün fiyatının geçerli olup olmadığı

        public Product Product { get; set; } // bu fiyat bilgisi bir ürüne ait 



    }
    public enum UnitPrice
    {
        
        Lira,
        Dolar,
        Euro
    }
}
