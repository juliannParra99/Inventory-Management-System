using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;    

namespace Business
{
    public class DataAccess
    {
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataReader reader;
        //propiedad publica para poder acceder al contenido de reader
        public SqlDataReader Reader
        {
            get { return reader; }
        }


        public DataAccess()
        {
            connection = new SqlConnection("server=.\\SQLEXPRESS; database= CATALOGO_DB ;integrated security = true");
            command = new SqlCommand();

        }

        public void setearConsulta(string consulta)
        {
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = consulta;
        }

        public void ejecutarLectura()
        {
            command.Connection = connection;
            try
            {
                connection.Open();
                reader = command.ExecuteReader();
            }
            catch (Exception ex )
            {

                throw ex;
            }
        }

        public void cerrarConexion()
        {
            //es recomendable cerrar el  lector y la conexion siempre.
            if (reader != null)
            {
                reader.Close();

            }
            connection.Close();
        }

    }
}


