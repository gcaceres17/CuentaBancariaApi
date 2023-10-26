using Infraestructura.Datos;
using Infraestructura.Modelos;
using Infraestructure.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.ContactosService
{
    public class CuentaService
    {

        CuentaDatos cuentaDatos;

        public CuentaService(string cadenaConexion)
        {
            cuentaDatos = new CuentaDatos(cadenaConexion);
        }


        public CuentaModel obtenerCuentasPorId(string nroCuenta)
        {
            return cuentaDatos.obtenerCuentasPorId(nroCuenta);
        }

        public List<CuentaModel> obtenerCuentas()
        {
            return cuentaDatos.obtenerCuentas();
        }

        public void InsertarCuenta(CuentaInserModel cuenta)
        {
            cuentaDatos.InsertarCuenta(cuenta);
        }

        public void ActualizarCuenta(CuentaInserModel cuenta)
        {
            cuentaDatos.ActualizarCuenta(cuenta);
        }

        public void EliminarCuenta(int id)
        {
            cuentaDatos.EliminarCuenta(id);
        }
    }
}
