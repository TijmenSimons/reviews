using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Videos.Commands.DeleteVideos;

public record DeleteVideoCommand(Guid Id) : IRequest;

public class DeleteVideoCommandHandler : IRequestHandler<DeleteVideoCommand>
{
	private readonly IApplicationDbContext _context;
	private readonly IConfiguration _configuration;

	public DeleteVideoCommandHandler(IApplicationDbContext context, IConfiguration configuration)
	{
		_context = context;
		_configuration = configuration;
	}

	public async Task<Unit> Handle(DeleteVideoCommand request, CancellationToken cancellationToken)
	{
		var video = await _context.Videos
			.FirstOrDefaultAsync(vid => vid.Id.Equals(request.Id), cancellationToken) ?? throw new NotFoundException(nameof(Video), request.Id);

		_context.Videos.Remove(video);

		File.Delete(Path.Combine(_configuration["VideoPath"], video.Id.ToString() + video.Extension));

		await _context.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
