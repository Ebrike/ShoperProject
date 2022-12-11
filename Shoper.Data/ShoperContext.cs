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
        public DbSet<ProductDiscount> ProductDiscounts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }



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
            modelBuilder.Entity<ProductDiscount>().ToTable("ProductDiscount").HasKey(d => d.ProductDiscountId);
            modelBuilder.Entity<Address>().ToTable("Address").HasKey(a => a.AddressId);
            modelBuilder.Entity<Customer>().ToTable("Customer").HasKey(c => c.CustomerId);
            modelBuilder.Entity<ProductComment>().ToTable("ProductComment").HasKey(c => c.CommentId);



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

            //PRODUCT-PRODUCTDISCOUNT
            modelBuilder.Entity<ProductDiscount>()
              .HasOne<Product>(pp => pp.Product)
              .WithMany(p => p.ProductDiscount)
              .HasForeignKey(pp => pp.ProductId)
              .HasConstraintName("Fk_ProductDiscountToProduct");

            //CUSTOMER-ADDRESS
            modelBuilder.Entity<Address>()
              .HasOne<Customer>(pp => pp.Customer)
              .WithMany(p => p.Addresses)
              .HasForeignKey(pp => pp.CustomerId)
              .HasConstraintName("Fk_CustomerToAddress");

            //PRODUCT-PRODUCTCOMMENT
            modelBuilder.Entity<ProductComment>()
              .HasOne<Product>(pp => pp.Product)
              .WithMany(p => p.ProductComment)
              .HasForeignKey(pp => pp.ProductId)
              .HasConstraintName("Fk_ProductCommentToProduct");

            // FLUENT API domain classlar arasında ilişkilendirmek için mapping relation 
        }
    }
}