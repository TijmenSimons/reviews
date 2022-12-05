using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Mappings;
using Template.Application.Dtos;
using Template.Domain.Entities;

namespace Template.Application.DefaultCategories.Queries.GetDefaultCategories;

public record GetDefaultCategoriesQuery : IRequest<IList<DefaultCategoryDto>>
{
	public bool? IsRoot { get; set; }
}


public class GetDefaultCategoriesQueryHandler : IRequestHandler<GetDefaultCategoriesQuery, IList<DefaultCategoryDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public GetDefaultCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<IList<DefaultCategoryDto>> Handle(GetDefaultCategoriesQuery request, CancellationToken cancellationToken)
	{
		IQueryable<DefaultCategory> query = _context.DefaultCategories;

		if (request.IsRoot is not null)
			query = query.Where(category => category.IsRoot == request.IsRoot);

		return await query
			.AsNoTracking()
			.OrderBy(t => t.Name)
			.ProjectToListAsync<DefaultCategoryDto>(_mapper.ConfigurationProvider, cancellationToken);
	}
}
