using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Pantallas_Sistema_facturaciónn.ClasesProductos
{
    public class ProductosBD
    {
        private readonly string cadena = ConfigurationManager.ConnectionStrings["cnFacturas"].ConnectionString;

        public List<DatosProducto> Listar(string filtro = "")
        {
            var lista = new List<DatosProducto>();

            using (var cn = new SqlConnection(cadena))
            using (var cmd = new SqlCommand(@"
SELECT IdProducto, Nombre, CodigoReferencia, PrecioCompra, PrecioVenta,
       CantidadStock, RutaImagen, Detalles, IdCategoria
FROM TBLPRODUCTOS
WHERE (Nombre + ' ' + CodigoReferencia) LIKE @filtro", cn))
            {
                cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");

                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new DatosProducto
                        {
                            IdProducto = Convert.ToInt32(dr["IdProducto"]),
                            Nombre = dr["Nombre"].ToString(),
                            CodigoReferencia = dr["CodigoReferencia"].ToString(),
                            PrecioCompra = Convert.ToDecimal(dr["PrecioCompra"]),
                            PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]),
                            CantidadStock = Convert.ToInt32(dr["CantidadStock"]),
                            RutaImagen = dr["RutaImagen"] == DBNull.Value ? "" : dr["RutaImagen"].ToString(),
                            Detalles = dr["Detalles"] == DBNull.Value ? "" : dr["Detalles"].ToString(),
                            IdCategoria = dr["IdCategoria"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["IdCategoria"])
                        });
                    }
                }
            }

            return lista;
        }

        public DatosProducto GetById(int id)
        {
            using (var cn = new SqlConnection(cadena))
            using (var cmd = new SqlCommand(@"
SELECT IdProducto, Nombre, CodigoReferencia, PrecioCompra, PrecioVenta,
       CantidadStock, RutaImagen, Detalles, IdCategoria
FROM TBLPRODUCTOS
WHERE IdProducto=@id", cn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new DatosProducto
                        {
                            IdProducto = Convert.ToInt32(dr["IdProducto"]),
                            Nombre = dr["Nombre"].ToString(),
                            CodigoReferencia = dr["CodigoReferencia"].ToString(),
                            PrecioCompra = Convert.ToDecimal(dr["PrecioCompra"]),
                            PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]),
                            CantidadStock = Convert.ToInt32(dr["CantidadStock"]),
                            RutaImagen = dr["RutaImagen"] == DBNull.Value ? "" : dr["RutaImagen"].ToString(),
                            Detalles = dr["Detalles"] == DBNull.Value ? "" : dr["Detalles"].ToString(),
                            IdCategoria = dr["IdCategoria"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["IdCategoria"])
                        };
                    }
                }
            }

            return null;
        }

        public void Guardar(DatosProducto p, string usuarioModifica)
        {
            using (var cn = new SqlConnection(cadena))
            using (var cmd = new SqlCommand("actualizar_Producto", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdProducto", p.IdProducto);
                cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
                cmd.Parameters.AddWithValue("@CodigoReferencia", p.CodigoReferencia);
                cmd.Parameters.AddWithValue("@PrecioCompra", p.PrecioCompra);
                cmd.Parameters.AddWithValue("@PrecioVenta", p.PrecioVenta);
                cmd.Parameters.AddWithValue("@CantidadStock", p.CantidadStock);
                cmd.Parameters.AddWithValue("@RutaImagen", (object)p.RutaImagen ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Detalles", (object)p.Detalles ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IdCategoria", p.IdCategoria.HasValue ? (object)p.IdCategoria : DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaModifica", DateTime.Now);
                cmd.Parameters.AddWithValue("@UsuarioModifico", usuarioModifica);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Eliminar(int id)
        {
            using (var cn = new SqlConnection(cadena))
            using (var cmd = new SqlCommand("Eliminar_Producto", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdProducto", id);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable ListarCategorias()
        {
            var dt = new DataTable();

            using (var cn = new SqlConnection(cadena))
            using (var cmd = new SqlCommand("SELECT IdCategoria, StrDescripcion FROM TBLCATEGORIAS", cn))
            using (var da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }

            return dt;
        }
    }
}
