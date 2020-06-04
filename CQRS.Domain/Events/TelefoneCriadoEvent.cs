using CQRS.Domain.Interfaces;
using CQRS.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Domain.Events
{
    public class TelefoneCriadoEvent : IEvent
    {

        public TipoTelefone Type { get; set; }
        public int AreaCode { get; set; }
        public int Number { get; set; }

    }
}
