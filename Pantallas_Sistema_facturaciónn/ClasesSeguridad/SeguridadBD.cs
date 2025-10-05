using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Pantallas_Sistema_facturaciónn.ClasesSeguridad
{
    public class SeguridadBD
    {
        private readonly string cadena = ConfigurationManager.ConnectionStrings["cnFacturas"].ConnectionString;

        public List<DatosSeguridad> Listar()
        {
            var lista = new List<DatosSeguridad>();
            using (var cn = new SqlConnection(cadena))
            using (var cmd = new SqlCommand("SELECT IdSeguridad, IdEmpleado, StrUsuario, StrClave, DtmFechaModifica, StrUsuarioModifico FROM TBLSEGURIDAD", cn))
            {
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new DatosSeguridad
                        {
                            IdSeguridad = Convert.ToInt32(dr["IdSeguridad"]),
                            IdEmpleado = dr["IdEmpleado"] == DBNull.Value ? 0 : Convert.ToInt32(dr["IdEmpleado"]),
                            StrUsuario = dr["StrUsuario"] == DBNull.Value ? "" : dr["StrUsuario"].ToString(),
                            StrClave = dr["StrClave"] == DBNull.Value ? "" : dr["StrClave"].ToString(),
                            DtmFechaModifica = dr["DtmFechaModifica"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["DtmFechaModifica"]),
                            StrUsuarioModifico = dr["StrUsuarioModifico"] == DBNull.Value ? "" : dr["StrUsuarioModifico"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        public DatosSeguridad GetByIdSeguridad(int idSeguridad)
        {
            using (var cn = new SqlConnection(cadena))
            using (var cmd = new SqlCommand("SELECT IdSeguridad, IdEmpleado, StrUsuario, StrClave, DtmFechaModifica, StrUsuarioModifico FROM TBLSEGURIDAD WHERE IdSeguridad=@id", cn))
            {
                cmd.Parameters.AddWithValue("@id", idSeguridad);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new DatosSeguridad
                        {
                            IdSeguridad = Convert.ToInt32(dr["IdSeguridad"]),
                            IdEmpleado = dr["IdEmpleado"] == DBNull.Value ? 0 : Convert.ToInt32(dr["IdEmpleado"]),
                            StrUsuario = dr["StrUsuario"] == DBNull.Value ? "" : dr["StrUsuario"].ToString(),
                            StrClave = dr["StrClave"] == DBNull.Value ? "" : dr["StrClave"].ToString(),
                            DtmFechaModifica = dr["DtmFechaModifica"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["DtmFechaModifica"]),
                            StrUsuarioModifico = dr["StrUsuarioModifico"] == DBNull.Value ? "" : dr["StrUsuarioModifico"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public DatosSeguridad GetByUsernameAndPassword(string usuario, string clave)
        {
            using (var cn = new SqlConnection(cadena))
            using (var cmd = new SqlCommand(@"SELECT TOP 1 IdSeguridad, IdEmpleado, StrUsuario, StrClave, DtmFechaModifica, StrUsuarioModifico
                                             FROM TBLSEGURIDAD WHERE StrUsuario=@u AND StrClave=@p", cn))
            {
                cmd.Parameters.AddWithValue("@u", usuario);
                cmd.Parameters.AddWithValue("@p", clave);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new DatosSeguridad
                        {
                            IdSeguridad = Convert.ToInt32(dr["IdSeguridad"]),
                            IdEmpleado = dr["IdEmpleado"] == DBNull.Value ? 0 : Convert.ToInt32(dr["IdEmpleado"]),
                            StrUsuario = dr["StrUsuario"] == DBNull.Value ? "" : dr["StrUsuario"].ToString(),
                            StrClave = dr["StrClave"] == DBNull.Value ? "" : dr["StrClave"].ToString(),
                            DtmFechaModifica = dr["DtmFechaModifica"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["DtmFechaModifica"]),
                            StrUsuarioModifico = dr["StrUsuarioModifico"] == DBNull.Value ? "" : dr["StrUsuarioModifico"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public void UpdateCredentialsByIdSeguridad(int idSeguridad, string nuevoUsuario, string nuevaClave, string usuarioModifico)
        {
            using (var cn = new SqlConnection(cadena))
            using (var cmd = new SqlCommand(@"
                UPDATE dbo.TBLSEGURIDAD
                SET StrUsuario = @u,
                    StrClave = @p,
                    DtmFechaModifica = GETDATE(),
                    StrUsuarioModifico = @editor
                WHERE IdSeguridad = @id", cn))
            {
                cmd.Parameters.AddWithValue("@u", (object)nuevoUsuario ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p", (object)nuevaClave ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@editor", (object)usuarioModifico ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@id", idSeguridad);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void EliminarPorEmpleado(int idEmpleado)
        {
            using (var cn = new SqlConnection(cadena))
            using (var cmd = new SqlCommand("Eliminar_Seguridad", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdEmpleado", idEmpleado);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
