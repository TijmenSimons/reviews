using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Application.Dtos;
using Template.Domain.Entities;

namespace Template.Application.Categories.Queries.GetCategory;

public record GetCategoryQuery : IRequest<CategoryDto>
{
	public Guid Id { get; set; }
}

public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public GetCategoryQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<CategoryDto> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
	{
		return await _context.Categories
			.AsNoTracking()
			.ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(category => category.Id.Equals(request.Id), cancellationToken)
			?? throw new NotFoundException(nameof(Category), request.Id);
	}
}
