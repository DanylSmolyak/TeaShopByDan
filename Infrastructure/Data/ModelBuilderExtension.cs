using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class ModelBuilderExtension
{
    public static void Configuration(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.Property(x => x.UserID).IsRequired(); 
            entity.Property(x => x.IsDeleted).IsRequired();

          
            entity.HasMany(x => x.CartItems)
                .WithOne(x => x.Cart)
                .HasForeignKey(x => x.CartId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(x => x.id); 
            entity.Property(x => x.CartId).IsRequired(); 
            entity.Property(x => x.ProductID).IsRequired();
            entity.Property(x => x.Quantity).IsRequired();
            entity.Property(x => x.UnitPrice).IsRequired();
            
            entity.HasOne(x => x.Product)
                .WithMany(x => x.CartItems)
                .HasForeignKey(x => x.ProductID)
                .OnDelete(DeleteBehavior.Cascade);
        });
        

         modelBuilder.Entity<User>(entity =>
         {
             entity.HasKey(x => x.Id);
             
             entity.Property(x => x.Email).IsRequired();
             entity.Property(x => x.Address).IsRequired(false);
                
             entity.HasMany(x => x.Orders)
                 .WithOne(x => x.UserServices)
                 .HasForeignKey(x => x.UserID)
                 .OnDelete(DeleteBehavior.Cascade);
                    
             entity.HasMany(x => x.Reviews)
                 .WithOne(x => x.UserServices)
                 .HasForeignKey(x => x.UserID)
                 .OnDelete(DeleteBehavior.NoAction);
                
             entity.HasMany(x => x.Blogs)
                 .WithOne(x => x.UserServices)
                 .HasForeignKey(x => x.UserID)
                 .OnDelete(DeleteBehavior.NoAction);

             entity.HasMany(x => x.BlogComments)
                 .WithOne(x => x.UserServices)
                 .HasForeignKey(x => x.UserID)
                 .OnDelete(DeleteBehavior.NoAction);
             
         });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(x => x.id);
            entity.Property(x => x.UserID).IsRequired();

            entity.HasMany(x => x.OrdersDetails)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderID)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(x => x.id);
            
            entity.Property(x => x.OrderID).IsRequired();
            entity.Property(x => x.ProductID).IsRequired();
            entity.Property(x => x.UnitPrice).IsRequired();
            entity.Property(x => x.Quantity).IsRequired();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(x => x.id);

            entity.Property(x => x.Name).IsRequired();
            entity.Property(x => x.Stock).IsRequired();
            entity.Property(x => x.Description).IsRequired(false);
            entity.Property(x => x.CategoryID).HasDefaultValue(null);
            entity.Property(x => x.AverageRating).HasDefaultValue(0);

            entity.HasMany(x => x.OrdersDetails)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(x => x.CartItems)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductID);


            entity.HasOne(x => x.Categorie)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryID)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(x => x.Photos)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId);
            
            entity.HasMany(x => x.ProductPrices)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId); 
            
            modelBuilder.Entity<Product>()
                .HasOne(p => p.TeaDetail)
                .WithOne(td => td.Product)
                .HasForeignKey<TeaDetail>(td => td.ProductId);
            
            entity.HasMany(x => x.Reviews)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductID)
                .OnDelete(DeleteBehavior.Cascade);
            
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(x => x.id);
            
            entity.Property(x => x.Title).IsRequired();
            entity.Property(x => x.UserID).IsRequired();
            entity.Property(x => x.Content).IsRequired();
            
            entity.HasMany(x => x.BlogComments)
                .WithOne(x => x.Blog)
                .HasForeignKey(x => x.BlogID)
                .OnDelete(DeleteBehavior.Cascade);

        });
        
        modelBuilder.Entity<BlogComment>(entity =>
        {
            entity.HasKey(x => x.id);
            entity.Property(x => x.Comment).IsRequired();
            entity.Property(x => x.BlogID).IsRequired();
            entity.Property(x => x.UserID).IsRequired();
        });

        modelBuilder.Entity<Categorie>(entity =>
        {
            entity.HasKey(x => x.id);
            entity.Property(x => x.CategoryName).IsRequired();
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(x => x.id);
            entity.Property(x => x.Rating).IsRequired();
            entity.Property(x => x.UserID).IsRequired();
            entity.Property(x => x.ProductID).IsRequired();
            entity.Property(x => x.Comment).IsRequired();
        });
        
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(x => x.id);
            
        });

        modelBuilder.Entity<ProductPrice>(entity =>
        {
            entity.HasKey(x => x.id);
            entity.Property(x => x.Price).IsRequired();
            entity.Property(x => x.WeightGrams).IsRequired();
            entity.Property(x => x.ProductId).IsRequired();
        });

        modelBuilder.Entity<TeaDetail>(entity =>
        {
            entity.HasKey(x => x.id);
            entity.Property(x => x.ProductId).IsRequired();

            
        });


    }
}