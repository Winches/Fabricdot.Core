using Fabricdot.Core.Security;

namespace Fabricdot.Core.Tests.Security;

public class HashHelprTests : TestBase
{
    [InlineData("", "d41d8cd98f00b204e9800998ecf8427e")]
    [InlineData("a", "0cc175b9c0f1b6a831c399e269772661")]
    [InlineData("message digest", "f96b697d7cb7938d525a2f31aaf161d0")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "c3fcd3d76192e4007dfb496cca67e13b")]
    [Theory]
    public void ToMd5_GivenInput_ReturnHashedString(
        string text,
        string hashedString)
    {
        HashHelper.ToMd5(text).Should().Be(hashedString.ToUpperInvariant());
    }

    [InlineData("", "da39a3ee5e6b4b0d3255bfef95601890afd80709")]
    [InlineData("a", "86f7e437faa5a7fce15d1ddcb9eaeaea377667b8")]
    [InlineData("message digest", "c12252ceda8be8994d5fa0290a47231c1d16aae3")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "32d10c7b8cf96570ca04ce37f2a19d84240d3a89")]
    [Theory]
    public void ToSha1_GivenInput_ReturnHashedString(
        string text,
        string hashedString)
    {
        HashHelper.ToSha1(text).Should().Be(hashedString.ToUpperInvariant());
    }

    [InlineData("", "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855")]
    [InlineData("a", "ca978112ca1bbdcafac231b39a23dc4da786eff8147c4e72b9807785afee48bb")]
    [InlineData("message digest", "f7846f55cf23e14eebeab5b4e1550cad5b509e3348fbc4efa3a1413d393cb650")]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "71c480df93d6ae2f1efad1447c66c9525e316218cf51fc8d9ed832f2daf18b73")]
    [Theory]
    public void ToSha256_GivenInput_ReturnHashedString(
        string text,
        string hashedString)
    {
        HashHelper.ToSha256(text).Should().Be(hashedString.ToUpperInvariant());
    }
}
