namespace DO;

/// <summary>
/// User entity represents a user with all its props
/// </summary>
/// <param name="Id"> id of user </param>
/// <param name="Password"> password </param>
/// <param name="Role"> user's role - admin/engineer </param>
public record User
(
    int Id,
    string Password,
    UserRole Role
)
{
   public User() : this(0, "", UserRole.Engineer) { }
}
