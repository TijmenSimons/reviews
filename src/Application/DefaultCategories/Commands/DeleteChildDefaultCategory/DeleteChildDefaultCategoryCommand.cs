using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Categories.Commands.LinkCategory;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.DefaultCategories.Commands.LinkDefaultCategory;

public record DeleteChildDefaultCategoryCommand : IRequest<Unit>
{
	public Guid ParentId { get; set; }

	public List<IdDefaultCategory> ChildIds { get; set; } = new();
}



public class DeleteChildDefaultCategoryCommandHandler : IRequestHandler<DeleteChildDefaultCategoryCommand, Unit>
{
	private readonly IApplicationDbContext _context;

	public DeleteChildDefaultCategoryCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	
	public async Task<Unit> Handle(DeleteChildDefaultCategoryCommand request, CancellationToken cancellationToken)
	{ 
		var parentCategory = await _context.DefaultCategories
			.Include(category => category.ChildCategories)
			.FirstOrDefaultAsync(category => category.Id.Equals(request.ParentId), cancellationToken) ?? throw new NotFoundException(nameof(Category), request.ParentId);

		List<DefaultCategory> toRemove = new();

		foreach (var item in parentCategory.ChildCategories)
			if (request.ChildIds.All(child => !child.Id.Equals(item.Id)))
				toRemove.Add(item);

		foreach (var item in toRemove)
			parentCategory.ChildCategories.Remove(item);

		await _context.SaveChangesAsync(cancellationToken);

		return Unit.Value;

	}
}
