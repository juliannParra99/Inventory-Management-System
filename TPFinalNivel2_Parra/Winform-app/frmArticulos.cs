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
        //se va utilizar varias veces este atributo cuando se requiera una lista
        private List<Articulo> articuloList;

        //atributos de items de comboBox
        private string marcaItemCbx;
        private string categoriaItemCbx;
        private string descripcionCbx;

        public frmArticulos()
        {
            InitializeComponent();
        }

        private void frmArticulos_Load(object sender, EventArgs e)
        {
            agregarArticulos();
            marcaItemCbx = "Brand";
            categoriaItemCbx = "Category";
            descripcionCbx = "Description";
            cbxCampo.Items.Add(marcaItemCbx); 
            cbxCampo.Items.Add(categoriaItemCbx);
            cbxCampo.Items.Add(descripcionCbx);

        }

        //metodo que centraliza el mostrar las listas de articulos.
        private void agregarArticulos()
        {
            ArticuloBusiness business = new ArticuloBusiness();
            try
            {
                articuloList = business.listarArticulo();
                dgvArticulos.DataSource = articuloList;
                //muestra los precios de la grilla solo con dos decimales
                decimalesPrecio();
                cargarImagen(articuloList[0].ImagenUrl);

                ocultarColumnas();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        //define la cantidad de decimales a mostrar
        private void decimalesPrecio()
        {
            dgvArticulos.Columns["Precio"].DefaultCellStyle.Format = "0.00";
        }

        //cambia la imagen cuando cambia la celda seleccionada
        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                //validacion por si se intenta leer algo que esta nulo en el DGV, como cuando se usa el filtro
                if (dgvArticulos.CurrentRow != null)
                {
                    Articulo seleccionadoRow;
                    seleccionadoRow = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    cargarImagen(seleccionadoRow.ImagenUrl);
                }
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
            try
            {
                articuloSeleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

                frmAltaArticulo modificar = new frmAltaArticulo(articuloSeleccionado);
                modificar.ShowDialog();
                agregarArticulos();

            }
            catch (Exception)
            {

                MessageBox.Show("Please select a record to modify.");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloBusiness business = new ArticuloBusiness();
            Articulo articuloSeleccionado;
            try
            {
                if (dgvArticulos.CurrentRow == null)
                {
                    MessageBox.Show("Please select a record to delete.");
                    return;
                }
                //se valida si realmente se quiere borrar el registro capturando el valor elegido en los botones del messageBox en la variable Resultado
                DialogResult resultado = MessageBox.Show("¿Are you sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (resultado == DialogResult.Yes)
                {
                    articuloSeleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    business.eliminar(articuloSeleccionado.Id);
                    agregarArticulos();

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = txtFiltro.Text;
            //se usa la lista declarada privada al inicio, para crear una nueva lista. Y se utiliza el filtro si 'filtro' no esta vacio
            try
            {
                if (filtro != "")
                {
                    listaFiltrada = articuloList.FindAll(x => x.Codigo.ToUpper().Contains(filtro.ToUpper()) || x.Nombre.ToUpper().Contains(filtro.ToUpper()));

                }
                else
                {
                    listaFiltrada = articuloList;
                }

                dgvArticulos.DataSource = null;
                dgvArticulos.DataSource = listaFiltrada;
                decimalesPrecio();
                ocultarColumnas();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void txtFiltroPrecio_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = txtFiltroPrecio.Text;

            try
            {
                if (!(validacionPrecio(filtro)))
                {
                    MessageBox.Show("Solo son validos numeros, y el signo de coma(,) para valores decimales.");
                }
                if (filtro != "" && validacionPrecio(filtro))
                {
                    listaFiltrada = articuloList.FindAll(x => x.Precio.ToString().ToUpper().Contains(filtro.ToUpper()));

                }
                else
                {
                    listaFiltrada = articuloList;
                }

                dgvArticulos.DataSource = null;
                dgvArticulos.DataSource = listaFiltrada;
                decimalesPrecio();
                ocultarColumnas();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void cbxCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string seleccionado = cbxCampo.SelectedItem.ToString();

            try
            {
                if (seleccionado == marcaItemCbx || seleccionado == categoriaItemCbx || seleccionado == descripcionCbx)
                {
                    cbxCriterio.Items.Clear();
                    cbxCriterio.Items.Add("Starts with");
                    cbxCriterio.Items.Add("Ends with");
                    cbxCriterio.Items.Add("Contains");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private bool validarFiltro()
        {
            
            if (cbxCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Please select the field to filter.");
                return true;
            }
            if (cbxCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Select the filter criteria.");
                return true;
            }
            
            return false;
        }

        
        private bool validacionPrecio(string cadena)
        {
            foreach (char x in cadena)
            {
                if (!(char.IsNumber(x) || x == ',' ))
                {
                    return false;
                }
            }
            return true;
        }
        

        
        private void btnFiltroAvanzado_Click(object sender, EventArgs e)
        {
            ArticuloBusiness business = new ArticuloBusiness();

            try
            {
                if (validarFiltro())
                    return;
                string campo = cbxCampo.SelectedItem.ToString();
                string criterio = cbxCampo.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;

                dgvArticulos.DataSource = business.filtroAvanzado(campo,criterio, filtro);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        
    }
}
