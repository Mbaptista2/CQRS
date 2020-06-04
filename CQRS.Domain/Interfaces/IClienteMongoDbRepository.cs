using CQRS.Model.MongoDb;
using System.Collections.Generic;

namespace CQRS.Infra.Data.MongoDB
{
    public interface IClienteMongoDbRepository
    {
        void Create(ClienteMongo customer);
        ClienteMongo GetCustomer(long id);
        ClienteMongo GetCustomerByEmail(string email);
        List<ClienteMongo> GetCustomers();
        void Remove(long id);
        void Update(ClienteMongo customer);
    }
}