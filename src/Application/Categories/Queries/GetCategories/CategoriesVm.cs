using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Application.Categories.Queries.GetCategories;
public class CategoriesVm
{
	public IList<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
}
