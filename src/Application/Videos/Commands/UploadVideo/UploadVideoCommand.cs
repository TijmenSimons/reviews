using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Application.Dtos;
using Template.Domain.Entities;

namespace Template.Application.Videos.Commands.UploadVideo;

public record UploadVideoCommand : IRequest<Guid>
{
	public IFormFile Video { get; set; } = default!;

	public Guid CreatorId { get; set; } = default!;
}

public class UploadVideoCommandHandler : IRequestHandler<UploadVideoCommand, Guid>
{
	private readonly IApplicationDbContext _context;
	private readonly IConfiguration _configuration;
	private readonly IIdentityService _identityService;

	public UploadVideoCommandHandler(IApplicationDbContext context, IConfiguration configuration, IIdentityService identityService)
	{
		_context = context;
		_configuration = configuration;
		_identityService = identityService;
	}

	public async Task<Guid> Handle(UploadVideoCommand request, CancellationToken cancellationToken)
	{
		User creator = await _identityService.Get(request.CreatorId.ToString()) ?? throw new NotFoundException(nameof(User), request.CreatorId);
		
		var video = new Video
		{
			Creator = creator,
			Extension = Path.GetExtension(request.Video.FileName)
		};
		_context.Videos.Add(video);

		var filePath = Path.Combine(_configuration["VideoPath"], video.Id.ToString() + video.Extension);
		using (var stream = System.IO.File.Create(filePath))
		{
			await request.Video.CopyToAsync(stream, cancellationToken);
		}

		await _context.SaveChangesAsync(cancellationToken);

		return video.Id;
	}
}
