using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Mappings;
using Template.Application.Dtos;

namespace Template.Application.Images.Queries.GetImages;

public record GetImagesQuery : IRequest<IList<ImageDto>>;

public class GetImagesQueryHandler : IRequestHandler<GetImagesQuery, IList<ImageDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public GetImagesQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<IList<ImageDto>> Handle(GetImagesQuery request, CancellationToken cancellationToken)
	{
		return await _context.Images
			.AsNoTracking()
			.ProjectToListAsync<ImageDto>(_mapper.ConfigurationProvider, cancellationToken);
	}
}
