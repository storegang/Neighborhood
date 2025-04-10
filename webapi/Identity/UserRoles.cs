using Microsoft.AspNetCore.Identity;

namespace webapi.Identity;

public static class UserRoles
{
    public const string BoardMember = "BoardMember";
    public const string Tenant = "Tenant";

    public static readonly IdentityRole[] IdentityRoleArray =
    [
        new(BoardMember),
        new(Tenant),
    ];
}
