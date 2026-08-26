using Festpay.Onboarding.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Festpay.Onboarding.Application.Features.V1.Transaction;
using Festpay.Onboarding.Domain.Entities;

public class GetTransactionsTest
{
    private static FestpayContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<FestpayContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new FestpayContext(options);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoTransactionsExist()
    {
        await using var context = CreateContext();

        var handler = new GetTransactions.Handler(context);

        var result = await handler.Handle(
            new GetTransactions.Query(),
            CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_ShouldReturnTransactions_WhenTransactionsExist()
    {
        await using var context = CreateContext();

        var originAccountId = Guid.NewGuid();
        var destinationAccountId = Guid.NewGuid();
        var amount = 100.00m;

        var transaction = new Transaction.Builder()
            .WithOriginAccountId(originAccountId)
            .WithDestinationAccountId(destinationAccountId)
            .WithAmount(amount)
            .Build();

        context.Transactions.Add(transaction);
        await context.SaveChangesAsync();

        var handler = new GetTransactions.Handler(context);

        var result = await handler.Handle(
            new GetTransactions.Query(),
            CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(transaction.Id, result[0].Id);
        Assert.Equal(transaction.OriginAccountId, result[0].OriginAccountId);
        Assert.Equal(transaction.DestinationAccountId, result[0].DestinationAccountId);
        Assert.Equal(transaction.Amount, result[0].Amount);
        Assert.Equal(transaction.Cancelled, result[0].Cancelled);
    }
}