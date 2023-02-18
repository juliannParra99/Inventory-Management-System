using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Articulo
    {
        public int Id{ get; set; }
        [DisplayName("Code")]
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Marca IdMarca { get; set; }
        public Categoria IdCategoria { get; set; }
        public string ImagenUrl { get; set; }
        public float Precio { get; set; }
    }
}
