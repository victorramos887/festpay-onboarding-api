using Carter;
using FluentValidation;
using MediatR;
using Festpay.Onboarding.Application.Common.Constants;
using Festpay.Onboarding.Application.Common.Exceptions;
using Festpay.Onboarding.Application.Common.Models;
using Festpay.Onboarding.Domain.Entities;
using Festpay.Onboarding.Infra.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Festpay.Onboarding.Application.Features.V1;

public sealed record CreateTransactionCommand(
    Guid OriginAccountId,
    Guid DestinationAccountId,
    decimal Amount
) : IRequest<Guid>;

public sealed class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.OriginAccountId).NotEmpty().WithMessage("Origin account ID is required.");
        RuleFor(x => x.DestinationAccountId).NotEmpty().WithMessage("Destination account ID is required.");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero.");
    }
}

public sealed class CreateTransactionCommandHandler(FestpayContext dbContext)
    : IRequestHandler<CreateTransactionCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateTransactionCommand request,
        CancellationToken cancellationToken
    )
    {
        var originExists = await dbContext.Accounts
            .AnyAsync(x => x.Id == request.OriginAccountId, cancellationToken);

        if (!originExists)
            throw new NotFoundException("Origin account");

        var destinationExists = await dbContext.Accounts
            .AnyAsync(x => x.Id == request.DestinationAccountId, cancellationToken);

        if (!destinationExists)
            throw new NotFoundException("Destination account");

        var transaction = new Transaction.Builder()
            .WithOriginAccountId(request.OriginAccountId)
            .WithDestinationAccountId(request.DestinationAccountId)
            .WithAmount(request.Amount)
            .Build();

        await dbContext.Transactions.AddAsync(transaction, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return transaction.Id;
    }
}

public sealed class CreateTransactionCommandEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost($"{EndpointConstants.V1}{EndpointConstants.Transaction}",
            async ([FromServices] ISender sender, [FromBody] CreateTransactionCommand command) =>
            {
                var result = await sender.Send(command);
                return Result.Ok(result);
            }
        )
        .WithTags(SwaggerTagsConstants.Transaction);
    }
}