using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Template.Application.Common.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Images.Commands.UploadImage;

public record UploadImageCommand : IRequest<Guid>
{
	public IFormFile Image { get; set; } = default!;
}

public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, Guid>
{
	private readonly IApplicationDbContext _context;
	private readonly IConfiguration _configuration;

	public UploadImageCommandHandler(IApplicationDbContext context, IConfiguration configuration)
	{
		_context = context;
		_configuration = configuration;
	}

	public async Task<Guid> Handle(UploadImageCommand request, CancellationToken cancellationToken)
	{
		var image = new Image
		{
			Extension = Path.GetExtension(request.Image.FileName)
		};
		_context.Images.Add(image);

		var filePath = Path.Combine(_configuration["ImagePath"], image.Id.ToString() + image.Extension);
		using (var stream = System.IO.File.Create(filePath))
		{
			await request.Image.CopyToAsync(stream, cancellationToken);
		}

		await _context.SaveChangesAsync(cancellationToken);

		return image.Id;
	}
}
