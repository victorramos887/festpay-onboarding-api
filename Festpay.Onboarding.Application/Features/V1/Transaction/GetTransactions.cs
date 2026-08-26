using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Festpay.Onboarding.Infra.Context;
using Festpay.Onboarding.Application.Common.Constants;

namespace Festpay.Onboarding.Application.Features.V1.Transaction;

public static class GetTransactions
{
    public sealed record Query : IRequest<List<Response>>;

    public sealed record Response(
        Guid Id,
        Guid OriginAccountId,
        Guid DestinationAccountId,
        decimal Amount,
        bool Cancelled
    );

    public sealed class Handler(FestpayContext dbContext)
        : IRequestHandler<Query, List<Response>>
    {
        public async Task<List<Response>> Handle(
            Query request,
            CancellationToken cancellationToken)
        {
            return await dbContext.Transactions
                .AsNoTracking()
                .Select(transaction => new Response(
                    transaction.Id,
                    transaction.OriginAccountId,
                    transaction.DestinationAccountId,
                    transaction.Amount,
                    transaction.Cancelled
                ))
                .ToListAsync(cancellationToken);
        }
    }
}

public sealed class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            $"{EndpointConstants.V1}{EndpointConstants.Transaction}",
            async (ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(
                    new GetTransactions.Query(),
                    cancellationToken
                );

                return Results.Ok(response);
            })
            .WithTags(SwaggerTagsConstants.Transaction);
    }
}