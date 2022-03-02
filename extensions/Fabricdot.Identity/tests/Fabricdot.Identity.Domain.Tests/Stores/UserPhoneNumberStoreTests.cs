using System.Threading.Tasks;
using Xunit;

namespace Fabricdot.Identity.Domain.Tests.Stores
{
    public class UserPhoneNumberStoreTests : UserStoreTestsBase
    {
        [Fact]
        public async Task GetPhoneNumberAsync_ReturnCorrectly()
        {
            var user = EntityBuilder.NewUserWithPhoneNumber();
            var phoneNumber = await UserStore.GetPhoneNumberAsync(user, default);

            Assert.Equal(user.PhoneNumber, phoneNumber);
        }

        [InlineData(true)]
        [InlineData(false)]
        [Theory]
        public async Task GetPhoneNumberConfirmedAsync_ReturnCorrectly(bool isConfirmed)
        {
            var user = EntityBuilder.NewUserWithPhoneNumber(phoneNumberConfirmed: isConfirmed);
            var phoneNumberConfirmed = await UserStore.GetPhoneNumberConfirmedAsync(user, default);

            Assert.Equal(isConfirmed, phoneNumberConfirmed);
        }

        [InlineData(null)]
        [InlineData("10000000000")]
        [InlineData(" 10000000000 ")]
        [Theory]
        public async Task SetPhoneNumberAsync_GivenInput_Correctly(string phoneNumber)
        {
            var user = EntityBuilder.NewUserWithPhoneNumber();
            var confirmed = user.PhoneNumberConfirmed;
            await UserStore.SetPhoneNumberAsync(user, phoneNumber, default);

            Assert.Equal(phoneNumber?.Trim(), user.PhoneNumber);
            Assert.Equal(confirmed, user.PhoneNumberConfirmed);
        }

        [InlineData(true)]
        [InlineData(false)]
        [Theory]
        public async Task SetPhoneNumberConfirmedAsync_GivenInput_Correctly(bool isConfirmed)
        {
            var user = EntityBuilder.NewUserWithPhoneNumber();
            await UserStore.SetPhoneNumberConfirmedAsync(user, isConfirmed, default);

            Assert.Equal(isConfirmed, user.PhoneNumberConfirmed);
        }
    }
}