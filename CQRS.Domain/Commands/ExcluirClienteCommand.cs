using CQRS.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Domain.Commands
{
    public class ExcluirClienteCommand : Command
    {
		internal ClienteExcluidoEvent ToCustomerEvent()
		{
			return new ClienteExcluidoEvent
			{
				Id = this.Id
			};
		}
	}
}
