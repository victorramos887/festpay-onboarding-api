using Carter;
using Festpay.Onboarding.Infra.Context;
using MediatR;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

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
            // endpoint aqui
        }
    }
}