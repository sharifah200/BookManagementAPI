// ==================== Models/RegisterModel.cs ====================
namespace BookManagementAPI.Models;

/// <summary>
/// نموذج بيانات التسجيل
/// Registration data model
/// </summary>
public class RegisterModel
{
    /// <summary>
    /// اسم المستخدم
    /// Username
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// البريد الإلكتروني
    /// Email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// كلمة المرور
    /// Password
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
