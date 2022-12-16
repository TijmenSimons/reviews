using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;
using Template.Application.Dtos;
using Template.Domain.Entities;

namespace Template.Application.Contents.Commands.UpdateContent;
public record UpdateContentCommand : IRequest<ContentDto>
{
	public Guid Id { get; set; }

	public string Title { get; set; } = default!;

	public string Description { get; set; } = default!;

	public ICollection<IdEntity> CategoryIds { get; set; } = new List<IdEntity>();

	public Guid? VideoId { get; set; }

	public bool IsActive { get; set; } = true;
}

public class UpdateContentCommandHandler : IRequestHandler<UpdateContentCommand, ContentDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public UpdateContentCommandHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<ContentDto> Handle(UpdateContentCommand request, CancellationToken cancellationToken)
	{
		var content = await _context.Contents
			.Include(content => content.Categories)
			.FirstOrDefaultAsync(content => content.Id.Equals(request.Id), cancellationToken) ?? throw new NotFoundException(nameof(Content), request.Id);

		var categories = await _context.Categories
			.Where(category => request.CategoryIds.Select(ci => ci.Id).Contains(category.Id)).ToListAsync(cancellationToken);

		content.Title = request.Title;
		content.Description = request.Description;
		content.Categories = categories;

		if (request.VideoId is not null)
			content.Video = await _context.Videos
				.FirstOrDefaultAsync(video => video.Id.Equals(request.VideoId), cancellationToken) ?? throw new NotFoundException(nameof(Category), request.VideoId);

		await _context.SaveChangesAsync(cancellationToken);

		return _mapper.Map<ContentDto>(content);
	}
}
