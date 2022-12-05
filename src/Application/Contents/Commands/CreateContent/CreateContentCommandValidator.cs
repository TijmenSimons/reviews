using FluentValidation;
using Template.Application.Common.Interfaces;

namespace Template.Application.Contents.Commands.CreateContent;

public class CreateContentCommandValidator : AbstractValidator<CreateContentCommand>
{
	private readonly IApplicationDbContext _context;

	public CreateContentCommandValidator(IApplicationDbContext context)
	{
		_context = context;

		RuleFor(c => c.Title)
			.NotEmpty().WithMessage("Title is required.");

		RuleFor(c => c.VideoId)
			.NotEmpty().WithMessage("Provide atleast 1 type of content");
	}
}
