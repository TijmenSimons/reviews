using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Application.Dtos;
using Template.Domain.Entities;

namespace Template.Application.DefaultCategories.Queries.GetDefaultCategory;

public record GetDefaultCategoryQuery : IRequest<DefaultCategoryDto>
{
	public Guid Id { get; set; }
}

public class GetDefaultCategoryQueryHandler : IRequestHandler<GetDefaultCategoryQuery, DefaultCategoryDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public GetDefaultCategoryQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<DefaultCategoryDto> Handle(GetDefaultCategoryQuery request, CancellationToken cancellationToken)
	{
		return await _context.DefaultCategories
			.AsNoTracking()
			.ProjectTo<DefaultCategoryDto>(_mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(category => category.Id.Equals(request.Id), cancellationToken)
			?? throw new NotFoundException(nameof(DefaultCategory), request.Id);
	}
}
