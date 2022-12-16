using FluentValidation;
using Template.Application.Common.Constants;
using Template.Application.Common.Validators;

namespace Template.Application.Images.Commands.UploadImage;

public class UploadVideoCommandValidator : AbstractValidator<UploadImageCommand>
{

	public UploadVideoCommandValidator()
	{
		RuleFor(command => command.Image)
			.Cascade(CascadeMode.Stop)
			.NotNull()
			.MaxFileSize(FileConstants.MaxUploadSize)
			.IsSupportedImage();
	}
}
