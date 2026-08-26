using Festpay.Onboarding.Infra.Context;
using Microsoft.EntityFrameworkCore;

public static class CancelTransaction
{
    public static async Task<bool> Handle(
        FestpayContext dbContext,
        Guid transactionId,
        CancellationToken cancellationToken
    )
    {
        var transaction = await dbContext.Transactions
            .FirstOrDefaultAsync(t => t.Id == transactionId, cancellationToken);

        if (transaction == null)
            return false;

        transaction.Cancel();
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}