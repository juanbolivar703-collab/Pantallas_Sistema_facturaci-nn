using Pantallas_Sistema_facturaciónn.ClasesEmpleados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Pantallas_Sistema_facturaciónn.ClasesEmpleados
{
    public class EmpleadosLogica
    {
        private readonly EmpleadosBD repo = new EmpleadosBD();

        public List<DatosEmpleado> Listar(string filtro = "") => repo.Listar(filtro);

        public DatosEmpleado Obtener(int id) => repo.GetById(id);

        public void Guardar(DatosEmpleado e, string usuarioModifica) => repo.Guardar(e, usuarioModifica);

        public void Eliminar(int id) => repo.Eliminar(id);

        public DataTable ListarRoles() => repo.ListarRoles();
    }
}
