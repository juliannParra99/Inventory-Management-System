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
                data.setearConsulta("select A.Codigo,A.Nombre, A.Descripcion, A.ImagenUrl, M.Descripcion marca, C.Descripcion categoria, A.Precio,A.IdCategoria,A.IdMarca,A.Id from Articulos as A , MARCAS as M, CATEGORIAS as C where A.IdMarca = M.Id and A.IdCategoria = C.Id");
                data.ejecutarLectura();

                //si hay un registro,una fila, va a recorrer los valores de esa fila, osea las columnas.
                while (data.Reader.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)data.Reader["Id"];
                    aux.Codigo = (string)data.Reader["Codigo"];
                    aux.Nombre = (string)data.Reader["Nombre"];
                    aux.Descripcion = (string)data.Reader["Descripcion"];

                    //validacion dbNull de column imagenUrl: solo lee si no esta nula el registro en la columna
                    if (!(data.Reader["ImagenUrl"] is DBNull))
                    {
                        aux.ImagenUrl = (string)data.Reader["ImagenUrl"];
                    }

                    aux.Precio = (decimal)data.Reader["Precio"];

                    //se agregan id de marca y categoria
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)data.Reader["IdMarca"];
                    aux.Marca.Descripcion = (string)data.Reader["marca"];

                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)data.Reader["IdCategoria"];
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
                data.setearConsulta("insert into ARTICULOS (Codigo,Nombre,Descripcion,IdMarca,IdCategoria,Precio, ImagenUrl)values(@Codigo, @Nombre ,@Descripcion,@Marca, @Categoria, @Precio, @UrlImagen)");
                data.setearParametro("@Codigo", nuevo.Codigo);
                data.setearParametro("@Nombre", nuevo.Nombre);
                data.setearParametro("@Descripcion", nuevo.Descripcion);
                //uso el id, por que el enlace entre tables es mediante el Id.
                data.setearParametro("@Marca", nuevo.Marca.Id);
                data.setearParametro("@Categoria", nuevo.Categoria.Id);
                data.setearParametro("@Precio", nuevo.Precio);
                data.setearParametro("@UrlImagen", nuevo.ImagenUrl);
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

        public void modificarArticulo(Articulo articulo)
        {
            DataAccess data = new DataAccess();

            try
            {
                data.setearConsulta("update ARTICULOS set Codigo = @cod, Nombre = @nom, Descripcion = @desc, IdMarca = @idMarca, IdCategoria = @idCategoria , ImagenUrl = @Img , Precio = @Precio where Id = @id");
                data.setearParametro("@cod", articulo.Codigo);
                data.setearParametro("@nom", articulo.Nombre);
                data.setearParametro("@desc", articulo.Descripcion);
                data.setearParametro("@idMarca", articulo.Marca.Id);
                data.setearParametro("@idCategoria", articulo.Categoria.Id);
                data.setearParametro("@Img", articulo.ImagenUrl);
                data.setearParametro("@Precio", articulo.Precio);
                data.setearParametro("@id", articulo.Id);

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

        public void eliminar(int id)
        {
            try
            {
                DataAccess data = new DataAccess();
                data.setearConsulta("delete from  articulos where id = @id");
                data.setearParametro("@id",id);
                data.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Articulo> filtroAvanzado(string campo, string criterio, string filtro)
        {
            List<Articulo> listaArticulos = new List<Articulo>();

            DataAccess data = new DataAccess();

            //logica que indica que si se selecciona un campo, se pueda acceder a los criterios correspondientes.
            try
            {
                string consulta = "select A.Codigo,A.Nombre, A.Descripcion, A.ImagenUrl, M.Descripcion marca, C.Descripcion categoria, A.Precio,A.IdCategoria,A.IdMarca,A.Id from Articulos as A , MARCAS as M, CATEGORIAS as C where A.IdMarca = M.Id and A.IdCategoria = C.Id And ";


                if (campo == "Precio")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "A.Precio >" +  filtro;
                            break;
                        case "Menor a":
                            consulta += "A.Precio <" + filtro;
                            break;
                        default:
                            consulta += "A.Precio =" + filtro;
                            break;
                    }
                }
                else if (campo == "Marca")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "M.Descripcion like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "M.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "M.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Categoria")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += " C.Descripcion like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "C.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "C.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "A.Descripcion like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "A.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "A.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }


                data.setearConsulta(consulta);
                data.ejecutarLectura();

                while (data.Reader.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)data.Reader["Id"];
                    aux.Codigo = (string)data.Reader["Codigo"];
                    aux.Nombre = (string)data.Reader["Nombre"];
                    aux.Descripcion = (string)data.Reader["Descripcion"];

                    //validacion dbNull de column imagenUrl: solo lee si no esta nula el registro en la columna
                    if (!(data.Reader["ImagenUrl"] is DBNull))
                    {
                        aux.ImagenUrl = (string)data.Reader["ImagenUrl"];
                    }

                    aux.Precio = (decimal)data.Reader["Precio"];

                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)data.Reader["IdMarca"];
                    aux.Marca.Descripcion = (string)data.Reader["marca"];

                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)data.Reader["IdCategoria"];
                    aux.Categoria.Descripcion = (string)data.Reader["categoria"];


                    listaArticulos.Add(aux);
                }

                return listaArticulos;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                data.cerrarConexion();
            }
            
        }


    }
}
