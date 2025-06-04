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

    public string? ResetCode { get; set; }

    public DateTime? CodeRequestedAt { get; set; }

    public DateTime? BannedBefore { get; set; }
}
