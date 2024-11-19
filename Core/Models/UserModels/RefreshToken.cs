using Microsoft.EntityFrameworkCore;

namespace Core.Dtos;

[Owned]
public class RefreshToken
{
    public string Token { get; set; }
    public DateTime ExpireData { get; set; }
    public bool IsExpired => DateTime.UtcNow >= ExpireData;
    public DateTime Created { get; set; }
    public DateTime? Revoked { get; set; }
    public bool IsActive => Revoked == null && !IsExpired;

}