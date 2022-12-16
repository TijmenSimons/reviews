using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.DefaultCategories.Commands.DeleteDefaultCategories;

public record DeleteDefaultCategoryCommand(Guid Id) : IRequest;

public class DeleteDefaultCategoryCommandHandler : IRequestHandler<DeleteDefaultCategoryCommand>
{
	private readonly IApplicationDbContext _context;

	public DeleteDefaultCategoryCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Unit> Handle(DeleteDefaultCategoryCommand request, CancellationToken cancellationToken)
	{
		var defaultCategory = await _context.DefaultCategories.FirstOrDefaultAsync(c => c.Id.Equals(request.Id), cancellationToken) ?? throw new NotFoundException(nameof(DefaultCategory), request.Id);

		var substituteCategories = await _context.Categories
			.Where(c => c.DefaultValue != null && c.DefaultValue.Id.Equals(defaultCategory.Id))
			.Include(c => c.ChildCategories)
			.ToListAsync(cancellationToken);

		foreach (var substituteCategory in substituteCategories)
			if (substituteCategory.ChildCategories.Any())
				throw new Exception("Validator failed, still existing children.");

		_context.Categories.RemoveRange(substituteCategories);

		_context.DefaultCategories.Remove(defaultCategory);

		await _context.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
