using CQRS.Domain.Interfaces;
using CQRS.Infra.Data.Base;
using CQRS.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace CQRS.Infra.Data
{
    public sealed class ClienteRepository: Repository<Cliente>, IClienteRepository
    {
        private readonly DataContext _context;
        public ClienteRepository(DataContext context):base(context)
        {
            _context = context;
        }

        public Cliente GetById(int id)
        {           
            Cliente cliente = _context.Clientes.Where(x => x.Id == id).FirstOrDefault();
            cliente.Telefones = _context.Telefone.Where(x => x.ClienteId == id).ToList();
            return cliente;           
        }
    }
}
