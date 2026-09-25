using TicketHub.Domain.Errors;
using TicketHub.Domain.ValueObjects;

namespace TicketHub.Domain.Tests;

public sealed class EmailAddressTests
{
    [Fact]
    public void Create_NormalizesCaseAndWhitespace()
    {
        var result = EmailAddress.Create("  Fan@Example.COM ");

        Assert.Equal("fan@example.com", result.Value!.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("not-an-email")]
    [InlineData("@example.com")]
    [InlineData("fan@")]
    [InlineData("fan @example.com")]
    public void Create_WithMalformedAddress_Fails(string input)
    {
        var result = EmailAddress.Create(input);

        Assert.Equal(DomainErrors.Email.InvalidFormat, result.Error);
    }
}
