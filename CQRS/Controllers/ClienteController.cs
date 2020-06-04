using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Domain.Commands;
using CQRS.Domain.Interfaces;
using CQRS.Infra.Data.MongoDB;
using CQRS.Model;
using CQRS.Model.MongoDb;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CQRS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
		private readonly IClienteRepository _clienteRepository;
        private readonly ICommandHandler<Command> _commandHandler;
        private readonly IClienteMongoDbRepository _mongoRepository;
        public ClienteController(IClienteRepository clienteRepository,
            ICommandHandler<Command> commandHandler,
            IClienteMongoDbRepository mongoRepository
            )
		{
			_clienteRepository = clienteRepository;
            _commandHandler = commandHandler;
            _mongoRepository = mongoRepository;
            if (_mongoRepository.GetCustomers().Count == 0)
            {
                var customerCmd = new CriarClienteCommand
                {
                    Nome = "George Michaels",
                    Email = "george@email.com",
                    Idade = 23,
                    Telefones = new List<CriarTelefoneCommand>
                    {
                        new CriarTelefoneCommand { Type = Model.Enums.TipoTelefone.CELLPHONE, AreaCode = 123, Number = 7543010 }
                    }
                };

                _commandHandler.Execute(customerCmd);
            }
        }
       
       
        [HttpPost]
		public IActionResult Post([FromBody] CriarClienteCommand customer)
		{
            _commandHandler.Execute(customer);

            return CreatedAtRoute("GetCliente", new { id = customer.Id }, customer);
        }

		[HttpGet("{id}", Name = "GetCliente")]
		public IActionResult GetById(int id)
		{
			var customer = _mongoRepository.GetCustomer(id);
			if (customer == null)
			{
				return NotFound();
			}
			return new ObjectResult(customer);
		}
        [HttpGet]
        public List<ClienteMongo> Get()
        {
            return _mongoRepository.GetCustomers();
        }
    }
}