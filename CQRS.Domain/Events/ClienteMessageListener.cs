using CQRS.Infra.Data.MongoDB;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Logging;
using RabbitMQ.Client.MessagePatterns;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;


namespace CQRS.Domain.Events
{
    public class ClienteMessageListener
    {
		private readonly IClienteMongoDbRepository _repository;
		private readonly IConfiguration _configuration;
		public ClienteMessageListener(IClienteMongoDbRepository repository, IConfiguration configuration)
		{
			_repository = repository;
			_configuration = configuration;
		}
		public void Start(string contentRootPath)
		{
			
			var username = _configuration["amqp:username"];
			var password = _configuration["amqp:password"];
			var hostname = _configuration["amqp:hostname"];
			var uri = _configuration["amqp:uri"];
			var virtualhost = _configuration["amqp:virtualhost"];

			ConnectionFactory connectionFactory = new ConnectionFactory()
			{
				UserName = username,
				Password = password,
				HostName = hostname,
				Uri = new Uri(uri),
				VirtualHost = virtualhost
			};

			connectionFactory.AutomaticRecoveryEnabled = true;
			connectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(15);
			using (IConnection conn = connectionFactory.CreateConnection())
			{
				using (IModel channel = conn.CreateModel())
				{
					DeclareQueues(channel);
					var subscriptionCreated = new Subscription(channel, Constants.QUEUE_CUSTOMER_CREATED, false);
					var subscriptionUpdated = new Subscription(channel, Constants.QUEUE_CUSTOMER_UPDATED, false);
					var subscriptionDeleted = new Subscription(channel, Constants.QUEUE_CUSTOMER_DELETED, false);
					while (true)
					{
						// Sleeps for 5 sec before trying again
						Thread.Sleep(5000);
						new Thread(() =>
						{
							ListerCreated(subscriptionCreated);
						}).Start();
						new Thread(() =>
						{
							ListenUpdated(subscriptionUpdated);
						}).Start();
						new Thread(() =>
						{
							ListenDeleted(subscriptionDeleted);
						}).Start();
					}
				}
			}
		}
		private void ListenDeleted(Subscription subscriptionDeleted)
		{
			BasicDeliverEventArgs eventArgsDeleted = subscriptionDeleted.Next();
			if (eventArgsDeleted != null)
			{
				string messageContent = Encoding.UTF8.GetString(eventArgsDeleted.Body);
				ClienteExcluidoEvent _deleted = JsonConvert.DeserializeObject<ClienteExcluidoEvent>(messageContent);
				_repository.Remove(_deleted.Id);
				subscriptionDeleted.Ack(eventArgsDeleted);
			}
		}
		private void ListenUpdated(Subscription subscriptionUpdated)
		{
			BasicDeliverEventArgs eventArgsUpdated = subscriptionUpdated.Next();
			if (eventArgsUpdated != null)
			{
				string messageContent = Encoding.UTF8.GetString(eventArgsUpdated.Body);
				ClienteAtualizadoEvent _updated = JsonConvert.DeserializeObject<ClienteAtualizadoEvent>(messageContent);
				_repository.Update(_updated.ToCustomerEntity(_repository.GetCustomer(_updated.Id)));
				subscriptionUpdated.Ack(eventArgsUpdated);
			}
		}
		private void ListerCreated(Subscription subscriptionCreated)
		{
			BasicDeliverEventArgs eventArgsCreated = subscriptionCreated.Next();
			if (eventArgsCreated != null)
			{
				string messageContent = Encoding.UTF8.GetString(eventArgsCreated.Body);
				ClienteCriadoEvent _created = JsonConvert.DeserializeObject<ClienteCriadoEvent>(messageContent);
				_repository.Create(_created.ToCustomerEntity());
				subscriptionCreated.Ack(eventArgsCreated);
			}
		}
		private static void DeclareQueues(IModel channel)
		{
			channel.QueueDeclare(
				queue: Constants.QUEUE_CUSTOMER_CREATED,
				durable: false,
				exclusive: false,
				autoDelete: false,
				arguments: null
			);
			channel.QueueDeclare(
				queue: Constants.QUEUE_CUSTOMER_UPDATED,
				durable: false,
				exclusive: false,
				autoDelete: false,
				arguments: null
			);
			channel.QueueDeclare(
				queue: Constants.QUEUE_CUSTOMER_DELETED,
				durable: false,
				exclusive: false,
				autoDelete: false,
				arguments: null
			);
		}
	}
}
