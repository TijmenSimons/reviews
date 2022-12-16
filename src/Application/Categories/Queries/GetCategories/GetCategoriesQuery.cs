using System.Threading;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Mappings;
using Template.Application.Dtos;
using Template.Domain.Entities;

namespace Template.Application.Categories.Queries.GetCategories;

public record GetCategoriesQuery : IRequest<IList<CategoryDto>>
{
	public bool? IsRoot { get; set; }

	public bool? IsActive { get; set; }

}

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IList<CategoryDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public GetCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<IList<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
	{
		IQueryable<Category> query = _context.Categories;

		if (request.IsRoot is not null)
			query = query.Where(category => category.IsRoot == request.IsRoot);

		if (request.IsActive is not null)
			query = query.Where(category => category.IsActive == request.IsActive);

		return await query
			.AsNoTracking()
			.OrderBy(t => t.Name)
			.ProjectToListAsync<CategoryDto>(_mapper.ConfigurationProvider, cancellationToken);
	}
}
