using FluentValidation;

namespace Template.Application.Common.Extensions;

public static class RuleBuilderExtensions
{
	public static IRuleBuilder<T, string?> Url<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
		ruleBuilder.Must(uri => uri is null || Uri.TryCreate(uri, UriKind.Absolute, out _));
}
