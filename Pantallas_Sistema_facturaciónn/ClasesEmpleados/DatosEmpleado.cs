using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pantallas_Sistema_facturaciónn.ClasesEmpleados
{
    public class DatosEmpleado
    {
        public int IdEmpleado { get; set; }
        public string Nombre { get; set; }
        public long NumDocumento { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public int? IdRolEmpleado { get; set; }
        public System.DateTime? DtmIngreso { get; set; }
        public System.DateTime? DtmRetiro { get; set; }
        public string DatosAdicionales { get; set; }
    }
}
