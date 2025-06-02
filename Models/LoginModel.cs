// ==================== Models/LoginModel.cs ====================
namespace BookManagementAPI.Models;

/// <summary>
/// نموذج بيانات تسجيل الدخول
/// Login data model
/// </summary>
public class LoginModel
{
    /// <summary>
    /// اسم المستخدم أو البريد الإلكتروني
    /// Username or email
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// كلمة المرور
    /// Password
    /// </summary>
    public string Password { get; set; } = string.Empty;
}