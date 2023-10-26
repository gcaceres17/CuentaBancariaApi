using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Modelos
{
    public class CiudadModel
    {
        [Key]
        public int idCiudad { get; set; }

        public string ciudad { get; set; }

        public string departamento { get; set; }

        public int codigo_postal { get; set; }
    }
}
