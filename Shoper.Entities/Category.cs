namespace Shoper.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryImagePath { get; set; } // resim yüklemek için - 1 kategoriye ait bir image var 

        public ICollection<Product> Products { get; set; }
    }
}