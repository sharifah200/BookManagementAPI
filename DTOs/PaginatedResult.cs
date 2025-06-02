// ==================== DTOs/BookCreateDto.cs ====================
namespace BookManagementAPI.DTOs;

/// <summary>
/// نموذج الاستجابة المقسمة للصفحات
/// Paginated response model
/// </summary>
/// <typeparam name="T">نوع البيانات المراد تقسيمها</typeparam>
public class PaginatedResult<T>
{
    /// <summary>
    /// البيانات المُرجعة
    /// Returned data
    /// </summary>
    public IEnumerable<T> Data { get; set; } = new List<T>();

    /// <summary>
    /// رقم الصفحة الحالية
    /// Current page number
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// حجم الصفحة
    /// Page size
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// العدد الإجمالي للعناصر
    /// Total count of items
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// العدد الإجمالي للصفحات
    /// Total number of pages
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// هل توجد صفحة سابقة
    /// Whether there is a previous page
    /// </summary>
    public bool HasPreviousPage { get; set; }

    /// <summary>
    /// هل توجد صفحة تالية
    /// Whether there is a next page
    /// </summary>
    public bool HasNextPage { get; set; }
}
