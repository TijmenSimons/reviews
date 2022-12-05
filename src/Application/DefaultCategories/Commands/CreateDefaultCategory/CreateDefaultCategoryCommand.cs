using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Application.Dtos;
using Template.Domain.Entities;

namespace Template.Application.DefaultCategories.Commands.CreateDefaultCategory;

public record CreateDefaultCategoryCommand : IRequest<DefaultCategoryDto>
{
	public string? Name { get; set; } = default!;

	public string? Description { get; set; } = default!;

	public Guid? ImageId { get; set; }

	public Guid? PartnerId { get; set; }

	public Guid? ParentId { get; set; }
}

public class CreateDefaultCategoryCommandHandler : IRequestHandler<CreateDefaultCategoryCommand, DefaultCategoryDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public CreateDefaultCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<DefaultCategoryDto> Handle(CreateDefaultCategoryCommand request, CancellationToken cancellationToken)
	{
		var defaultCategory = new DefaultCategory
		{
			Name = request.Name!,
			Description = request.Description!,
			IsRoot = request.ParentId is null,
			Image = request.ImageId is null
				? null
				: await _context.Images.FirstOrDefaultAsync(image => image.Id.Equals(request.ImageId), cancellationToken)
				  ?? throw new NotFoundException(nameof(DefaultCategory), request.ImageId),

			Partner = request.PartnerId is null
				? null
				: await _context.Partners.FirstOrDefaultAsync(partner => partner.Id.Equals(request.PartnerId), cancellationToken)
				  ?? throw new NotFoundException(nameof(DefaultCategory), request.PartnerId),

			ParentCategory = request.ParentId is null
				? null
				: await _context.DefaultCategories.FirstOrDefaultAsync(c => c.Id.Equals(request.ParentId), cancellationToken)
				  ?? throw new NotFoundException(nameof(DefaultCategory), request.ParentId)
		};

		_context.DefaultCategories.Add(defaultCategory);

		var categoriesToBeUpdated = await _context.Categories
			.Include(c => c.ChildCategories)
			.Where(c => c.HasDefaults == true)
			.ToListAsync(cancellationToken);

		// Don't search for all children because data should always be 2 children deep at max.
		foreach (var category in categoriesToBeUpdated)
		{
			var newCategory = new Category
			{
				Name = string.Empty,
				Description = string.Empty,
				DefaultValue = defaultCategory,
				IsRoot = false,
				IsActive = true,
				HasDefaults = false,
			};

			if (defaultCategory.IsRoot)
				newCategory.ParentCategory = category;
			else
			{
				var childCategories = await _context.Categories
					.Where(cc => category.ChildCategories.Select(ci => ci.Id).Contains(cc.Id))
					.Include(c => c.DefaultValue).ThenInclude(c => c!.ParentCategory)
					.Include(c => c.ChildCategories)
					.ToListAsync(cancellationToken);

				var childChildCategoryIds = childCategories.SelectMany(cc => cc.ChildCategories).Select(cc => cc.Id);

				newCategory.ParentCategory = childCategories.FirstOrDefault(c => c.DefaultValue!.Id.Equals(defaultCategory.ParentCategory!.Id))
				                             ?? await _context.Categories
					                             .Where(c => childChildCategoryIds.Contains(c.Id))
					                             .Include(c => c.DefaultValue).ThenInclude(c => c!.ParentCategory)
					                             .Include(c => c.ChildCategories)
					                             .FirstOrDefaultAsync(c => c.DefaultValue!.Id.Equals(defaultCategory.ParentCategory!.Id), cancellationToken)
				                             ?? throw new NotFoundException($"Couldn't find parent category for: {defaultCategory.Name} in {category.Name}");
			}
			_context.Categories.Add(newCategory);
		}

		await _context.SaveChangesAsync(cancellationToken);

		return _mapper.Map<DefaultCategoryDto>(defaultCategory);
	}
}
