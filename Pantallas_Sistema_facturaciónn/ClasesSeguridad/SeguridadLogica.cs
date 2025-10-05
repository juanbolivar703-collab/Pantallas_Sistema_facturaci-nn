using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pantallas_Sistema_facturaciónn.ClasesSeguridad
{
    public class SeguridadLogica
    {
        private readonly SeguridadBD repo = new SeguridadBD();

        public List<DatosSeguridad> Listar() => repo.Listar();

        public DatosSeguridad ObtenerPorIdSeguridad(int id) => repo.GetByIdSeguridad(id);

        public DatosSeguridad Authenticate(string usuario, string clave) => repo.GetByUsernameAndPassword(usuario, clave);

        public void UpdateCredentialsByIdSeguridad(int idSeguridad, string nuevoUsuario, string nuevaClave, string usuarioModifico)
            => repo.UpdateCredentialsByIdSeguridad(idSeguridad, nuevoUsuario, nuevaClave, usuarioModifico);

        public void EliminarPorEmpleado(int idEmpleado) => repo.EliminarPorEmpleado(idEmpleado);
    }
}
