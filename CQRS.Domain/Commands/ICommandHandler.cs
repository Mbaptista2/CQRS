using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Domain.Commands
{
	public interface ICommandHandler<T> where T : Command
	{
		void Execute(T command);
	}
}
