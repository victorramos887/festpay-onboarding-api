using Carter;
using Festpay.Onboarding.Application.Common.Constants;
using Festpay.Onboarding.Infra.Context;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Festpay.Onboarding.Application.Features.V1.Transaction;

public static class GetTransactionById
{
    public sealed record Query(Guid Id) : IRequest<Response?>;

    public sealed record Response(
        Guid Id,
        Guid OriginAccountId,
        Guid DestinationAccountId,
        decimal Amount,
        bool Cancelled
    );

    public sealed class Handler(FestpayContext dbContext)
        : IRequestHandler<Query, Response?>
    {
        public async Task<Response?> Handle(
            Query request,
            CancellationToken cancellationToken)
        {
            return await dbContext.Transactions
                .AsNoTracking()
                .Where(transaction => transaction.Id == request.Id)
                .Select(transaction => new Response(
                    transaction.Id,
                    transaction.OriginAccountId,
                    transaction.DestinationAccountId,
                    transaction.Amount,
                    transaction.Cancelled
                ))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }

    public sealed class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(
                $"{EndpointConstants.V1}{EndpointConstants.Transaction}/{{id:guid}}",
                async (Guid id, ISender sender, CancellationToken cancellationToken) =>
                {
                    var response = await sender.Send(
                        new Query(id),
                        cancellationToken);

                    return response is null ? Results.NotFound() : Results.Ok(response);
                })
                .WithTags(SwaggerTagsConstants.Transaction);
        }
    }
}