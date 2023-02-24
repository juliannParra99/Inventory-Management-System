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
        public frmAltaArticulo()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Articulo nuevoArticulo = new Articulo();

            try
            {
                nuevoArticulo.Codigo = txtCodigo.Text;
                nuevoArticulo.Nombre= txtNombre.Text;
                nuevoArticulo.Descripcion = txtDescripcion.Text;
                nuevoArticulo.Precio = decimal.Parse(txtPrecio.Text);

                nuevoArticulo.Categoria = (Categoria)cbxCategoria.SelectedItem;

                nuevoArticulo.Marca = (Marca)cbxMarca.SelectedItem;

                ArticuloBusiness businessArticulo = new ArticuloBusiness();
                businessArticulo.agregarArticulo(nuevoArticulo);

                MessageBox.Show("Agregado exitosamente");
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
                cbxMarca.DataSource = marcaBusiness.listarMarca(); 

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
    }
}
