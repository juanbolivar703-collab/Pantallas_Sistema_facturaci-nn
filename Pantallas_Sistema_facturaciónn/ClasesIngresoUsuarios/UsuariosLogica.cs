using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pantallas_Sistema_facturaciónn.Clases
{
    public class UsuariosLogica
    {
        private readonly UsuariosBD repo = new UsuariosBD();

        public DatosUsuario Validar(string usu, string clave)
        {
            return repo.BuscarUsuario(usu, clave);
        }
    }
}
