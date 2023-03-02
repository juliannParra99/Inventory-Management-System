using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Domain;
using Business;

namespace Winform_app
{
    public partial class frmAltaArticulo : Form
    {
        private Articulo articulo = null;

        public frmAltaArticulo()
        {
            InitializeComponent();
        }
        //sobrecarga de constructor
        public frmAltaArticulo( Articulo articulo)
        {
            InitializeComponent();
            Text = "Modificar Articulo";
            this.articulo = articulo;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //se utiliza el atributo 'articulo' tanto para agregar como para modificar: mediante validacion
            //si no es nulo esta toco el boton 'modificar'
            ArticuloBusiness businessArticulo = new ArticuloBusiness();

            try
            {
                if (articulo == null)
                {
                    articulo = new Articulo();
                }
                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre= txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Precio = decimal.Parse(txtPrecio.Text);
                articulo.ImagenUrl = txtUrlImagen.Text;

                articulo.Categoria = (Categoria)cbxCategoria.SelectedItem;

                articulo.Marca = (Marca)cbxMarca.SelectedItem;

                //se valida con la propiedad Id: si el articulo existia, ya tenia un Id, por lo que es una modificacion.
                if (articulo.Id != 0)
                {
                    businessArticulo.modificarArticulo(articulo);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                    businessArticulo.agregarArticulo(articulo);
                    MessageBox.Show("Agregado exitosamente");
                }


                Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            
        }

        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
            CategoriaBusiness categoriaBusiness = new CategoriaBusiness();
            MarcaBusiness marcaBusiness = new MarcaBusiness();
            try
            {
                cbxCategoria.DataSource = categoriaBusiness.listarCategoria();
                cbxCategoria.ValueMember = "Id";
                cbxCategoria.DisplayMember = "Descripcion";
                cbxMarca.DataSource = marcaBusiness.listarMarca();
                cbxMarca.ValueMember = "Id";
                cbxMarca.DisplayMember = "Descripcion";

                if (articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtPrecio.Text = articulo.Precio.ToString();
                    txtUrlImagen.Text = articulo.ImagenUrl;
                    cargarImagen(articulo.ImagenUrl);
                    cbxCategoria.SelectedValue = articulo.Categoria.Id;
                    cbxMarca.SelectedValue = articulo.Marca.Id;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
        }

        //Metodo para cargar imagen
        private void cargarImagen(string imagen)
        {
            try
            {
                pbxArticulos.Load(imagen);

            }
            catch (Exception)
            {
                pbxArticulos.Load("https://developers.elementor.com/docs/assets/img/elementor-placeholder-image.png");

            }
        }
    }
}
