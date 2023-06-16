using System.Data;
using Fabricdot.Infrastructure.Data;
using Moq;
using Moq.Protected;

namespace Fabricdot.Infrastructure.Tests.Data;

public class SqlConnectionFactoryTests : TestFor<Mock<SqlConnectionFactory>>
{
    [Fact]
    public void GetOpenConnection_WithoutConnection_ReturnNewConnection()
    {
        var connectionMock = InjectMock<IDbConnection>();
        SetupCreateConnection(() => connectionMock.Object);

        var connection = Sut.Object.GetOpenConnection();

        connection.Should().BeSameAs(connectionMock.Object);
        connectionMock.Verify(v => v.Open(), Times.Once());
    }

    [Fact]
    public void GetOpenConnection_WithOpenningConnection_ReturnExistsConnetion()
    {
        SetupCreateConnection(() =>
        {
            var connectionMock = Mock<IDbConnection>();
            connectionMock.SetupGet(v => v.State).Returns(ConnectionState.Open);
            return connectionMock.Object;
        });

        var connection1 = Sut.Object.GetOpenConnection();
        var connection2 = Sut.Object.GetOpenConnection();

        connection1.Should().BeSameAs(connection2);
    }

    [Fact]
    public void Dispose_Should_ReleaseConnection()
    {
        var connectionMock = InjectMock<IDbConnection>();
        connectionMock.SetupGet(v => v.State).Returns(ConnectionState.Open);
        SetupCreateConnection(() => connectionMock.Object);

        var connection = Sut.Object.GetOpenConnection();
        Sut.Object.Dispose();
        connectionMock.Verify(v => v.Dispose(), Times.Once());
    }

    private void SetupCreateConnection(Func<IDbConnection> func)
    {
        Sut.Protected()
           .Setup<IDbConnection>("CreateConnection", ItExpr.IsAny<string>())
           .Returns((string _) => func());
    }
}
