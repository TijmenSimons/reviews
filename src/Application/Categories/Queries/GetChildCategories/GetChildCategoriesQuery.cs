using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Mappings;
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
		
		return await _context.Categories
			.Where(category => category.ParentCategory.Id.Equals(request.Id))
			.ProjectToListAsync<CategoryDto>(_mapper.ConfigurationProvider, cancellationToken) ?? throw new NotFoundException(nameof(Category), request.Id);
	}
}
