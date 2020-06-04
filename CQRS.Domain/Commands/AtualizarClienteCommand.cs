using CQRS.Domain.Events;
using CQRS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS.Domain.Commands
{
    public class AtualizarClienteCommand : Command
    {
		public string Nome { get; set; }
		public int Idade { get; set; }
		public List<CriarTelefoneCommand> Telefones { get; set; }

		public ClienteAtualizadoEvent ToCustomerEvent()
		{
			return new ClienteAtualizadoEvent
			{
				Id = this.Id,
				Nome = this.Nome,
				Idade = this.Idade,
				Telefones = this.Telefones.Select(phone => new TelefoneCriadoEvent
				{
					Type = phone.Type,
					AreaCode = phone.AreaCode,
					Number = phone.Number
				}).ToList()
			};
		}

		public Cliente ToCustomerRecord(Cliente record)
		{
			record.Nome = this.Nome;
			record.Nome = this.Nome;
			record.Telefones = this.Telefones.Select(phone => new Telefone
			{
				Type = phone.Type,
				AreaCode = phone.AreaCode,
				Number = phone.Number
			}).ToList()
				;
			return record;
		}
	}
}
