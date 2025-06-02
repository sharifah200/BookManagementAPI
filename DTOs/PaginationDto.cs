// ==================== DTOs/BookCreateDto.cs ====================
namespace BookManagementAPI.DTOs;

// ==================== DTOs/PaginationDto.cs ====================
/// <summary>
/// نموذج البيانات للصفحات
/// Data transfer object for pagination
/// </summary>
public class PaginationDto
{
    private int _pageNumber = 1;
    private int _pageSize = 10;

    /// <summary>
    /// رقم الصفحة (الافتراضي 1)
    /// Page number (default 1)
    /// </summary>
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    /// <summary>
    /// حجم الصفحة (الافتراضي 10، الحد الأقصى 50)
    /// Page size (default 10, maximum 50)
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 50 ? 50 : value < 1 ? 10 : value;
    }
}
