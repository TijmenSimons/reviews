using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Interfaces;
using Template.Application.Dtos;

namespace Template.Application.Contents.Queries.GetContent;
public record GetContentQuery : IRequest<ContentDto>
{
	public Guid Id { get; set; }
}

class GetContentQueryHandler : IRequestHandler<GetContentQuery, ContentDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public GetContentQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<ContentDto> Handle(GetContentQuery request, CancellationToken cancellationToken)
	{
		return _mapper.Map<ContentDto>(
			await _context.Contents
				.AsNoTracking()
				.ProjectTo<ContentDto>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(content => content.Id.Equals(request.Id), cancellationToken));
	}
}
