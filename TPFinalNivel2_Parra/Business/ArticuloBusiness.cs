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
            List<Articulo> listaArticulos = new List<Articulo>();
            DataAccess data = new DataAccess();
            try
            {
                data.setearConsulta("select A.Codigo,A.Nombre, A.Descripcion, A.ImagenUrl, M.Descripcion marca, C.Descripcion categoria, A.Precio from Articulos as A , MARCAS as M, CATEGORIAS as C where A.IdMarca = M.Id and A.IdCategoria = C.Id");
                data.ejecutarLectura();

                //si hay un registro,una fila, va a recorrer los valores de esa fila, osea las columnas.
                while (data.Reader.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Codigo = (string)data.Reader["Codigo"];
                    aux.Nombre = (string)data.Reader["Nombre"];
                    aux.Descripcion = (string)data.Reader["Descripcion"];
                    aux.ImagenUrl = (string)data.Reader["ImagenUrl"];
                    aux.Precio = (decimal)data.Reader["Precio"];

                    aux.Marca = new Marca();
                    
                    aux.Marca.Descripcion = (string)data.Reader["marca"];

                    aux.Categoria = new Categoria();

                    aux.Categoria.Descripcion = (string)data.Reader["categoria"];
                    

                    listaArticulos.Add(aux);

                }
                return listaArticulos;
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

        public void agregarArticulo(Articulo nuevo)
        {
            DataAccess data = new DataAccess();
            try
            {
                data.setearConsulta("insert into ARTICULOS (Codigo,Nombre,Descripcion,IdMarca,IdCategoria,Precio)values(@Codigo, @Nombre ,@Descripcion,@Marca, @Categoria, @Precio)");
                data.setearConsulta("@Codigo", nuevo.Codigo);
                data.setearConsulta("@Nombre", nuevo.Nombre);
                data.setearConsulta("@Descripcion", nuevo.Descripcion);
                //uso el id, por que el enlace entre tables es mediante el Id.
                data.setearConsulta("@Marca", nuevo.Marca.Id);
                data.setearConsulta("@Categoria", nuevo.Categoria.Id);
                data.setearConsulta("@Precio", nuevo.Precio);
                data.ejecutarAccion();
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
