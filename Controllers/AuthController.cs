using System.Text;
using System.Security.Claims;
using BookManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

// ==================== Controllers/AuthController.cs ====================


namespace BookManagementAPI.Controllers;

/// <summary>
/// تحكم في عمليات المصادقة والتفويض
/// Controller for authentication and authorization operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager,
    IConfiguration configuration,
    ILogger<AuthController> logger) : ControllerBase
{
    /// <summary>
    /// تسجيل مستخدم جديد
    /// Register a new user
    /// </summary>
    /// <param name="model">بيانات التسجيل</param>
    /// <returns>نتيجة التسجيل</returns>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Register([FromBody] RegisterModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new IdentityUser
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                logger.LogInformation("User {UserName} registered successfully", model.UserName);
                return Ok(new { message = "تم التسجيل بنجاح" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred during user registration");
            return BadRequest("حدث خطأ أثناء التسجيل");
        }
    }

    /// <summary>
    /// تسجيل دخول المستخدم
    /// User login
    /// </summary>
    /// <param name="model">بيانات تسجيل الدخول</param>
    /// <returns>رمز الوصول JWT</returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Login([FromBody] LoginModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await signInManager.PasswordSignInAsync(
                model.UserName, model.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await userManager.FindByNameAsync(model.UserName);
                var token = GenerateJwtToken(user!);

                logger.LogInformation("User {UserName} logged in successfully", model.UserName);

                return Ok(new
                {
                    token = token,
                    expiration = DateTime.UtcNow.AddHours(24),
                    message = "تم تسجيل الدخول بنجاح"
                });
            }

            logger.LogWarning("Failed login attempt for user {UserName}", model.UserName);
            return Unauthorized("بيانات تسجيل الدخول غير صحيحة");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred during user login");
            return BadRequest("حدث خطأ أثناء تسجيل الدخول");
        }
    }

    /// <summary>
    /// توليد رمز JWT
    /// Generate JWT token
    /// </summary>
    /// <param name="user">المستخدم</param>
    /// <returns>رمز JWT</returns>
    private string GenerateJwtToken(IdentityUser user)
    {
        var jwtKey = configuration["Jwt:Key"];
        var jwtIssuer = configuration["Jwt:Issuer"];
        var jwtAudience = configuration["Jwt:Audience"];

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
