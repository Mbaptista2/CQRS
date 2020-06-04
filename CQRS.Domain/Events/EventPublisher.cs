
using CQRS.Domain.Events;
using CQRS.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Infra.Data.RabbitMq
{
    public class EventPublisher
    {
		private readonly ConnectionFactory connectionFactory;
		private readonly IConfiguration _configuration;
		public EventPublisher(IHostingEnvironment env, IConfiguration configuration)
		{
			_configuration = configuration;
			var username = _configuration["amqp:username"];
			var password = _configuration["amqp:password"];
			var hostname = _configuration["amqp:hostname"];
			var uri = _configuration["amqp:uri"];
			var virtualhost = _configuration["amqp:virtualhost"];	
			
			connectionFactory = new ConnectionFactory() 
			{
				UserName = username,
				Password = password,
				HostName = hostname,
				Uri = new Uri(uri) ,
				VirtualHost = virtualhost
			};
								
		}
		public void PublishEvent<T>(T @event) where T : IEvent
		{
			using (IConnection conn = connectionFactory.CreateConnection())
			{
				using (IModel channel = conn.CreateModel())
				{
					var queue = @event is ClienteCriadoEvent ?
						Constants.QUEUE_CUSTOMER_CREATED : @event is ClienteAtualizadoEvent ?
							Constants.QUEUE_CUSTOMER_UPDATED : Constants.QUEUE_CUSTOMER_DELETED;
					channel.QueueDeclare(
						queue: queue,
						durable: false,
						exclusive: false,
						autoDelete: false,
						arguments: null
					);
					var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
					channel.BasicPublish(
						exchange: "",
						routingKey: queue,
						basicProperties: null,
						body: body
					);
				}
			}
		}
	}
}
