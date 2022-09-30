using Template.Application.TodoLists.Queries.ExportTodos;

namespace Template.Application.Common.Interfaces;

public interface ICsvFileBuilder
{
	byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
}
