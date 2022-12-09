using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Entities
{
    public class ProductImage
    {
        public int ImageId { get; set; }
        public int ProductId { get; set; } // foreign key
                                           // hangi ürünün resmi olduğu
        public byte[] Image { get; set; } // veri tabanında bunu bytelara çevirerek kaydetsin
        public Product Product { get; set; }

    }
}
