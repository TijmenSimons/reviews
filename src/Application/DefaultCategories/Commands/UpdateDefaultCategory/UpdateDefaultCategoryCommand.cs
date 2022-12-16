using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Application.Dtos;
using Template.Domain.Entities;

namespace Template.Application.DefaultCategories.Commands.UpdateDefaultCategory;

public record UpdateDefaultCategoryCommand : IRequest<DefaultCategoryDto>
{
	public Guid Id { get; set; } = default!; 

	public string? Name { get; set; } = default!;

	public string? Description { get; set; } = default!;

	public Guid? ImageId { get; set; }

	public Guid? PartnerId { get; set; }

	public Guid? ParentId { get; set; }
}

public class UpdateDefaultCategoryCommandHandler : IRequestHandler<UpdateDefaultCategoryCommand, DefaultCategoryDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public UpdateDefaultCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context; 
		_mapper = mapper;
	}

	public async Task<DefaultCategoryDto> Handle(UpdateDefaultCategoryCommand request, CancellationToken cancellationToken)
	{
		var defaultCategory = await _context.DefaultCategories
			.Include(defaultCategory => defaultCategory.ParentCategory)
			.FirstOrDefaultAsync(category => category.Id.Equals(request.Id), cancellationToken) ?? throw new NotFoundException(nameof(DefaultCategory), request.Id);

		defaultCategory.Name = request.Name!;
		defaultCategory.Description = request.Description!;

		defaultCategory.Image = request.ImageId is null
			? null
			: await _context.Images
				.FirstOrDefaultAsync(image => image.Id.Equals(request.ImageId), cancellationToken) ?? throw new NotFoundException(nameof(DefaultCategory), request.ImageId);

		defaultCategory.Partner = request.PartnerId is null
			? null
			: await _context.Partners
				.FirstOrDefaultAsync(partner => partner.Id.Equals(request.PartnerId), cancellationToken) ?? throw new NotFoundException(nameof(DefaultCategory), request.PartnerId);
		

		if (!defaultCategory.ParentCategory.Id.Equals(request.ParentId))
		{
			var hasDefaultsCategoriesToBeUpdated = await _context.Categories
				.Include(c => c.ChildCategories)
				.Where(c => c.HasDefaults == true)
				.ToListAsync(cancellationToken);

			// Don't search for all children because data should always be 2 children deep at max.
			foreach (var hasDefaultsCategory in hasDefaultsCategoriesToBeUpdated)
			{
				var substituteCategories = await _context.Categories
					.Where(c => c.DefaultValue != null && c.DefaultValue.Id.Equals(defaultCategory.Id))
					.Include(c => c.ParentCategory).ThenInclude(c => c.ParentCategory)
					.Include(c => c.ParentCategory).ThenInclude(c => c.ParentCategory).ThenInclude(c => c.DefaultValue)
					.Include(c => c.ParentCategory).ThenInclude(c => c.DefaultValue)
					.ToListAsync(cancellationToken) ?? throw new NotFoundException($"Couldn't find substitute categories"); ;

				foreach (var item in substituteCategories)
				{
					Console.WriteLine(item.Name);
					Console.WriteLine(item.ParentCategory.DefaultValue.Name);
					Console.WriteLine(item.ParentCategory.ParentCategory.DefaultValue.Name);
				}

				var substituteCategory = await _context.Categories
					.Where(c => c.DefaultValue != null && c.DefaultValue.Id.Equals(defaultCategory.Id))
					.Include(c => c.ParentCategory).ThenInclude(c => c!.ParentCategory)
					.Where(c => c.ParentCategory != null && 
						(
							(c.ParentCategory.HasDefaults && c.ParentCategory.Id.Equals(hasDefaultsCategory.Id)) ||
							(c.ParentCategory.ParentCategory != null && c.ParentCategory.ParentCategory.HasDefaults && c.ParentCategory.ParentCategory.Id.Equals(hasDefaultsCategory.Id)))
						)
					.FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException($"Couldn't find substitute category for: {defaultCategory.Name} in {hasDefaultsCategory.Name}"); ;

				Console.WriteLine(substituteCategory);

				if (defaultCategory.IsRoot)
					substituteCategory.ParentCategory = hasDefaultsCategory;
				else
				{
					var childCategories = await _context.Categories
						.Where(cc => hasDefaultsCategory.ChildCategories.Select(ci => ci.Id).Contains(cc.Id))
						.Include(c => c.DefaultValue).ThenInclude(c => c!.ParentCategory)
						.Include(c => c.ChildCategories)
						.ToListAsync(cancellationToken);

					var childChildCategoryIds = childCategories.SelectMany(cc => cc.ChildCategories).Select(cc => cc.Id);

					substituteCategory.ParentCategory = childCategories.FirstOrDefault(c => c.DefaultValue!.Id.Equals(defaultCategory.ParentCategory!.Id))
												 ?? await _context.Categories
													 .Where(c => childChildCategoryIds.Contains(c.Id))
													 .Include(c => c.DefaultValue).ThenInclude(c => c!.ParentCategory)
													 .Include(c => c.ChildCategories)
													 .FirstOrDefaultAsync(c => c.DefaultValue!.Id.Equals(defaultCategory.ParentCategory!.Id), cancellationToken)
												 ?? throw new NotFoundException($"Couldn't find parent category for: {defaultCategory.Name} in {hasDefaultsCategory.Name}");
				}
			}
		}
		await _context.SaveChangesAsync(cancellationToken);

		return await _context.DefaultCategories
			.AsNoTracking()
			.ProjectTo<DefaultCategoryDto>(_mapper.ConfigurationProvider)
			.FirstAsync(c => c.Id.Equals(defaultCategory.Id), cancellationToken);
	}
}
