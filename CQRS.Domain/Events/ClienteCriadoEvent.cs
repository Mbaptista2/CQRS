using CQRS.Domain.Interfaces;
using CQRS.Model.MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS.Domain.Events
{
    public class ClienteCriadoEvent : IEvent
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public List<TelefoneCriadoEvent> Telefones { get; set; }
        public ClienteMongo ToCustomerEntity()
        {
            return new ClienteMongo
            {
                Id = this.Id,
                Email = this.Email,
                Nome = this.Nome,
                Idade = this.Idade,
                Telefones = this.Telefones.Select(phone => new TelefoneMongo
                {
                    Type = phone.Type,
                    AreaCode = phone.AreaCode,
                    Number = phone.Number
                }).ToList()
            };
        }

    }
}
