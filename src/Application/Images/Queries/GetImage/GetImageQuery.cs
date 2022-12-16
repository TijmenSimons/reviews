using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Application.Dtos;
using Template.Domain.Entities;

namespace Template.Application.Images.Queries.GetImage;

public record GetImageQuery : IRequest<ImageDto>
{
	public Guid Id { get; set; }
}

public class GetImageQueryHandler : IRequestHandler<GetImageQuery, ImageDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public GetImageQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<ImageDto> Handle(GetImageQuery request, CancellationToken cancellationToken)
	{
		return _mapper.Map<ImageDto>(await _context.Images
				.AsNoTracking()
				.FirstOrDefaultAsync(image => image.Id.Equals(request.Id), cancellationToken) ?? throw new NotFoundException(nameof(Image), request.Id));
	}
}
