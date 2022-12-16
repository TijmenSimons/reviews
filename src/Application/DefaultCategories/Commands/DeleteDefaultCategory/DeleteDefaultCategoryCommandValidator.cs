using FluentValidation;
using Template.Application.DefaultCategories.Commands.DeleteDefaultCategories;
using Template.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Template.Application.DefaultCategories.Commands.DeleteDefaultCategory;

public class DeleteDefaultCategoryCommandValidator : AbstractValidator<DeleteDefaultCategoryCommand>
{
	private readonly IApplicationDbContext _context;

	public DeleteDefaultCategoryCommandValidator(IApplicationDbContext context)
	{
		_context = context;

		RuleFor(v => v.Id)
			.NotEmpty().WithMessage("Id must not be empty.")
			.MustAsync(HasNoChildren).WithMessage("DefaultCategory still has children somewhere.")
			.MustAsync(HasNoContent).WithMessage("DefaultCategory still has content somewhere.");
	}

	public async Task<bool> HasNoChildren(Guid id, CancellationToken cancellationToken)
	{
		return await _context.Categories
			.Where(c => c.DefaultValue != null && c.DefaultValue.Id.Equals(id))
			.AllAsync(c => !c.ChildCategories.Any(), cancellationToken);
	}

	public async Task<bool> HasNoContent(Guid id, CancellationToken cancellationToken)
	{
		return await _context.Contents
			.AllAsync(c => !c.Categories.Any(c => c.DefaultValue != null && c.DefaultValue.Id.Equals(id)), cancellationToken);
	}
}
