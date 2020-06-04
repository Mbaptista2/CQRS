using CQRS.Domain.Interfaces;
using CQRS.Infra.Data.RabbitMq;
using CQRS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Domain.Commands
{
    public class ClienteCommandHandler: ICommandHandler<Command>
    {
		private IClienteRepository _repository;
		private EventPublisher _eventPublisher;

		public ClienteCommandHandler(EventPublisher eventPublisher, IClienteRepository repository)
		{
			_eventPublisher = eventPublisher;
			_repository = repository;
		}

		public void Execute(Command command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command is null");
			}

			if (command is CriarClienteCommand createCommand)
			{
				Cliente created = _repository.Create(createCommand.ToCustomerRecord());
				_repository.unitOfWork.SaveChangesAsync();
				_eventPublisher.PublishEvent(createCommand.ToCustomerEvent(created.Id));
			}
			else if (command is AtualizarClienteCommand updateCommand)
			{
				Cliente record = _repository.GetById(updateCommand.Id);
				_repository.Update(updateCommand.ToCustomerRecord(record));
				_repository.unitOfWork.SaveChangesAsync();
				_eventPublisher.PublishEvent(updateCommand.ToCustomerEvent());
			}
			else if (command is ExcluirClienteCommand deleteCommand)
			{
				_repository.Delete(deleteCommand.Id);
				_repository.unitOfWork.SaveChangesAsync();
				_eventPublisher.PublishEvent(deleteCommand.ToCustomerEvent());
			}
		}
	}
}
