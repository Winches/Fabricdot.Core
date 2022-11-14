using System.Security.Claims;

namespace Fabricdot.Core.Security;

public static class SharedClaimTypes
{
    public static string NameIdentifier { get; set; } = ClaimTypes.NameIdentifier;

    public static string Name { get; set; } = ClaimTypes.Name;

    public static string GivenName { get; set; } = ClaimTypes.GivenName;

    public static string Surname { get; set; } = ClaimTypes.Surname;

    public static string Email { get; set; } = ClaimTypes.Email;

    public static string MobilePhone { get; set; } = ClaimTypes.MobilePhone;

    public static string Role { get; set; } = ClaimTypes.Role;
}