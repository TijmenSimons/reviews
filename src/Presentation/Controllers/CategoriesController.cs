using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Application.Categories.Commands.CreateCategory;
using Template.Application.Categories.Commands.DeleteCategories;
using Template.Application.Categories.Commands.LinkCategory;
using Template.Application.Categories.Commands.UpdateCategory;
using Template.Application.Categories.Queries.GetCategories;
using Template.Application.Categories.Queries.GetCategory;
using Template.Application.Categories.Queries.GetLinkedCategories;
using Template.Application.Dtos;
using Template.Domain.Entities;
using Template.Domain.Enums;

namespace Template.Presentation.Controllers;

public class CategoriesController : ApiControllerBase
{
	[HttpGet]
	[AllowAnonymous]
	public async Task<ActionResult<IList<CategoryDto>>> Get(
		[FromQuery] bool? isRoot,
		[FromQuery] bool? isActive
		)
	{
		return Ok(await Mediator.Send(new GetCategoriesQuery
		{
			IsRoot = isRoot,
			IsActive = isActive
		}));
	}

	[HttpGet("{id:guid}")]
	[AllowAnonymous]
	public async Task<IActionResult> Get(Guid id)
	{
		return Ok(await Mediator.Send(new GetCategoryQuery { Id = id }));
	}

	[HttpPost]
	public async Task<ActionResult<CategoryDto>> Create(CreateCategoryCommand command)
	{
		return await Mediator.Send(command);
	}

	[HttpPatch]
	public async Task<ActionResult<CategoryDto>> Update(UpdateCategoryCommand command)
	{
		return await Mediator.Send(command);
	}

	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> Delete(Guid id)
	{
		await Mediator.Send(new DeleteCategoryCommand(id));

		return NoContent();
	}


	[HttpGet("{id:guid}/childcategories")]
	[AllowAnonymous]
	public async Task<IActionResult> GetChildren([FromRoute]Guid id)
	{
		return Ok(await Mediator.Send(new GetChildCategoriesQuery { Id = id }));
	}

	/// <summary>
	/// Insert list with linked id's as shown in the example, So  
	/// Make a field with parentId with selected main category, and a field with ChildIds { id : "Child id example"} 
	/// </summary>
	/// <param name="id"></param>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPatch("{id:guid}/childcategories")]
	public async Task<IActionResult> LinkChildren(Guid id, LinkChildCategoryCommand command)
	{
		if (!id.Equals(command.ParentId))
			return BadRequest();

		await Mediator.Send(command);
		return Ok();
	}



	[HttpDelete("{id:guid}/childcategories")]
	public async Task<IActionResult> DeleteLinkedChildren(Guid id, DeleteChildCategoryCommand command)
	{
		await Mediator.Send(command);
		return Ok();
	}


}
