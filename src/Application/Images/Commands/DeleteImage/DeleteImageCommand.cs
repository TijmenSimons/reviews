using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Images.Commands.DeleteImages;

public record DeleteImageCommand(Guid Id) : IRequest;

public class DeleteImageCommandHandler : IRequestHandler<DeleteImageCommand>
{
	private readonly IApplicationDbContext _context;
	private readonly IConfiguration _configuration;

	public DeleteImageCommandHandler(IApplicationDbContext context, IConfiguration configuration)
	{
		_context = context;
		_configuration = configuration;
	}

	public async Task<Unit> Handle(DeleteImageCommand request, CancellationToken cancellationToken)
	{
		var image = await _context.Images
			.FirstOrDefaultAsync(i => i.Id.Equals(request.Id), cancellationToken) ?? throw new NotFoundException(nameof(Image), request.Id);

		_context.Images.Remove(image);

		File.Delete(Path.Combine(_configuration["ImagePath"], image.Id.ToString() + image.Extension));

		await _context.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
