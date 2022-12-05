using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Mappings;
using Template.Application.Dtos;
using Template.Domain.Entities;

namespace Template.Application.Contents.Queries.GetContents;
public record GetContentsQuery : IRequest<List<ContentDto>>
{
	public bool? IsActive { get; set; }
}

public class GetContentsQueryHandler : IRequestHandler<GetContentsQuery, List<ContentDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public GetContentsQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<List<ContentDto>> Handle(GetContentsQuery request, CancellationToken cancellationToken)
	{
		IQueryable<Content> query = _context.Contents;

		if (request.IsActive is not null)
			query = query.Where(category => category.IsActive == request.IsActive);

		return await query
			.AsNoTracking()
			.OrderBy(t => t.Title)
			.ProjectToListAsync<ContentDto>(_mapper.ConfigurationProvider, cancellationToken);
	}
}
