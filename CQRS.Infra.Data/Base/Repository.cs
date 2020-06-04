
using CQRS.Domain;
using CQRS.Infra.Data.Interfaces;
using CQRS.Model.Base;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Infra.Data.Base
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        private readonly DataContext _context;

        public Repository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public IUnitOfWork unitOfWork
        {
            get
            {
                return _context;
            }
        }

        public T Create(T entity)
        {

            if (entity.IsTransient())
            {
                return _context.Add(entity).Entity;
            }
            else
                return entity;
        }

        public void Delete(int id)

        {

            _context.Remove(id);


        }

        public void Update(T entity)

        {

            //Write your logic here to update an entity

        }
    }
}
