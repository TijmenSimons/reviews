using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Categories.Commands.DeleteCategories;

public record DeleteCategoryCommand(Guid Id) : IRequest;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
	private readonly IApplicationDbContext _context;

	public DeleteCategoryCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
	{
		var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id.Equals(request.Id), cancellationToken) ?? throw new NotFoundException(nameof(Category), request.Id);

		_context.Categories.Remove(category);

		await _context.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
