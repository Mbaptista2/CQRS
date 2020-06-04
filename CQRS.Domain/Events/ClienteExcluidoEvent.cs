using CQRS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Domain.Events
{
    public class ClienteExcluidoEvent: IEvent
    {
        public int Id { get; set; }
    }
}
