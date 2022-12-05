using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Categories.Commands.LinkCategory;

public record LinkChildCategoryCommand : IRequest<Unit>
{
	public Guid ParentId { get; set; } = default!;

	public List<IdCategory> ChildIds { get; set; } = default!;


}

public class IdCategory
{
	public Guid Id { get; set; }
}

public class LinkChildCategoryCommandHandler : IRequestHandler<LinkChildCategoryCommand, Unit>
{
	private readonly IApplicationDbContext _context;

	public LinkChildCategoryCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}


	public async Task<Unit> Handle(LinkChildCategoryCommand request, CancellationToken cancellationToken)
	{
		var parentCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id.Equals(request.ParentId), cancellationToken) ?? throw new NotFoundException(nameof(Category), request.ParentId);

		var childCategories = await _context.Categories.Where(childCategorie => request.ChildIds.Select(ci => ci.Id).Contains(childCategorie.Id)).ToListAsync(cancellationToken);

		if (request.ChildIds.Count != childCategories.Count)
		{
			var missingIds = request.ChildIds.Where(ci => childCategories.FirstOrDefault(cc => cc.Id.Equals(ci)) is null);

			throw new NotFoundException(nameof(Category), string.Join(",", missingIds));
		}

		parentCategory.ChildCategories = childCategories;

		await _context.SaveChangesAsync(cancellationToken);

		return Unit.Value;

	}
}
