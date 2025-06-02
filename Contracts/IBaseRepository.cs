// ==================== Repositories/IBaseRepository.cs ====================
using BookManagementAPI.Repositories;

namespace BookManagementAPI.Contracts;

/// <summary>
/// الواجهة الأساسية للمستودع
/// Base repository interface
/// </summary>
/// <typeparam name="T">نوع الكيان</typeparam>
public interface IBaseRepository<T> where T : class
{
    /// <summary>
    /// الحصول على جميع الكيانات
    /// Get all entities
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// الحصول على كيان بالمعرف
    /// Get entity by id
    /// </summary>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// إضافة كيان جديد
    /// Add new entity
    /// </summary>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// تحديث كيان موجود
    /// Update existing entity
    /// </summary>
    Task<T> UpdateAsync(T entity);

    /// <summary>
    /// حذف كيان
    /// Delete entity
    /// </summary>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// حفظ التغييرات
    /// Save changes
    /// </summary>
    Task<bool> SaveAsync();
}
