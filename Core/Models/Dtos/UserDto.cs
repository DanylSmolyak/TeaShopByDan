namespace Core.Dtos;

public class UserDto
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Image { get; set; }
    public string Address { get; set; }
}

public class UpdateUserDto
{
    public string Username { get; set; }
    public string Address { get; set; }
}