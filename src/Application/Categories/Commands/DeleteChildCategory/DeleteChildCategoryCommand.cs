using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Categories.Commands.LinkCategory;

public record DeleteChildCategoryCommand : IRequest<Unit>
{
	public Guid ParentId { get; set; }

	public List<IdCategory> ChildIds { get; set; } = new();
}


public class DeleteChildCategoryCommandHandler : IRequestHandler<DeleteChildCategoryCommand, Unit>
{
	private readonly IApplicationDbContext _context;

	public DeleteChildCategoryCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Unit> Handle(DeleteChildCategoryCommand request, CancellationToken cancellationToken)
	{
		var parentCategory = await _context.Categories
			.Include(category => category.ChildCategories)
			.FirstOrDefaultAsync(category => category.Id.Equals(request.ParentId), cancellationToken) ?? throw new NotFoundException(nameof(Category), request.ParentId);

		List<Category> toRemove = new();

		foreach (var item in parentCategory.ChildCategories)
			if (request.ChildIds.All(child => !child.Id.Equals(item.Id)))
				toRemove.Add(item);

		foreach (var item in toRemove)
			parentCategory.ChildCategories.Remove(item);

		await _context.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
