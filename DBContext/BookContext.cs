using BookManagementAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// ==================== Data/BookContext.cs ====================


namespace BookManagementAPI.DBContext;

/// <summary>
/// سياق قاعدة البيانات للكتب مع دعم الهوية
/// Database context for books with Identity support
/// </summary>
public class BookContext(DbContextOptions<BookContext> options) : IdentityDbContext(options)
{
    /// <summary>
    /// مجموعة الكتب في قاعدة البيانات
    /// Books collection in the database
    /// </summary>
    public DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // تكوين نموذج الكتاب
        // Configure Book model
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Title).IsRequired().HasMaxLength(200);
            entity.Property(b => b.Author).IsRequired().HasMaxLength(100);
            entity.Property(b => b.PublishedDate).IsRequired();
            entity.Property(b => b.NumberOfPages).IsRequired();
            entity.Property(b => b.CreatedAt).IsRequired();
            entity.Property(b => b.UpdatedAt).IsRequired();

            // إنشاء فهرس على العنوان والمؤلف لتحسين الأداء
            // Create index on title and author for better performance
            entity.HasIndex(b => b.Title);
            entity.HasIndex(b => b.Author);
        });

        // إضافة بيانات تجريبية
        // Seed data
        modelBuilder.Entity<Book>().HasData(
            new Book
            {
                Id = 1,
                Title = "The Great Gatsby",
                Author = "F. Scott Fitzgerald",
                PublishedDate = new DateTime(1925, 4, 10),
                NumberOfPages = 180,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Book
            {
                Id = 2,
                Title = "To Kill a Mockingbird",
                Author = "Harper Lee",
                PublishedDate = new DateTime(1960, 7, 11),
                NumberOfPages = 376,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );
    }
}