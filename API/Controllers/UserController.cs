using Core.Dtos;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IPhotoService _photoService;
    public UserController(IUserService userService, IPhotoService photoService)
    {
        _userService = userService;
        _photoService = photoService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUserById()
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == "uid")?.Value;
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
            return NotFound(new { message = "User not found" });
        
        return Ok(user);
    }

    [Authorize]
    [HttpPost("update-profile")]
    public async Task<IActionResult> UpdateUserProfile(UpdateUserDto model)
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == "uid")?.Value;
        if (userId == null)
            return Unauthorized();

        var result = await _userService.UpdateUserAsync(userId, model);
        if (!result)
            return BadRequest("Failed to update profile");

        return Ok("Profile updated successfully");
    }

    [Authorize]
    [HttpPost("upload-image")]
    public async Task<ActionResult> AddImagesForProduct([FromForm] IFormFile file)
    {
        var userID = GetAuthorizedUserId();
        
        if (file == null )
        {
            return BadRequest("File is required.");
        }
        var photo = await _photoService.UploadUserPhoto(file, userID);
        return Ok(photo);
    }
    
    [Authorize]
    [HttpGet("secured")]
    public async Task<IActionResult> GetSecuredData()
    {
        return Ok("This Secured Data is available only for Authenticated Users.");
    }
    
    [HttpPost("register")]
    public async Task<ActionResult> RegisterAsync(RegisterModel model)
    {
        var result = await _userService.RegisterAsync(model);
        
        return Ok(result);
    }
    
    [HttpPost("token")]
    public async Task<IActionResult> GetTokenAsync(TokenRequestModel model)
    {
        var result = await _userService.GetTokenAsync(model);
        SetRefreshTokenInCookie(result.RefreshToken);
        return Ok(result);
    }
    
    [HttpPost("addrole")]
    public async Task<IActionResult> AddRoleAsync(AddRoleModel model)
    {
        var result = await _userService.AddRoleAsync(model);
        return Ok(result);
    }
    
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var response = await _userService.RefreshTokenAsync(refreshToken);
        if (!string.IsNullOrEmpty(response.RefreshToken))
            SetRefreshTokenInCookie(response.RefreshToken);
        return Ok(response);
    }
    
    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
    {
        // accept token from request body or cookie
        var token = model.Token ?? Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(token))
            return BadRequest(new { message = "Token is required" });

        var response = _userService.RevokeToken(token);

        if (!response)
            return NotFound(new { message = "Token not found" });

        return Ok(new { message = "Token revoked" });
    }
    
    
    
    
    private void SetRefreshTokenInCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions()
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(10),
        };
        
        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
    
    private string GetAuthorizedUserId()
    {
        return User.Claims.FirstOrDefault(x => x.Type == "uid")?.Value;
    }
}