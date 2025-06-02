using BookManagementAPI.Contracts;
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// ==================== Controllers/AuthController.cs ====================


namespace BookManagementAPI.Controllers;

// ==================== Controllers/BooksController.cs ====================

/// <summary>
/// تحكم في عمليات إدارة الكتب
/// Controller for book management operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BooksController(IBooksRepository booksRepository, ILogger<BooksController> logger) : ControllerBase
{

    /// <summary>
    /// الحصول على جميع الكتب مع التقسيم للصفحات
    /// Get all books with pagination
    /// </summary>
    /// <param name="pageNumber">رقم الصفحة (الافتراضي 1)</param>
    /// <param name="pageSize">حجم الصفحة (الافتراضي 10، الحد الأقصى 50)</param>
    /// <param name="searchTerm">مصطلح البحث (اختياري)</param>
    /// <returns>قائمة الكتب مع معلومات الصفحات</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedResult<Book>>> GetBooks(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null)
    {
        try
        {
            var pagination = new PaginationDto { PageNumber = pageNumber, PageSize = pageSize };
            var result = await booksRepository.GetBooksAsync(pagination, searchTerm);

            logger.LogInformation("Retrieved {Count} books from page {PageNumber}",
                result.Data.Count(), result.PageNumber);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving books");
            return BadRequest("حدث خطأ أثناء جلب الكتب");
        }
    }

    /// <summary>
    /// الحصول على كتاب محدد بالمعرف
    /// Get a specific book by ID
    /// </summary>
    /// <param name="id">معرف الكتاب</param>
    /// <returns>تفاصيل الكتاب</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
        try
        {
            var book = await booksRepository.GetByIdAsync(id);

            if (book == null)
            {
                logger.LogWarning("Book with ID {BookId} not found", id);
                return NotFound($"الكتاب بالمعرف {id} غير موجود");
            }

            logger.LogInformation("Retrieved book with ID {BookId}", id);
            return Ok(book);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving book with ID {BookId}", id);
            return BadRequest("حدث خطأ أثناء جلب الكتاب");
        }
    }

    /// <summary>
    /// إضافة كتاب جديد (يتطلب المصادقة)
    /// Add a new book (requires authentication)
    /// </summary>
    /// <param name="bookDto">بيانات الكتاب الجديد</param>
    /// <returns>الكتاب المُنشأ</returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Book>> CreateBook([FromBody] BookCreateDto bookDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = new Book
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                PublishedDate = bookDto.PublishedDate,
                NumberOfPages = bookDto.NumberOfPages,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdBook = await booksRepository.AddAsync(book);
            await booksRepository.SaveAsync();

            logger.LogInformation("Created new book with ID {BookId}", createdBook.Id);

            return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBook);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating book");
            return BadRequest("حدث خطأ أثناء إنشاء الكتاب");
        }
    }

    /// <summary>
    /// تحديث كتاب موجود (يتطلب المصادقة)
    /// Update an existing book (requires authentication)
    /// </summary>
    /// <param name="id">معرف الكتاب</param>
    /// <param name="bookDto">البيانات المحدثة</param>
    /// <returns>الكتاب المحدث</returns>
    [HttpPut("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Book>> UpdateBook(int id, [FromBody] BookUpdateDto bookDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingBook = await booksRepository.GetByIdAsync(id);
            if (existingBook == null)
            {
                logger.LogWarning("Attempted to update non-existent book with ID {BookId}", id);
                return NotFound($"الكتاب بالمعرف {id} غير موجود");
            }

            // تحديث الحقول المتوفرة فقط
            // Update only provided fields
            if (!string.IsNullOrEmpty(bookDto.Title))
                existingBook.Title = bookDto.Title;

            if (!string.IsNullOrEmpty(bookDto.Author))
                existingBook.Author = bookDto.Author;

            if (bookDto.PublishedDate.HasValue)
                existingBook.PublishedDate = bookDto.PublishedDate.Value;

            if (bookDto.NumberOfPages.HasValue)
                existingBook.NumberOfPages = bookDto.NumberOfPages.Value;

            existingBook.UpdatedAt = DateTime.UtcNow;

            var updatedBook = await booksRepository.UpdateAsync(existingBook);
            await booksRepository.SaveAsync();

            logger.LogInformation("Updated book with ID {BookId}", id);

            return Ok(updatedBook);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating book with ID {BookId}", id);
            return BadRequest("حدث خطأ أثناء تحديث الكتاب");
        }
    }

    /// <summary>
    /// حذف كتاب (يتطلب المصادقة)
    /// Delete a book (requires authentication)
    /// </summary>
    /// <param name="id">معرف الكتاب</param>
    /// <returns>رسالة تأكيد الحذف</returns>
    [HttpDelete("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> DeleteBook(int id)
    {
        try
        {
            var deleted = await booksRepository.DeleteAsync(id);
            if (!deleted)
            {
                logger.LogWarning("Attempted to delete non-existent book with ID {BookId}", id);
                return NotFound($"الكتاب بالمعرف {id} غير موجود");
            }

            await booksRepository.SaveAsync();

            logger.LogInformation("Deleted book with ID {BookId}", id);

            return Ok(new { message = $"تم حذف الكتاب بالمعرف {id} بنجاح" });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting book with ID {BookId}", id);
            return BadRequest("حدث خطأ أثناء حذف الكتاب");
        }
    }

    /// <summary>
    /// البحث عن الكتب بالعنوان أو المؤلف
    /// Search books by title or author
    /// </summary>
    /// <param name="searchTerm">مصطلح البحث</param>
    /// <returns>قائمة الكتب المطابقة</returns>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<Book>>> SearchBooks([FromQuery] string searchTerm)
    {
        try
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return BadRequest("مصطلح البحث مطلوب");
            }

            var books = await booksRepository.SearchBooksAsync(searchTerm);

            logger.LogInformation("Search for '{SearchTerm}' returned {Count} books", searchTerm, books.Count());

            return Ok(books);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while searching books with term '{SearchTerm}'", searchTerm);
            return BadRequest("حدث خطأ أثناء البحث عن الكتب");
        }
    }

    /// <summary>
    /// الحصول على الكتب بواسطة المؤلف
    /// Get books by author
    /// </summary>
    /// <param name="author">اسم المؤلف</param>
    /// <returns>قائمة كتب المؤلف</returns>
    [HttpGet("author/{author}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooksByAuthor(string author)
    {
        try
        {
            if (string.IsNullOrEmpty(author))
            {
                return BadRequest("اسم المؤلف مطلوب");
            }

            var books = await booksRepository.GetBooksByAuthorAsync(author);

            logger.LogInformation("Retrieved {Count} books by author '{Author}'", books.Count(), author);

            return Ok(books);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving books by author '{Author}'", author);
            return BadRequest("حدث خطأ أثناء جلب كتب المؤلف");
        }
    }
}