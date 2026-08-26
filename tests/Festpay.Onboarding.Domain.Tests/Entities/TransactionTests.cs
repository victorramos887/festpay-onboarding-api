using Festpay.Onboarding.Domain.Entities;


namespace Festpay.Onboarding.Domain.Tests.Entities;

public class TransactionTests
{
    [Fact]
    public void Should_Create_Transaction_When_Data_Is_Valid()
    {
        var originAccountId = Guid.NewGuid();
        var destinationAccountId = Guid.NewGuid();
        var amount = 100.00m;

        var transaction = new Transaction.Builder()
            .WithOriginAccountId(originAccountId)
            .WithDestinationAccountId(destinationAccountId)
            .WithAmount(amount)
            .Build();

        Assert.Equal(originAccountId, transaction.OriginAccountId);
        Assert.Equal(destinationAccountId, transaction.DestinationAccountId);
        Assert.Equal(amount, transaction.Amount);
        Assert.False(transaction.Cancelled);
    }

    [Fact]
    public void Should_Cancel_Transaction_When_Cancel_Is_Called()
    {
        var transaction = new Transaction.Builder()
            .WithOriginAccountId(Guid.NewGuid())
            .WithDestinationAccountId(Guid.NewGuid())
            .WithAmount(100.00m)
            .Build();

        transaction.Cancel();

        Assert.True(transaction.Cancelled);
    }

    [Fact]
    public void Should_Throw_When_Origin_And_Destination_Are_Equal()
    {
        var accountId = Guid.NewGuid();

        var exception = Assert.Throws<ArgumentException>(() =>
            new Transaction.Builder()
                .WithOriginAccountId(accountId)
                .WithDestinationAccountId(accountId)
                .WithAmount(100.00m)
                .Build());

        Assert.Equal("Origin and destination account IDs cannot be the same.", exception.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_Throw_When_Amount_Is_Invalid(decimal amount)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new Transaction.Builder()
                .WithOriginAccountId(Guid.NewGuid())
                .WithDestinationAccountId(Guid.NewGuid())
                .WithAmount(amount)
                .Build());

        Assert.Equal("Amount must be greater than zero.", exception.Message);
    }
}