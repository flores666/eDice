using AccountService.Models;
using Infrastructure.AccountService.Models;

namespace AccountService.Helpers;

public static class UsersMapper
{
    public static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Description = user.Description,
            Email = user.Email,
            Id = user.Id,
            CreatedAt = user.CreatedAt,
            Name = user.Name,
            ProfilePicture = user.ProfilePicture,
            BannedBefore = user.BannedBefore,
            ProfilePicturePreview = user.ProfilePicturePreview
        };
    }
}
