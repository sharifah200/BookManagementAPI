using BookManagementAPI.Contracts;
using BookManagementAPI.DBContext;
using Microsoft.EntityFrameworkCore;
// ==================== Repositories/BaseRepository.cs ====================

namespace BookManagementAPI.Repositories;

/// <summary>
/// المستودع الأساسي العام
/// Generic base repository implementation
/// </summary>
/// <typeparam name="T">نوع الكيان</typeparam>
public class BaseRepository<T>(BookContext context) : IBaseRepository<T> where T : class
{
    protected readonly BookContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    /// <summary>
    /// الحصول على جميع الكيانات
    /// Get all entities
    /// </summary>
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>
    /// الحصول على كيان بالمعرف
    /// Get entity by id
    /// </summary>
    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <summary>
    /// إضافة كيان جديد
    /// Add new entity
    /// </summary>
    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    /// <summary>
    /// تحديث كيان موجود
    /// Update existing entity
    /// </summary>
    public virtual async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return entity;
    }

    /// <summary>
    /// حذف كيان
    /// Delete entity
    /// </summary>
    public virtual async Task<bool> DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null)
            return false;

        _dbSet.Remove(entity);
        return true;
    }

    /// <summary>
    /// حفظ التغييرات
    /// Save changes
    /// </summary>
    public virtual async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
