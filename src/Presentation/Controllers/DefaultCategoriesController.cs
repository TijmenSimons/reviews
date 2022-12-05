using Microsoft.AspNetCore.Mvc;
using Template.Application.DefaultCategories.Commands.CreateDefaultCategory;
using Template.Application.DefaultCategories.Commands.DeleteDefaultCategories;
using Template.Application.DefaultCategories.Commands.LinkDefaultCategory;
using Template.Application.DefaultCategories.Commands.UpdateDefaultCategory;
using Template.Application.DefaultCategories.Queries.GetDefaultCategories;
using Template.Application.DefaultCategories.Queries.GetDefaultCategory;
using Template.Application.DefaultCategories.Queries.GetLinkedDefaultCategories;
using Template.Application.Dtos;

namespace Template.Presentation.Controllers;

public class DefaultCategoriesController : ApiControllerBase
{
	[HttpGet]
	public async Task<ActionResult<IList<DefaultCategoryDto>>> Get(
		[FromQuery] bool? isRoot
		)
	{
		return Ok(await Mediator.Send(new GetDefaultCategoriesQuery
		{
			IsRoot = isRoot
		}));
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> Get(Guid id)
	{
		return Ok(await Mediator.Send(new GetDefaultCategoryQuery { Id = id }));
	}

	[HttpPost]
	public async Task<ActionResult<DefaultCategoryDto>> Create(CreateDefaultCategoryCommand command)
	{
		return await Mediator.Send(command);
	}

	[HttpPatch]
	public async Task<ActionResult<DefaultCategoryDto>> Update(UpdateDefaultCategoryCommand command)
	{
		return await Mediator.Send(command);
	}

	// Functies werken, maar missen een belangrijk deel, maar ik wil eerst al data kunnen aanmaken.
	//[HttpDelete("{id:guid}")]
	//public async Task<ActionResult> Delete(Guid id)
	//{
	//	await Mediator.Send(new DeleteDefaultCategoryCommand(id));

	//	return NoContent();
	//}

	[HttpGet("{id:guid}/childcategories")]
	public async Task<IActionResult> GetChildren([FromRoute]Guid id)
	{
		return Ok(await Mediator.Send(new GetChildDefaultCategoriesQuery { Id = id }));
	}

	//[HttpPatch("{id:guid}/childcategories")]
	//public async Task<IActionResult> LinkChildren(Guid id, LinkChildDefaultCategoryCommand command)
	//{
	//	if (!id.Equals(command.ParentId))
	//		return BadRequest();

	//	await Mediator.Send(command);
	//	return Ok();
	//}

	//[HttpDelete("{id:guid}/childcategories")]
	//public async Task<IActionResult> DeleteLinkedChildren(Guid id, DeleteChildDefaultCategoryCommand command)
	//{
	//	await Mediator.Send(command);
	//	return Ok();
	//}
}
