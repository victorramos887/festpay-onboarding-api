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
}