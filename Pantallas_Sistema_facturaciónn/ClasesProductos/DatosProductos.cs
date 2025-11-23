using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pantallas_Sistema_facturaciónn.ClasesProductos
{
    public class DatosProducto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string CodigoReferencia { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public int CantidadStock { get; set; }
        public string RutaImagen { get; set; }
        public string Detalles { get; set; }
        public int? IdCategoria { get; set; }
    }
}
