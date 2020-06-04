using CQRS.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using CQRS.Model;

namespace CQRS.Domain.Interfaces
{
    public interface IClienteRepository: IRepository<Cliente>
    {
        Cliente GetById(int id);
    }
}
