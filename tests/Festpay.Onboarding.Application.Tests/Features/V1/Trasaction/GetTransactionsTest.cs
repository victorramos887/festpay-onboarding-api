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

        var transaction = new Transaction(
            // use os mesmos parâmetros do teste anterior
        );

        context.Transactions.Add(transaction);
        await context.SaveChangesAsync();

        var handler = new GetTransactions.Handler(context);

        var result = await handler.Handle(
            new GetTransactions.Query(),
            CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(transaction.Id, result[0].Id);
    }
}