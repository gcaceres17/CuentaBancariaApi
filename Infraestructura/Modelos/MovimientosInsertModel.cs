using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Modelos
{
    public class MovimientosInsertModel
    {
        public int idMovimiento { get; set; }

        public int idCuenta { get; set; }

        public DateTime fechaMovimiento { get; set; }

        public double saldoActual { get; set; }

        public double saldoAnterior { get; set; }

        public double MontoMovimiento { get; set; }

        public string tipoMovimiento { get; set; }

        public int cuentaOrigen { get; set; }

        public int cuentaDestino { get; set; }

        public int canal { get; set; }

    }
}
