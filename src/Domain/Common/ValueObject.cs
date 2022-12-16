namespace Template.Domain.Common;

// Learn more: https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/implement-value-objects
public abstract class ValueObject
{
	protected static bool EqualOperator(ValueObject? left, ValueObject? right) =>
		!(left is null ^ right is null) && left?.Equals(right!) != false;

	protected static bool NotEqualOperator(ValueObject? left, ValueObject? right) => !EqualOperator(left, right);

	protected abstract IEnumerable<object?> GetEqualityComponents();

	public override bool Equals(object? obj) =>
		obj != null && obj.GetType() == GetType() && GetEqualityComponents().SequenceEqual(((ValueObject)obj).GetEqualityComponents());

	public override int GetHashCode() =>
		GetEqualityComponents()
			.Select(obj => obj != null ? obj.GetHashCode() : 0)
			.Aggregate((x, y) => x ^ y);
}
