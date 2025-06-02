// ==================== Models/Book.cs ====================
using System.ComponentModel.DataAnnotations;

namespace BookManagementAPI.Models;

/// <summary>
/// نموذج الكتاب - يمثل الكتاب في قاعدة البيانات
/// Book model - represents a book entity in the database
/// </summary>
public class Book
{
    /// <summary>
    /// معرف الكتاب الفريد
    /// Unique identifier for the book
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// عنوان الكتاب
    /// Title of the book
    /// </summary>
    [Required(ErrorMessage = "عنوان الكتاب مطلوب")]
    [StringLength(200, ErrorMessage = "عنوان الكتاب يجب أن يكون أقل من 200 حرف")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// اسم المؤلف
    /// Author name
    /// </summary>
    [Required(ErrorMessage = "اسم المؤلف مطلوب")]
    [StringLength(100, ErrorMessage = "اسم المؤلف يجب أن يكون أقل من 100 حرف")]
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// تاريخ النشر
    /// Publication date
    /// </summary>
    [Required(ErrorMessage = "تاريخ النشر مطلوب")]
    public DateTime PublishedDate { get; set; }

    /// <summary>
    /// عدد الصفحات
    /// Number of pages
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "عدد الصفحات يجب أن يكون أكبر من صفر")]
    public int NumberOfPages { get; set; }

    /// <summary>
    /// تاريخ الإنشاء
    /// Creation date
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// تاريخ آخر تحديث
    /// Last update date
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
