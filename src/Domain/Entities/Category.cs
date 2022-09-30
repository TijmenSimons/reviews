using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Domain.Entities;
public class Category
{
	public Guid Id { get; set; }

	public string Name { get; set; } = default!;

	public string Description { get; set; } = default!;

	public string? ImagePath { get; set; }

	public Partner? Partner { get; set; }

	public bool IsActive { get; set; } = true;

	public bool IsMainCategory { get; set; } = true;


}
