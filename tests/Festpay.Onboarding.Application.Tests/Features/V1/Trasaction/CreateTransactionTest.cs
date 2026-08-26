using Festpay.Onboarding.Application.Common.Exceptions;
using Festpay.Onboarding.Application.Features.V1;
using Festpay.Onboarding.Domain.Entities;
using Festpay.Onboarding.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Festpay.Onboarding.Application.Tests.Features.V1.Trasaction;

public class CreateTransactionTest
{
	private static FestpayContext CreateContext()
	{
		var options = new DbContextOptionsBuilder<FestpayContext>()
			.UseInMemoryDatabase(Guid.NewGuid().ToString())
			.Options;

		return new FestpayContext(options);
	}

	private static Account CreateAccount()
	{
		return new Account.Builder()
			.WithName("Test Account")
			.WithEmail("test@example.com")
			.WithPhone("11999999999")
			.WithDocument("12345678909")
			.Build();
	}

	[Fact]
	public async Task Handle_ShouldCreateTransaction_WhenAccountsExist()
	{
		await using var context = CreateContext();

		var originAccount = CreateAccount();
		var destinationAccount = CreateAccount();

		context.Accounts.Add(originAccount);
		context.Accounts.Add(destinationAccount);
		await context.SaveChangesAsync();

		var command = new CreateTransactionCommand(
			originAccount.Id,
			destinationAccount.Id,
			150.75m);

		var handler = new CreateTransactionCommandHandler(context);

		var result = await handler.Handle(command, CancellationToken.None);

		var transaction = await context.Transactions
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == result);

		Assert.NotEqual(Guid.Empty, result);
		Assert.NotNull(transaction);
		Assert.Equal(originAccount.Id, transaction!.OriginAccountId);
		Assert.Equal(destinationAccount.Id, transaction.DestinationAccountId);
		Assert.Equal(150.75m, transaction.Amount);
		Assert.False(transaction.Cancelled);
	}

	[Fact]
	public async Task Handle_ShouldThrowNotFound_WhenOriginAccountDoesNotExist()
	{
		await using var context = CreateContext();

		var destinationAccount = CreateAccount();
		context.Accounts.Add(destinationAccount);
		await context.SaveChangesAsync();

		var command = new CreateTransactionCommand(
			Guid.NewGuid(),
			destinationAccount.Id,
			150.75m);

		var handler = new CreateTransactionCommandHandler(context);

		var exception = await Assert.ThrowsAsync<NotFoundException>(
			() => handler.Handle(command, CancellationToken.None));

		Assert.Equal("Origin account not found", exception.Message);
	}
}
