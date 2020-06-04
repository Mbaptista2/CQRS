using CQRS.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Model
{
    public class Telefone
    {
        public long Id { get; set; }
        public TipoTelefone Type { get; set; }
        public int AreaCode { get; set; }
        public int Number { get; set; }

        public int ClienteId { get; set; }        
    }
}
