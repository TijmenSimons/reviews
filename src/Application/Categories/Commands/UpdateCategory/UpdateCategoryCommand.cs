using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;
using Template.Application.Dtos;
using Template.Domain.Entities;

namespace Template.Application.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand : IRequest<CategoryDto>
{
	public Guid Id { get; set; } = default!; 

	public string? Name { get; set; } = default!;

	public string? Description { get; set; } = default!;

	public Guid? ImageId { get; set; }

	public Guid? PartnerId { get; set; }

	public Guid? ParentId { get; set; }

	public bool IsActive { get; set; } = true;

	public Guid? DefaultId { get; set; }

	public bool HasDefaults { get; set; }
}

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public UpdateCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context; 
		_mapper = mapper;
	}

	public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
	{
		var category = await _context.Categories
			.FirstOrDefaultAsync(category => category.Id.Equals(request.Id), cancellationToken) ?? throw new NotFoundException(nameof(Category), request.Id);

		category.Name = request.DefaultId is null ? request.Name! : string.Empty;
		category.Description = request.DefaultId is null ? request.Description! : string.Empty;
		category.IsRoot = request.ParentId is null;
		category.IsActive = request.IsActive;
		category.HasDefaults = request.HasDefaults;

		if (request.DefaultId is not null)
			category.DefaultValue = await _context.DefaultCategories
				.FirstOrDefaultAsync(partner => partner.Id.Equals(request.DefaultId), cancellationToken) ?? throw new NotFoundException(nameof(DefaultCategory), request.DefaultId);
		else
		{
			category.Image = request.ImageId is null
				? null
				: await _context.Images
					.FirstOrDefaultAsync(image => image.Id.Equals(request.ImageId), cancellationToken) ?? throw new NotFoundException(nameof(Category), request.ImageId);

			category.Partner = request.PartnerId is null
				? null
				: await _context.Partners
					.FirstOrDefaultAsync(partner => partner.Id.Equals(request.PartnerId), cancellationToken) ?? throw new NotFoundException(nameof(Category), request.PartnerId);
		}

		category.ParentCategory = request.ParentId is null 
			? null
			: await _context.Categories
				.FirstOrDefaultAsync(c => c.Id.Equals(request.ParentId), cancellationToken) ?? throw new NotFoundException(nameof(Category), request.ParentId);

		await _context.SaveChangesAsync(cancellationToken);

		return await _context.Categories
			.AsNoTracking()
			.ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
			.FirstAsync(c => c.Id.Equals(request.Id), cancellationToken);
	}
}
