using System.Threading.Tasks;
using Xunit;

namespace Fabricdot.Identity.Domain.Tests.Stores
{
    public class UserEmailStoreTests : UserStoreTestsBase
    {
        [Fact]
        public async Task GetEmailAsync_ReturnCorrectly()
        {
            var user = EntityBuilder.NewUserWithEmail();
            var email = await UserStore.GetEmailAsync(user, default);
            Assert.Equal(user.Email, email);
        }

        [Fact]
        public async Task GetNormalizedEmailAsync_ReturnCorrectly()
        {
            var user = EntityBuilder.NewUserWithEmail();
            var normalizedEmail = await UserStore.GetNormalizedEmailAsync(user, default);
            Assert.Equal(user.NormalizedEmail, normalizedEmail);
        }

        [InlineData(true)]
        [InlineData(false)]
        [Theory]
        public async Task GetEmailConfirmedAsync_ReturnCorrectly(bool isConfirmed)
        {
            var user = EntityBuilder.NewUserWithEmail(emailConfirmed: isConfirmed);
            var emailConfirmed = await UserStore.GetEmailConfirmedAsync(user, default);
            Assert.Equal(isConfirmed, emailConfirmed);
        }

        [InlineData(null)]
        [InlineData("qwe@banana.com")]
        [Theory]
        public async Task SetEmailAsync_GiveInput_Correctly(string email)
        {
            var user = EntityBuilder.NewUser();
            await UserStore.SetEmailAsync(user, email, default);
            Assert.Equal(email, user.Email);
        }

        [InlineData(null)]
        [InlineData("QWE@BANANA.COM")]
        [Theory]
        public async Task SetNormalizedEmailAsync_GiveInput_Correctly(string normalizedEmail)
        {
            var user = EntityBuilder.NewUser();
            await UserStore.SetNormalizedEmailAsync(user, normalizedEmail, default);
            Assert.Equal(normalizedEmail, user.NormalizedEmail);
        }

        [InlineData(true)]
        [InlineData(false)]
        [Theory]
        public async Task SetEmailConfirmedAsync_GivenIpur_Correctly(bool isConfirmed)
        {
            var user = EntityBuilder.NewUser();
            await UserStore.SetEmailConfirmedAsync(user, isConfirmed, default);
            Assert.Equal(isConfirmed, user.EmailConfirmed);
        }
    }
}