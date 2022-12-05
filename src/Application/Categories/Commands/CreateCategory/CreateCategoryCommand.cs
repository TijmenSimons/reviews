using System.Xml.Linq;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;
using Template.Application.Dtos;
using Template.Domain.Entities;

namespace Template.Application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand : IRequest<CategoryDto>
{
	public string? Name { get; set; }

	public string? Description { get; set; }

	public Guid? ImageId { get; set; }

	public Guid? PartnerId { get; set; }

	public Guid? ParentId { get; set; }

	public bool IsActive { get; set; } = true;

	public Guid? DefaultId { get; set; }

	public bool HasDefaults { get; set; } = false;
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public CreateCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
	{
		var category = new Category
		{
			Name = request.DefaultId is null ? request.Name! : string.Empty,
			Description = request.DefaultId is null ? request.Description! : string.Empty,
			Image = null,
			Partner = null,
			IsRoot = request.ParentId is null,
			IsActive = request.IsActive,
			DefaultValue = null,
			HasDefaults = request.HasDefaults,
		};

		if (request.DefaultId is not null)
			category.DefaultValue = await _context.DefaultCategories
				.FirstOrDefaultAsync(partner => partner.Id.Equals(request.DefaultId), cancellationToken) ?? throw new NotFoundException(nameof(DefaultCategory), request.DefaultId);
		else
		{
			if (request.ImageId is not null)
				category.Image = await _context.Images
					.FirstOrDefaultAsync(image => image.Id.Equals(request.ImageId), cancellationToken) ?? throw new NotFoundException(nameof(Category), request.ImageId);

			if (request.PartnerId is not null)
				category.Partner = await _context.Partners.FirstOrDefaultAsync(partner => partner.Id.Equals(request.PartnerId), cancellationToken) ?? throw new NotFoundException(nameof(Category), request.PartnerId);
		}

		category.ParentCategory = request.ParentId is null
			? null
			: await _context.Categories.FirstOrDefaultAsync(c => c.Id.Equals(request.ParentId), cancellationToken) 
			  ?? throw new NotFoundException(nameof(Category), request.ParentId);

		_context.Categories.Add(category);

		await _context.SaveChangesAsync(cancellationToken);

		return _mapper.Map<CategoryDto>(category);
	}
}
