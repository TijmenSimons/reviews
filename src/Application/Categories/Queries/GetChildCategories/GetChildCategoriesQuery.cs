using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Application.Dtos;
using Template.Domain.Entities;

namespace Template.Application.Categories.Queries.GetLinkedCategories;
public record GetChildCategoriesQuery : IRequest<ICollection<CategoryDto>>
{
	// input client. // route parametets
	public Guid Id { get; set; }
	
}

public class GetChildCategoriesQueryHandler : IRequestHandler<GetChildCategoriesQuery, ICollection<CategoryDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	// constructor : 
	public GetChildCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<ICollection<CategoryDto>> Handle(GetChildCategoriesQuery request, CancellationToken cancellationToken)
	{
		
		var category = await _context.Categories
			.Include(category => category.ChildCategories).ThenInclude(category => category.Image)
			.Include(category => category.Image)
			.Include(category => category.ChildCategories).ThenInclude(category => category.DefaultValue)
			.Include(category => category.DefaultValue)
			.FirstOrDefaultAsync(category => category.Id.Equals( request.Id), cancellationToken: cancellationToken) ?? throw new NotFoundException(nameof(Category), request.Id);

		return _mapper.Map<ICollection<CategoryDto>>(category.ChildCategories); 
	}
}
