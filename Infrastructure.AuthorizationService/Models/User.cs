using System;
using System.Collections.Generic;

namespace Infrastructure.AuthorizationService.Models;

public partial class User
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool EmailConfirmed { get; set; }

    public string PasswordResetCode { get; set; } = null!;

    public short FailedLoginCount { get; set; }

    public DateTime? DisabledBefore { get; set; }
}
