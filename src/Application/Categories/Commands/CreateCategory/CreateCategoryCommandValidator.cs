using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Interfaces;

namespace Template.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
	private readonly IApplicationDbContext _context;

	public CreateCategoryCommandValidator(IApplicationDbContext context)
	{
		_context = context;

		RuleFor(v => v.Name)
			.NotEmpty().WithMessage("Name is required.")
			.MustAsync(BeUniqueTitle!).WithMessage("The specified name already exists.");
	}

	public async Task<bool> BeUniqueTitle(string name, CancellationToken cancellationToken)
	{
		return await _context.Categories
			.AllAsync(l => l.Name != name, cancellationToken);
	}
}
