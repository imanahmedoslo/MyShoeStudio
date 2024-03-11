using Microsoft.EntityFrameworkCore;
using MyShoeStudio.Data.Models; 
namespace MyShoeStudio.Data
{
    public class MyShoeStudioDbContext:DbContext
    {
        public MyShoeStudioDbContext(DbContextOptions<MyShoeStudioDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<Product_ShoppingList> Product_ShoppingLists { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Product_Wishlist> Product_Wishlists { get; set; }

        public DbSet<PersonalInfo> PersonalInfos { get; set; }
      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

    }
}
