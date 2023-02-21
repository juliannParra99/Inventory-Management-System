using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Business
{
    public class ArticuloBusiness
    {
        public List<Articulo> listarArticulo()
        {
            List<Articulo> listaArticulo = new List<Articulo>();
            DataAccess data = new DataAccess();
            try
            {
                data.setearConsulta("select Codigo,Nombre, Descripcion, ImagenUrl from ARTICULOS");
                data.ejecutarLectura();

                //si hay un registro,una fila, va a recorrer los valores de esa fila, osea las columnas.
                while (data.Reader.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Codigo = (string)data.Reader["Codigo"];
                    aux.Nombre = (string)data.Reader["Nombre"];
                    aux.Descripcion = (string)data.Reader["Descripcion"];
                    aux.ImagenUrl = (string)data.Reader["ImagenUrl"];

                    listaArticulo.Add(aux);

                }
                return listaArticulo;
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
