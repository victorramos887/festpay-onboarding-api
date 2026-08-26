using Festpay.Onboarding.Application.Features.V1.Transaction;
using Festpay.Onboarding.Domain.Entities;
using Festpay.Onboarding.Infra.Context;
using Microsoft.EntityFrameworkCore;
using DomainTransaction = Festpay.Onboarding.Domain.Entities.Transaction;

namespace Festpay.Onboarding.Application.Tests.Features.V1.Transaction;

public class CancelTransactionTest
{
    private static FestpayContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<FestpayContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new FestpayContext(options);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenTransactionDoesNotExist()
    {
        await using var context = CreateContext();

        var result = await CancelTransaction.Handle(
            context,
            Guid.NewGuid(),
            CancellationToken.None);

        Assert.False(result);
    }

    [Fact]
    public async Task Handle_ShouldCancelTransaction_WhenTransactionExists()
    {
        await using var context = CreateContext();

        var transaction = new DomainTransaction.Builder()
            .WithOriginAccountId(Guid.NewGuid())
            .WithDestinationAccountId(Guid.NewGuid())
            .WithAmount(100.00m)
            .Build();

        context.Transactions.Add(transaction);
        await context.SaveChangesAsync();

        var result = await CancelTransaction.Handle(
            context,
            transaction.Id,
            CancellationToken.None);

        var updatedTransaction = await context.Transactions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == transaction.Id);

        Assert.True(result);
        Assert.NotNull(updatedTransaction);
        Assert.True(updatedTransaction!.Cancelled);
    }
}
