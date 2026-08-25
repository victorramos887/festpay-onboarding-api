namespace Festpay.Onboarding.Domain.Entities;


public class Transaction : EntityBase
{
    public Guid OriginAccountId { get; private set; }
    public Guid DestinationAccountId { get; private set; }
    public decimal Amount { get; private set; }
    public bool Cancelled { get; private set; }

    public override void Validate()
    {
        if(OriginAccountId == Guid.Empty)
            throw new ArgumentException("Origin account ID cannot be empty.");
        if(DestinationAccountId == Guid.Empty)
            throw new ArgumentException("Destination account ID cannot be empty.");
        if(Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.");
        if(OriginAccountId == DestinationAccountId)
            throw new ArgumentException("Origin and destination account IDs cannot be the same.");
    }

    public void Cancel()
    {
        Cancelled = true;
    }
    public class Builder
    {
        private readonly Transaction _transaction = new();

        public Builder WithOriginAccountId(Guid originAccountId)
        {
            _transaction.OriginAccountId = originAccountId;
            return this;
        }

        public Builder WithDestinationAccountId(Guid destinationAccountId)
        {
            _transaction.DestinationAccountId = destinationAccountId;
            return this;
        }

        public Builder WithAmount(decimal amount)
        {
            _transaction.Amount = amount;
            return this;
        }

        public Transaction Build()
        {
            _transaction.Validate();
            return _transaction;
        }
    }
}