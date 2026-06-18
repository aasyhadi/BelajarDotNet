namespace BelajarApi.Dtos;

public class CreateUserDto
{
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string Role { get; set; } = "User";
}

public class UpdateUserRoleDto
{
    public string Role { get; set; } = "User";
}

public class ResetPasswordDto
{
    public string NewPassword { get; set; } = "";
}