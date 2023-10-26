using Infraestructura.Datos;
using Infraestructura.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.ContactosService
{
    public class MovimientoService
    {
        MovimientoDatos movimientoDatos;

        public MovimientoService(string cadenaConexion)
        {
            movimientoDatos = new MovimientoDatos(cadenaConexion);
        }


        public MovimientosModel obtenerMovimientosPorId(string nroCuenta)
        {
            return movimientoDatos.obtenerMovimientosPorId(nroCuenta);
        }

        public List<MovimientosModel> obtenerMovimientos()
        {
            return movimientoDatos.obtenerMovimientos();
        }

        public void InsertarMovimiento(MovimientosInsertModel movimiento)
        {
            movimientoDatos.InsertarMovimiento(movimiento);
        }

        public void ActualizarMovimiento(MovimientosInsertModel movimiento)
        {
            movimientoDatos.ActualizarMovimiento(movimiento);
        }

        public void EliminarMovimiento(int id)
        {
            movimientoDatos.EliminarMovimiento(id);
        }
    }
}
