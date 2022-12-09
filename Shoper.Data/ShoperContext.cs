using Microsoft.EntityFrameworkCore;
using Shoper.Entities;

namespace Shoper.Data
{
    public class ShoperContext : DbContext
    {
        public ShoperContext()
        {

        }
        public DbSet<Category> Categories { get; set; } // dbset- verilen bilgileri tablo gibi oluşturur. 
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductPrice> ProductPrices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=LAPTOP-9SRIKLEO\SQLEXPRESS;Database=ShoperDbContext;Trusted_Connection=True;TrustServerCertificate=True");
            // sql server bağlantısı için 

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // tablo ismi ve tabloya ait primary key verilir. 
            modelBuilder.Entity<Category>().ToTable("Category").HasKey(c=>c.CategoryId);// to table- tablo adı ,haskey- primary key 
            modelBuilder.Entity<Product>().ToTable("Product").HasKey(p=>p.ProductId);
            modelBuilder.Entity<ProductImage>().ToTable("ProductImage").HasKey(pi => pi.ImageId);
            modelBuilder.Entity<ProductPrice>().ToTable("ProductPrice").HasKey(pp=>pp.PriceId);
            // FLUENT API
            // migration oluştururken burası çalışacak 


            // RELATIONS ///
            // CATEGORY-PRODUCT 
            modelBuilder.Entity<Category>()
                .HasMany<Product>(c=>c.Products)
                .WithOne(p=>p.ProductCategory)
                .HasForeignKey(p=>p.CategoryId)
                .HasConstraintName("Fk_ProductToCategory")
                .OnDelete(DeleteBehavior.Cascade);
            // bir kategori silindiğinde kategoriye ait ürünler de silinsin - on delete cascade

            //PRODUCT-PRODUCTPRICE 
            modelBuilder.Entity<ProductPrice>()
                .HasOne<Product>(pp => pp.Product)
                .WithMany(p => p.ProductPrice)
                .HasForeignKey(pp => pp.ProductId)
                .HasConstraintName("Fk_ProductPriceToProduct");

            //PRODUCT-PRODUCTIMAGE
            modelBuilder.Entity<ProductImage>()
               .HasOne<Product>(pp => pp.Product)
               .WithMany(p => p.ProductImage)
               .HasForeignKey(pp => pp.ProductId)
               .HasConstraintName("Fk_ProductImageToProduct");

        }
    }
}