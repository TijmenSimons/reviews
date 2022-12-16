using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Application.Dtos;
using Template.Domain.Entities;

namespace Template.Application.DefaultCategories.Queries.GetLinkedDefaultCategories;
public record GetChildDefaultCategoriesQuery : IRequest<ICollection<DefaultCategoryDto>>
{
	public Guid Id { get; set; }
}

public class GetChildDefaultCategoriesQueryHandler : IRequestHandler<GetChildDefaultCategoriesQuery, ICollection<DefaultCategoryDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public GetChildDefaultCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<ICollection<DefaultCategoryDto>> Handle(GetChildDefaultCategoriesQuery request, CancellationToken cancellationToken)
	{
		
		var category = await _context.DefaultCategories
			.Include(category => category.ChildCategories).ThenInclude(category => category.Image)
			.Include(category => category.Image)
			.FirstOrDefaultAsync(category => category.Id.Equals( request.Id), cancellationToken: cancellationToken) ?? throw new NotFoundException(nameof(Category), request.Id);

		return _mapper.Map<ICollection<DefaultCategoryDto>>(category.ChildCategories); 
	}
}
