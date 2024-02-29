using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

/// <summary>
/// user entity
/// "Id" -  Id of user
/// "Password" - user's password
/// "UserRole" - role of user in project (admin/engineer)
/// </summary>
public class User
{
    public int Id { get; set; } 
    public string? Password { get; set; }
    public UserRole Role { get; set; }
    public override string ToString() => Tools.ToStringProperties(this);
}
