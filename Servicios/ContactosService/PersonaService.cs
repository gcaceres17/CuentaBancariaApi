using System;
using Infraestructura.Modelos;
using Infraestructure.Datos;


namespace Servicios.ContactosService
{
    public class PersonaService
    {
        PersonasDatos personaDatos;

        public PersonaService(string cadenaConexion)
        {
            personaDatos = new PersonasDatos(cadenaConexion);
        }


        public PersonaModel obtenerPersonaById(string documento)
        {
            return personaDatos.obtenerPersonaPorId(documento);
        }

        public List<PersonaModel> obtenerPersonas()
        {
            return personaDatos.obtenerPersonas();
        }

        public void insertarPersona(PersonaInsertModel persona)
        {
            validarDatos(persona);
            personaDatos.insertarPersona(persona);
        }

        public void actualizarPersona(PersonaInsertModel persona)
        {
            validarDatos(persona);
            personaDatos.ActualizarPersona(persona);
        }

        public void eliminarPersona(int id)
        {
            personaDatos.eliminarPersona(id);
        }

        private void validarDatos(PersonaInsertModel persona)
        {
            if (persona.nombre.Trim().Length == 0)
            {
                throw new Exception("Se debe cargar el nombre");
            }

        }

    }
}

