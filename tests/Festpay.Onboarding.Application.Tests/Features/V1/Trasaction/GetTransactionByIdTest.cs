using Festpay.Onboarding.Application.Features.V1.Transaction;
using Festpay.Onboarding.Domain.Entities;
using Festpay.Onboarding.Infra.Context;
using Microsoft.EntityFrameworkCore;

public class GetTransactionByIdTest
{
    private static FestpayContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<FestpayContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new FestpayContext(options);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenTransactionDoesNotExist()
    {
        await using var context = CreateContext();

        var handler = new GetTransactionById.Handler(context);

        var query = new GetTransactionById.Query(Guid.NewGuid());

        var result = await handler.Handle(
            query,
            CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_ShouldReturnTransaction_WhenTransactionExists()
    {
        // Arrange
        await using var context = CreateContext();

        var originAccountId = Guid.NewGuid();
        var destinationAccountId = Guid.NewGuid();
        var amount = 250.75m;

        var transaction = new Transaction.Builder()
            .WithOriginAccountId(originAccountId)
            .WithDestinationAccountId(destinationAccountId)
            .WithAmount(amount)
            .Build();

        context.Transactions.Add(transaction);
        await context.SaveChangesAsync();

        var handler = new GetTransactionById.Handler(context);

        var query = new GetTransactionById.Query(transaction.Id);

        // Act
        var result = await handler.Handle(
            query,
            CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(transaction.Id, result.Id);
        Assert.Equal(transaction.OriginAccountId, result.OriginAccountId);
        Assert.Equal(transaction.DestinationAccountId, result.DestinationAccountId);
        Assert.Equal(transaction.Amount, result.Amount);
        Assert.Equal(transaction.Cancelled, result.Cancelled);
    }
}