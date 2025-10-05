using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Pantallas_Sistema_facturaciónn.ClasesEmpleados
{
    public class EmpleadosBD
    {
        private readonly string cadena = ConfigurationManager.ConnectionStrings["cnFacturas"].ConnectionString;

        public List<DatosEmpleado> Listar(string filtro = "")
        {
            var lista = new List<DatosEmpleado>();
            using (var cn = new SqlConnection(cadena))
            using (var cmd = new SqlCommand(@"SELECT IdEmpleado, strNombre, NumDocumento, StrDireccion, StrTelefono, StrEmail, IdRolEmpleado, DtmIngreso, DtmRetiro, strDatosAdicionales FROM TBLEMPLEADO
WHERE (strNombre + ' ' + ISNULL(StrDireccion,'')) LIKE @filtro", cn))
            {
                cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new DatosEmpleado
                        {
                            IdEmpleado = Convert.ToInt32(dr["IdEmpleado"]),
                            Nombre = dr["strNombre"].ToString(),
                            NumDocumento = dr["NumDocumento"] == DBNull.Value ? 0 : Convert.ToInt64(dr["NumDocumento"]),
                            Direccion = dr["StrDireccion"] == DBNull.Value ? "" : dr["StrDireccion"].ToString(),
                            Telefono = dr["StrTelefono"] == DBNull.Value ? "" : dr["StrTelefono"].ToString(),
                            Email = dr["StrEmail"] == DBNull.Value ? "" : dr["StrEmail"].ToString(),
                            IdRolEmpleado = dr["IdRolEmpleado"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["IdRolEmpleado"]),
                            DtmIngreso = dr["DtmIngreso"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["DtmIngreso"]),
                            DtmRetiro = dr["DtmRetiro"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["DtmRetiro"]),
                            DatosAdicionales = dr["strDatosAdicionales"] == DBNull.Value ? "" : dr["strDatosAdicionales"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        public DatosEmpleado GetById(int id)
        {
            using (var cn = new SqlConnection(cadena))
            using (var cmd = new SqlCommand("SELECT IdEmpleado, strNombre, NumDocumento, StrDireccion, StrTelefono, StrEmail, IdRolEmpleado, DtmIngreso, DtmRetiro, strDatosAdicionales FROM TBLEMPLEADO WHERE IdEmpleado=@id", cn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new DatosEmpleado
                        {
                            IdEmpleado = Convert.ToInt32(dr["IdEmpleado"]),
                            Nombre = dr["strNombre"].ToString(),
                            NumDocumento = dr["NumDocumento"] == DBNull.Value ? 0 : Convert.ToInt64(dr["NumDocumento"]),
                            Direccion = dr["StrDireccion"] == DBNull.Value ? "" : dr["StrDireccion"].ToString(),
                            Telefono = dr["StrTelefono"] == DBNull.Value ? "" : dr["StrTelefono"].ToString(),
                            Email = dr["StrEmail"] == DBNull.Value ? "" : dr["StrEmail"].ToString(),
                            IdRolEmpleado = dr["IdRolEmpleado"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["IdRolEmpleado"]),
                            DtmIngreso = dr["DtmIngreso"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["DtmIngreso"]),
                            DtmRetiro = dr["DtmRetiro"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["DtmRetiro"]),
                            DatosAdicionales = dr["strDatosAdicionales"] == DBNull.Value ? "" : dr["strDatosAdicionales"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public void Guardar(DatosEmpleado e, string usuarioModifica)
        {
            using (var cn = new SqlConnection(cadena))
            using (var cmd = new SqlCommand("actualizar_Empleado", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdEmpleado", e.IdEmpleado);
                cmd.Parameters.AddWithValue("@strNombre", e.Nombre ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@NumDocumento", e.NumDocumento);
                cmd.Parameters.AddWithValue("@StrDireccion", e.Direccion ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@StrTelefono", e.Telefono ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@StrEmail", e.Email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@IdRolEmpleado", e.IdRolEmpleado ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DtmIngreso", e.DtmIngreso ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DtmRetiro", e.DtmRetiro ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@strDatosAdicionales", e.DatosAdicionales ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DtmFechaModifica", DateTime.Now);
                cmd.Parameters.AddWithValue("@StrUsuarioModifico", usuarioModifica);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Eliminar(int id)
        {
            using (var cn = new SqlConnection(cadena))
            using (var cmd = new SqlCommand("Eliminar_Empleado", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdEmpleado", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable ListarRoles()
        {
            var dt = new DataTable();
            using (var cn = new SqlConnection(cadena))
            using (var cmd = new SqlCommand("SELECT IdRolEmpleado, StrDescripcion FROM TBLROLES", cn))
            {
                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }
    }
}