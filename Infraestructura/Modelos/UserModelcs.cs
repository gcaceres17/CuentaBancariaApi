using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Modelos
{
    public class UserModel
    {
        [Key]
        public int id_usuario { get; set; }

        public PersonaInsertModel persona { get; set; }

        public string name { get; set; }

        public string password { get; set; }

        public string level { get; set; }

        public string status { get; set; }
    }

}

