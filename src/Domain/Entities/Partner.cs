using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Domain.Entities;
public class Partner
{
	public Guid Id { get; set; }

	public string Name { get; set; } = default!;

	public string LogoPath { get; set; } = default!;

	public string? Url { get; set; }
}
