using CQRS.Domain.Interfaces;
using CQRS.Model.MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS.Domain.Events
{
    public class ClienteAtualizadoEvent : IEvent
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public List<TelefoneCriadoEvent> Telefones { get; set; }
        public ClienteMongo ToCustomerEntity(ClienteMongo entity)
        {
            return new ClienteMongo
            {
                Id = this.Id,
                Email = entity.Email,
                Nome = entity.Nome.Equals(this.Nome) ? entity.Nome : this.Nome,
                Idade = entity.Idade.Equals(this.Idade) ? entity.Idade : this.Idade,
                Telefones = GetNewOnes(entity.Telefones).Select(phone => new TelefoneMongo { AreaCode = phone.AreaCode, Number = phone.Number }).ToList()
            };
        }
        private List<TelefoneMongo> GetNewOnes(List<TelefoneMongo> Phones)
        {
            return Phones.Where(a => !this.Telefones.Any(x => x.Type == a.Type
                && x.AreaCode == a.AreaCode
                && x.Number == a.Number)).ToList<TelefoneMongo>();
        }

    }
}
