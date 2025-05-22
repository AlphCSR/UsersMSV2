using System;
using UsersMS.Application.Queries;
using UsersMS.Commons.Dtos.Response;
using Xunit;

public class GetUserQueryTest
{
    [Fact]
    public void Default_Constructor_Should_Set_Defaults()
    {
        var query = new GetUserQuery();
        Assert.Equal(Guid.Empty, query.UserId);
    }

    [Fact]
    public void Constructor_With_UserId_Should_Set_UserId()
    {
        var userId = Guid.NewGuid();
        var query = new GetUserQuery(userId);
        Assert.Equal(userId, query.UserId);
    }
}
