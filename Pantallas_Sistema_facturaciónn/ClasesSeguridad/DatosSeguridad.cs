using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pantallas_Sistema_facturaciónn.ClasesSeguridad
{
    public class DatosSeguridad
    {
        public int IdSeguridad { get; set; }
        public int IdEmpleado { get; set; }
        public string StrUsuario { get; set; }
        public string StrClave { get; set; }
        public System.DateTime? DtmFechaModifica { get; set; }
        public string StrUsuarioModifico { get; set; }

    }
}
