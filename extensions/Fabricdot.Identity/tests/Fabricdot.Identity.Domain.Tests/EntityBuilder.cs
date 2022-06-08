using System;
using Fabricdot.Identity.Domain.Entities.UserAggregate;

namespace Fabricdot.Identity.Domain.Tests;

public static class EntityBuilder
{
    public static IdentityUser NewUser(
        string userName = "name1",
        bool lockoutEnabled = true)
    {
        return new IdentityUser(Guid.NewGuid(), userName)
        {
            LockoutEnabled = lockoutEnabled
        };
    }

    public static IdentityUser NewUserWithPassword(
        string userName = "name1",
        string passwordHash = "PasswordHash")
    {
        return new IdentityUser(Guid.NewGuid(), userName)
        {
            PasswordHash = passwordHash
        };
    }

    public static IdentityUser NewUserWithEmail(
        string userName = "name1",
        string email = "qwe@banana.com",
        bool emailConfirmed = false)
    {
        return new IdentityUser(Guid.NewGuid(), userName, email)
        {
            EmailConfirmed = emailConfirmed
        };
    }

    public static IdentityUser NewUserWithPhoneNumber(
        string userName = "name1",
        string phoneNumber = "10000000000",
        bool phoneNumberConfirmed = true)
    {
        var user = new IdentityUser(Guid.NewGuid(), userName);
        user.ChangePhoneNumber(phoneNumber, phoneNumberConfirmed);
        return user;
    }
}