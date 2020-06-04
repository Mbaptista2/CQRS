using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Domain.Commands
{
	public abstract class Command
	{
		public int Id { get; set; }
	}
}
