using System;
using System.Collections.Generic;

namespace footic.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Upassword { get; set; } = null!;

    public string UserType { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int? TeamId { get; set; }

    public virtual Team? Team { get; set; }
}
