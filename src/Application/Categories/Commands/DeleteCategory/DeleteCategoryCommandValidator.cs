using FluentValidation;
using Template.Application.Categories.Commands.DeleteCategories;
using Template.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Template.Application.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
	private readonly IApplicationDbContext _context;

	public DeleteCategoryCommandValidator(IApplicationDbContext context)
	{
		_context = context;

		RuleFor(v => v.Id)
			.NotEmpty().WithMessage("Id must not be empty.")
			.MustAsync(HasNoDefault).WithMessage("Category is a substitute category for a default category, delete default category to delete this one, or simply set this one as hidden.")
			.MustAsync(HasNoChildren).WithMessage("Category still has direct children.")
			.MustAsync(HasNoContent).WithMessage("Category still has content attached.");
	}

	public async Task<bool> HasNoDefault(Guid id, CancellationToken cancellationToken)
	{
		return await _context.Categories
			.Where(c => c.Id.Equals(id))
			.AllAsync(c => c.DefaultValue == null, cancellationToken);
	}

	public async Task<bool> HasNoChildren(Guid id, CancellationToken cancellationToken)
	{
		return await _context.Categories
			.Where(c => c.Id.Equals(id))
			.AllAsync(c => !c.ChildCategories.Any(), cancellationToken);
	}

	public async Task<bool> HasNoContent(Guid id, CancellationToken cancellationToken)
	{
		return await _context.Contents
			.AllAsync(c => !c.Categories.Any(c => c.Id.Equals(id)), cancellationToken);
	}
}
