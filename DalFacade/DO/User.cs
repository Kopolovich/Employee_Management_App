namespace DO;

public record User
(
    int Id,
    string Password,
    UserRole Role
)
{
   public User() : this(0, "", UserRole.Engineer) { }
}
