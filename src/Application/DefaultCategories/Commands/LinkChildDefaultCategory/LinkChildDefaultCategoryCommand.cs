using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.DefaultCategories.Commands.LinkDefaultCategory;

public record LinkChildDefaultCategoryCommand : IRequest<Unit>
{
	public Guid ParentId { get; set; } = default!;

	public List<IdDefaultCategory> ChildIds { get; set; } = default!;
}

public class IdDefaultCategory
{
	public Guid Id { get; set; }
}

public class LinkChildDefaultCategoryCommandHandler : IRequestHandler<LinkChildDefaultCategoryCommand, Unit>
{
	private readonly IApplicationDbContext _context;

	public LinkChildDefaultCategoryCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}


	public async Task<Unit> Handle(LinkChildDefaultCategoryCommand request, CancellationToken cancellationToken)
	{
		var parentCategory = await _context.DefaultCategories.FirstOrDefaultAsync(c => c.Id.Equals(request.ParentId), cancellationToken) ?? throw new NotFoundException(nameof(Category), request.ParentId);

		var childCategories = await _context.DefaultCategories.Where(childCategorie => request.ChildIds.Select(ci => ci.Id).Contains(childCategorie.Id)).ToListAsync(cancellationToken);

		if (request.ChildIds.Count != childCategories.Count)
		{
			var missingIds = request.ChildIds.Where(ci => childCategories.FirstOrDefault(cc => cc.Id.Equals(ci)) is null);

			throw new NotFoundException(nameof(Category), string.Join(",", missingIds));
		}

		childCategories.ForEach(c => c.IsRoot = false);

		parentCategory.ChildCategories = childCategories;

		await _context.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
