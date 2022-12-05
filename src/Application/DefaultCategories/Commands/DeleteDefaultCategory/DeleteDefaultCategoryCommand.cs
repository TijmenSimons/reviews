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
		var category = await _context.DefaultCategories.FirstOrDefaultAsync(c => c.Id.Equals(request.Id), cancellationToken) ?? throw new NotFoundException(nameof(Category), request.Id);

		_context.DefaultCategories.Remove(category);

		await _context.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
