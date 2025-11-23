using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Pantallas_Sistema_facturaciónn.ClasesProductos
{
    public class ProductosLogica
    {
        private readonly ProductosBD repo = new ProductosBD();

        public List<DatosProducto> Listar(string filtro = "") => repo.Listar(filtro);

        public DatosProducto Obtener(int id) => repo.GetById(id);

        public void Guardar(DatosProducto p, string usuarioModifica) => repo.Guardar(p, usuarioModifica);

        public void Eliminar(int id) => repo.Eliminar(id);

        public DataTable ListarCategorias() => repo.ListarCategorias();
    }
}
