using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Application.Dtos;
using Template.Domain.Entities;

namespace Template.Application.Videos.Queries.GetVideo;

public record GetVideoQuery : IRequest<VideoDto>
{
	public Guid Id { get; set; }
}

public class GetVideoQueryHandler : IRequestHandler<GetVideoQuery, VideoDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public GetVideoQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<VideoDto> Handle(GetVideoQuery request, CancellationToken cancellationToken)
	{
		return _mapper.Map<VideoDto>(await _context.Videos
				.AsNoTracking()
				.Include(video => video.Creator)
				.FirstOrDefaultAsync(video => video.Id.Equals(request.Id), cancellationToken) ?? throw new NotFoundException(nameof(Video), request.Id));
	}
}
