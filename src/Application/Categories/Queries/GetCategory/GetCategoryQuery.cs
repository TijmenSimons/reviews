using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Interfaces;
using Template.Application.Dtos;

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
		return _mapper.Map<CategoryDto>(
			await _context.Categories
				.AsNoTracking()
				.Include(category => category.Image)
				.Include(category => category.Partner)
				.Include(category => category.ParentCategory).ThenInclude(category => category.DefaultValue)
				.Include(category => category.ChildCategories)
				.Include(category => category.DefaultValue)
				.FirstOrDefaultAsync(category => category.Id.Equals(request.Id), cancellationToken));
	}
}
