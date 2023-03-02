﻿using System;
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
            articuloSeleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

            frmAltaArticulo modificar = new frmAltaArticulo(articuloSeleccionado);
            modificar.ShowDialog();
            agregarArticulos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloBusiness business = new ArticuloBusiness();
            Articulo articuloSeleccionado;
            try
            {
                //se valida si realmente se quiere borrar el registro capturando el valor elegido en los botones del messageBox en la variable Resultado
                DialogResult resultado = MessageBox.Show("¿Estas seguro que queres eliminar este registro?", "Eliminar Registro", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

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
                    listaFiltrada = articuloList.FindAll(x => x.Codigo.ToUpper().Contains(filtro.ToUpper()));

                }
                else
                {
                    listaFiltrada = articuloList;
                }

                dgvArticulos.DataSource = null;
                dgvArticulos.DataSource = listaFiltrada;
                ocultarColumnas();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
