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

        var transaction = new Transaction(
            // mesmos parâmetros que você usa no CreateTransactionTest
        );

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
    }
}