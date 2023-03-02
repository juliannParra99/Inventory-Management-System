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
    public partial class frmArticulos : Form
    {
        //se va utilizar varias veces este atributo

        private List<Articulo> articuloList;
        public frmArticulos()
        {
            InitializeComponent();
        }

        private void frmArticulos_Load(object sender, EventArgs e)
        {
            agregarArticulos();
        }

        //metodo que centraliza el mostrar las listas de articulos.
        private void agregarArticulos()
        {
            ArticuloBusiness business = new ArticuloBusiness();
            try
            {
                articuloList = business.listarArticulo();
                dgvArticulos.DataSource = articuloList;
                cargarImagen(articuloList[0].ImagenUrl);
                ocultarColumnas();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        //cambia la imagen cuando cambia la celda seleccionada
        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                Articulo seleccionadoRow;
                seleccionadoRow = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionadoRow.ImagenUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                
            }
            
        }

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

        private void ocultarColumnas()
        {
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
            dgvArticulos.Columns["Id"].Visible = false;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaArticulo alta = new frmAltaArticulo();
            alta.ShowDialog();
            agregarArticulos();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            //obtiene el objeto de la fila seleccionada  de la dgvArticulos 
            Articulo articuloSeleccionado;
            articuloSeleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

            frmAltaArticulo modificar = new frmAltaArticulo(articuloSeleccionado);
            modificar.ShowDialog();
            agregarArticulos();
        }
    }
}
