using System.ComponentModel.DataAnnotations;

// ==================== DTOs/BookCreateDto.cs ====================
namespace BookManagementAPI.DTOs;

/// <summary>
/// نموذج البيانات لإنشاء كتاب جديد
/// Data transfer object for creating a new book
/// </summary>
public class BookCreateDto
{
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
}

// ==================== DTOs/BookUpdateDto.cs ====================

/// <summary>
/// نموذج البيانات لتحديث كتاب موجود
/// Data transfer object for updating an existing book
/// </summary>
public class BookUpdateDto
{
    /// <summary>
    /// عنوان الكتاب (اختياري)
    /// Title of the book (optional)
    /// </summary>
    [StringLength(200, ErrorMessage = "عنوان الكتاب يجب أن يكون أقل من 200 حرف")]
    public string? Title { get; set; }

    /// <summary>
    /// اسم المؤلف (اختياري)
    /// Author name (optional)
    /// </summary>
    [StringLength(100, ErrorMessage = "اسم المؤلف يجب أن يكون أقل من 100 حرف")]
    public string? Author { get; set; }

    /// <summary>
    /// تاريخ النشر (اختياري)
    /// Publication date (optional)
    /// </summary>
    public DateTime? PublishedDate { get; set; }

    /// <summary>
    /// عدد الصفحات (اختياري)
    /// Number of pages (optional)
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "عدد الصفحات يجب أن يكون أكبر من صفر")]
    public int? NumberOfPages { get; set; }
}
