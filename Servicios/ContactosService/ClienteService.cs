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
    public class ClienteService
    {
        ClienteDatos clientedatos;

        public ClienteService(string cadenaConexion)
        {
            clientedatos = new ClienteDatos(cadenaConexion);
        }

        public ClienteModel obtenerClientePorId(string documento)
        {
            return clientedatos.obtenerClientePorId(documento);
        }

        public List<ClienteModel> obtenerClientes()
        {
            return clientedatos.obtenerClientes();
        }

        public void insertarCliente(ClienteInsertModel cliente)
        {
            clientedatos.insertarCliente(cliente);
        }

        public void actualizarCliente (ClienteInsertModel cliente)
        {
            clientedatos.ActualizarCliente(cliente);
        }

        public void eliminarCliente (int id)
        {
            clientedatos.EliminarCliente(id);
        }
    }
}
