using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using Template.Application.Common.Interfaces;

namespace Template.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
	private readonly Stopwatch _timer;
	private readonly ILogger<TRequest> _logger;
	private readonly ICurrentUserService _currentUserService;
	private readonly IIdentityService _identityService;

	public PerformanceBehaviour(
		ILogger<TRequest> logger,
		ICurrentUserService currentUserService,
		IIdentityService identityService)
	{
		_timer = new Stopwatch();

		_logger = logger;
		_currentUserService = currentUserService;
		_identityService = identityService;
	}

	public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
	{
		_timer.Start();

		var response = await next();

		_timer.Stop();

		var elapsedMilliseconds = _timer.ElapsedMilliseconds;

		if (elapsedMilliseconds > 500)
		{
			var requestName = typeof(TRequest).Name;
			var userId = _currentUserService.UserId ?? string.Empty;
			var userName = string.IsNullOrEmpty(userId) ? string.Empty : await _identityService.GetUserNameAsync(userId);

			if (_logger.IsEnabled(LogLevel.Warning))
				_logger.LogWarning("template_dotnet Long Running Request: {RequestName} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
					requestName, elapsedMilliseconds, userId, userName, request);
		}

		return response;
	}
}
