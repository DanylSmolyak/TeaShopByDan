using Core.Dtos;

namespace Core.Interfaces;

public interface IUserService
{
    Task<string> RegisterAsync(RegisterModel model);
    Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model);
    
    Task<string> AddRoleAsync(AddRoleModel model);
    
    Task<AuthenticationModel> RefreshTokenAsync(string token);
    
    bool RevokeToken(string token);
    
    Task<UserDto> GetUserByIdAsync(string userId);
    Task<bool> UpdateUserAsync(string userId, UpdateUserDto model); 
    
}