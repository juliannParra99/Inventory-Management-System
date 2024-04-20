using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Business
{
    public class MarcaBusiness
    {
        public List<Marca> listarMarca()
        {
            List<Marca> listaMarcas = new List<Marca>();
            DataAccess data = new DataAccess();

            try
            {
                data.setearConsulta("select Id,Descripcion from MARCAS");
                data.ejecutarLectura();

                while (data.Reader.Read())
                {
                    Marca aux = new Marca();
                    aux.Id = (int)data.Reader["Id"];
                    aux.Descripcion = (string)data.Reader["Descripcion"];

                    listaMarcas.Add(aux);
                }

                return listaMarcas;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                data.cerrarConexion();
            }
        }
    }
}
