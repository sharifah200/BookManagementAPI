// ==================== Repositories/IBooksRepository.cs ====================
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementAPI.Contracts;

// ==================== Repositories/IBooksRepository.cs ====================

/// <summary>
/// واجهة مستودع الكتب
/// Books repository interface
/// </summary>
public interface IBooksRepository : IBaseRepository<Book>
{
    /// <summary>
    /// الحصول على الكتب مع التقسيم للصفحات
    /// Get books with pagination
    /// </summary>
    Task<PaginatedResult<Book>> GetBooksAsync(PaginationDto pagination, string? searchTerm = null);

    /// <summary>
    /// البحث عن الكتب بالعنوان أو المؤلف
    /// Search books by title or author
    /// </summary>
    Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm);

    /// <summary>
    /// الحصول على الكتب بواسطة المؤلف
    /// Get books by author
    /// </summary>
    Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author);
}