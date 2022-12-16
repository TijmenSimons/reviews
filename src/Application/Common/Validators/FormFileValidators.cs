using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Template.Application.Common.Validators;

public static class FormFileValidators
{
	public static IRuleBuilderOptions<T, IFormFile> MaxFileSize<T>(this IRuleBuilder<T, IFormFile> ruleBuilder, int bytes) =>
		ruleBuilder
			.Must(file => file.Length < bytes)
			.WithMessage($"File is to large. Maximum allowed file size is {bytes} bytes.");

	public static IRuleBuilderOptions<T, IFormFile> IsSupportedImage<T>(this IRuleBuilder<T, IFormFile> ruleBuilder)
	{
		return ruleBuilder
			.Must(file =>
			{
				using var reader = new BinaryReader(file.OpenReadStream());
				return reader.ReadBytes(PngSignature.Length).Take(PngSignature.Length).SequenceEqual(PngSignature);
			})
			.WithMessage("Only 'PNG' is allowed");
	}

	public static IRuleBuilderOptions<T, IFormFile> IsSupportedVideo<T>(this IRuleBuilder<T, IFormFile> ruleBuilder)
	{
		return ruleBuilder
			.Must(file =>
			{
				using var reader = new BinaryReader(file.OpenReadStream());
				return reader.ReadBytes(Mp4Signature.Length).Take(Mp4Signature.Length).SequenceEqual(Mp4Signature);
			})
			.WithMessage("Only 'MP4' is allowed");
	}

	private static readonly byte[] PngSignature = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
	private static readonly byte[] Mp4Signature = { 0x00, 0x00, 0x00, 0x20, 0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D };
}
