using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;
using Template.Application.Dtos;
using Template.Domain.Entities;

namespace Template.Application.Contents.Commands.CreateContent;

public record CreateContentCommand : IRequest<ContentDto>
{
	public string Title { get; set; } = default!;

	public string Description { get; set; } = default!;

	public ICollection<IdEntity> CategoryIds { get; set; } = new List<IdEntity>();

	public Guid? VideoId { get; set; }

	public bool IsActive { get; set; } = true;
}

public class CreateContentCommandHandler : IRequestHandler<CreateContentCommand, ContentDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public CreateContentCommandHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<ContentDto> Handle(CreateContentCommand request, CancellationToken cancellationToken)
	{
		var content = new Content()
		{
			Title = request.Title,
			Description = request.Description,
		};

		var categories = await _context.Categories
			.Where(category => request.CategoryIds.Select(ci => ci.Id).Contains(category.Id))
			.Include(category => category.DefaultValue)
			.ToListAsync(cancellationToken);

		if (request.CategoryIds.Count != categories.Count)
		{
			var missingIds = request.CategoryIds.Where(ci => categories.FirstOrDefault(cc => cc.Id.Equals(ci)) is null);

			throw new NotFoundException(nameof(Category), string.Join(",", missingIds));
		}

		content.Categories = categories;

		if (request.VideoId is not null)
			content.Video = await _context.Videos
				.FirstOrDefaultAsync(video => video.Id.Equals(request.VideoId), cancellationToken) ?? throw new NotFoundException(nameof(Category), request.VideoId);

		_context.Contents.Add(content);

		await _context.SaveChangesAsync(cancellationToken);

		return _mapper.Map<ContentDto>(content);
	}
}
