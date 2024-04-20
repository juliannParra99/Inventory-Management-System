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
        public int Id { get; set; }
        [DisplayName("Code")]
        public string Codigo { get; set; }
        [DisplayName("Name")]
        public string Nombre { get; set; }
        [DisplayName("Description")]
        public string Descripcion { get; set; }
        [DisplayName("Brand")]
        public Marca Marca { get; set; }
        [DisplayName("Category")]
        public Categoria Categoria { get; set; }
        public string ImagenUrl { get; set; }
        [DisplayName("Price")]
        public decimal Precio { get; set; }
    }
}
