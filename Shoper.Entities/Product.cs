using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Entities
{
    public class Product
    {
        [Display(Name ="Ürün Kodu")]
        public int ProductId { get; set; }

        [Display(Name = "Ürün Adı")]
        public string ProductName { get; set; }

        [Display(Name = "Ürün Stoğu")]
        public int ProductStock { get; set; }

        [Display(Name = "Ürün Açıklaması")]
        public string Description { get; set; }

        [Display(Name = "Kategori Adı")]

        public int CategoryId { get; set; }
        public Category ProductCategory { get; set; } // ürünün hangi kategoriye ait olduğu 

        public ICollection<ProductPrice> ProductPrice { get; set; }
        public ICollection<ProductImage>  ProductImage { get; set; }

        public ICollection<ProductDiscount> ProductDiscount { get; set; } // bir ürüne tanımlı birden fazla indrim olabilir. 
        public ICollection<ProductComment> ProductComment { get; set; }
    }
}
