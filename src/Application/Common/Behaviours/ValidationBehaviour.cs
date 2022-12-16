using FluentValidation;
using MediatR;
using ValidationException = Template.Application.Common.Exceptions.ValidationException;

namespace Template.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	 where TRequest : notnull
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
	{
		_validators = validators;
	}

	public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
	{
		if (_validators.Any())
		{
			var context = new ValidationContext<TRequest>(request);

			var validationResults = await Task.WhenAll(
				_validators.Select(validator =>
					validator.ValidateAsync(context, cancellationToken)));

			var failures = validationResults
				.Where(validationResult => validationResult.Errors.Any())
				.SelectMany(validationResult => validationResult.Errors)
				.ToList();

			if (failures.Any())
				throw new ValidationException(failures);
		}
		return await next();
	}
}
