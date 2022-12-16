using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Interfaces;

namespace Template.Application.DefaultCategories.Commands.CreateDefaultCategory;

public class CreateDefaultCategoryCommandValidator : AbstractValidator<CreateDefaultCategoryCommand>
{
	private readonly IApplicationDbContext _context;

	public CreateDefaultCategoryCommandValidator(IApplicationDbContext context)
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
