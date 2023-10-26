using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Modelos
{
    public class ClienteInsertModel
    {
        public int idCliente { get; set; }

        public DateTime fechaIngreso { get; set; }

        public int IdPersona { get; set; }

        public string calificacion { get; set; }

        public string estado { get; set; }
    }
}

