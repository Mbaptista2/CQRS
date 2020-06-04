using CQRS.Model.MongoDb;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Infra.Data.MongoDB
{
	public class ClienteMongoDbRepository : IClienteMongoDbRepository
	{
		private const string _customerDB = "ClienteDB";
		private const string _customerCollection = "Clientes";
		private IMongoDatabase _db;
		public ClienteMongoDbRepository()
		{
			MongoClient _client = new MongoClient("mongodb://localhost:27017");
			_db = _client.GetDatabase(_customerDB);
		}
		public List<ClienteMongo> GetCustomers()
		{
			return _db.GetCollection<ClienteMongo>(_customerCollection).Find(_ => true).ToList();
		}
		public ClienteMongo GetCustomer(long id)
		{
			return _db.GetCollection<ClienteMongo>(_customerCollection).Find(customer => customer.Id == id).SingleOrDefault();
		}
		public ClienteMongo GetCustomerByEmail(string email)
		{
			return _db.GetCollection<ClienteMongo>(_customerCollection).Find(customer => customer.Email == email).Single();
		}
		public void Create(ClienteMongo customer)
		{
			_db.GetCollection<ClienteMongo>(_customerCollection).InsertOne(customer);
		}
		public void Update(ClienteMongo customer)
		{
			var filter = Builders<ClienteMongo>.Filter.Where(_ => _.Id == customer.Id);
			_db.GetCollection<ClienteMongo>(_customerCollection).ReplaceOne(filter, customer);
		}
		public void Remove(long id)
		{
			var filter = Builders<ClienteMongo>.Filter.Where(_ => _.Id == id);
			var operation = _db.GetCollection<ClienteMongo>(_customerCollection).DeleteOne(filter);
		}
	}
}
