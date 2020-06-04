using CQRS.Domain;
using CQRS.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Infra.Data.Interfaces
{
    public interface IRepository<T> where T : EntityBase
    {
        IUnitOfWork unitOfWork { get; }      

        T Create(T entity);

        void Delete(int id);

        void Update(T entity);

    }
}
