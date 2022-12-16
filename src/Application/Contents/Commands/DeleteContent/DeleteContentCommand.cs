using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Application.Dtos;
using Template.Domain.Entities;

namespace Template.Application.Contents.Commands.DeleteContent;
public record DeleteContentCommand(Guid Id) : IRequest;

public class DeleteContentCommandHandler : IRequestHandler<DeleteContentCommand>
{
	private readonly IApplicationDbContext _context;

	public DeleteContentCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Unit> Handle(DeleteContentCommand request, CancellationToken cancellationToken)
	{
		var content = await _context.Contents.FirstOrDefaultAsync(c => c.Id.Equals(request.Id), cancellationToken) ?? throw new NotFoundException(nameof(Content), request.Id);

		_context.Contents.Remove(content);

		await _context.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
