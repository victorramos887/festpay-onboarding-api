using Carter;
using Festpay.Onboarding.Application.Common.Constants;
using Festpay.Onboarding.Infra.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Festpay.Onboarding.Application.Features.V1.Transaction;

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

public sealed class CancelTransactionCommandEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(
            $"{EndpointConstants.V1}{EndpointConstants.Transaction}/{{transactionId:guid}}",
            async (
                Guid transactionId,
                FestpayContext dbContext,
                CancellationToken cancellationToken) =>
            {
                var result = await CancelTransaction.Handle(
                    dbContext,
                    transactionId,
                    cancellationToken);

                return result ? Results.NoContent() : Results.NotFound();
            }
        )
        .WithTags(SwaggerTagsConstants.Transaction);
    }
}