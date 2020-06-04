using CQRS.Domain.Events;
using CQRS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS.Domain.Commands
{
    public class CriarClienteCommand : Command
    {
		public string Nome { get; set; }
		public string Email { get; set; }
		public int Idade { get; set; }
		public List<CriarTelefoneCommand> Telefones { get; set; }

		public ClienteCriadoEvent ToCustomerEvent(int id)
		{
			return new ClienteCriadoEvent
			{
				Id = id,
				Nome = this.Nome,
				Email = this.Email,
				Idade = this.Idade,
				Telefones = this.Telefones.Select(phone => new TelefoneCriadoEvent { AreaCode = phone.AreaCode, Number = phone.Number }).ToList()
			};
		}

		public Cliente ToCustomerRecord()
		{
			return new Cliente
			{
				Nome = this.Nome,
				Email = this.Email,
				Idade = this.Idade,
				Telefones = this.Telefones.Select(phone => new Telefone { AreaCode = phone.AreaCode, Number = phone.Number }).ToList()
			};
		}
	}
}
