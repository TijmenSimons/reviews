using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Template.Presentation.Pages;

public class AuthorizeModel : PageModel
{
	public IActionResult OnGet()
	{
		return Page();
	}
}
