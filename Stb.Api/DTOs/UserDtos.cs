namespace Stb.Api.DTOs;

public record UserDto(int Id, string Username, string Userfirstname, string Email, string Mobile, string Role);

public class CreateUserDto
{
    public string Username { get; set; } = null!;
    public string Userfirstname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Mobile { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = "User";
}

public class UpdateUserDto
{
    public string Username { get; set; } = null!;
    public string Userfirstname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Mobile { get; set; } = null!;
    public string? Password { get; set; }
    public string Role { get; set; } = "User";
}

