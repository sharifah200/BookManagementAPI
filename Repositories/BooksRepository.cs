using BookManagementAPI.Contracts;
using BookManagementAPI.DBContext;
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
// ==================== Repositories/BaseRepository.cs ====================

namespace BookManagementAPI.Repositories;

// ==================== Repositories/BooksRepository.cs ====================
/// <summary>
/// تنفيذ مستودع الكتب
/// Books repository implementation
/// </summary>
public class BooksRepository(BookContext context) : BaseRepository<Book>(context), IBooksRepository
{

    /// <summary>
    /// الحصول على الكتب مع التقسيم للصفحات والبحث
    /// Get books with pagination and search
    /// </summary>
    public async Task<PaginatedResult<Book>> GetBooksAsync(PaginationDto pagination, string? searchTerm = null)
    {
        var query = _dbSet.AsQueryable();

        // تطبيق البحث إذا تم توفير مصطلح البحث
        // Apply search if search term is provided
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm));
        }

        // حساب العدد الإجمالي
        // Calculate total count
        var totalCount = await query.CountAsync();

        // تطبيق التقسيم للصفحات
        // Apply pagination
        var books = await query
            .OrderBy(b => b.Title)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        // حساب معلومات الصفحات
        // Calculate pagination info
        var totalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize);

        return new PaginatedResult<Book>
        {
            Data = books,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasPreviousPage = pagination.PageNumber > 1,
            HasNextPage = pagination.PageNumber < totalPages
        };
    }

    /// <summary>
    /// البحث عن الكتب بالعنوان أو المؤلف
    /// Search books by title or author
    /// </summary>
    public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm)
    {
        return await _dbSet
            .Where(b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm))
            .OrderBy(b => b.Title)
            .ToListAsync();
    }

    /// <summary>
    /// الحصول على الكتب بواسطة المؤلف
    /// Get books by author
    /// </summary>
    public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author)
    {
        return await _dbSet
            .Where(b => b.Author.Contains(author))
            .OrderBy(b => b.PublishedDate)
            .ToListAsync();
    }
}