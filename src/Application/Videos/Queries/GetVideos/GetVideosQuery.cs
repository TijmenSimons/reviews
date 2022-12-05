using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Mappings;
using Template.Application.Dtos;

namespace Template.Application.Videos.Queries.GetVideos;

public record GetVideosQuery : IRequest<IList<VideoDto>>;

public class GetVideosQueryHandler : IRequestHandler<GetVideosQuery, IList<VideoDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public GetVideosQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<IList<VideoDto>> Handle(GetVideosQuery request, CancellationToken cancellationToken)
	{
		return await _context.Videos
			.AsNoTracking()
			.Include(video => video.Creator)
			.ProjectToListAsync<VideoDto>(_mapper.ConfigurationProvider, cancellationToken);
	}
}
