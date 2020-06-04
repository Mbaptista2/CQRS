using CQRS.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Model
{
    public class Cliente: EntityBase
    {      
        public string Email { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public List<Telefone> Telefones { get; set; }
    }
}
