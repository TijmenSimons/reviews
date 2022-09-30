using MediatR;
using Microsoft.Extensions.Logging;
using Template.Domain.Events;

namespace Template.Application.TodoItems.EventHandlers;

public class TodoItemCompletedEventHandler : INotificationHandler<TodoItemCompletedEvent>
{
	private readonly ILogger<TodoItemCompletedEventHandler> _logger;

	public TodoItemCompletedEventHandler(ILogger<TodoItemCompletedEventHandler> logger)
	{
		_logger = logger;
	}

	public Task Handle(TodoItemCompletedEvent notification, CancellationToken cancellationToken)
	{
		_logger.LogInformation("template_dotnet Domain Event: {DomainEvent}", notification.GetType().Name);

		return Task.CompletedTask;
	}
}
