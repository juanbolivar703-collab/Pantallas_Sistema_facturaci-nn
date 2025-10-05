using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;



namespace Pantallas_Sistema_facturaciónn.Clases
{
    public class UsuariosBD
    {
        private readonly string cadena = ConfigurationManager.ConnectionStrings["cnFacturas"].ConnectionString;

        public DatosUsuario BuscarUsuario(string usu, string clave)
        {
            using (var cn = new SqlConnection(cadena))
            using (var cmd = new SqlCommand("SELECT IdSeguridad, IdEmpleado, StrUsuario, StrClave FROM TBLSEGURIDAD WHERE StrUsuario=@u AND StrClave=@c", cn))
            {
                cmd.Parameters.AddWithValue("@u", usu);
                cmd.Parameters.AddWithValue("@c", clave);
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new DatosUsuario
                        {
                            Id = Convert.ToInt32(dr["IdSeguridad"]),
                            IdEmpleado = dr["IdEmpleado"] == DBNull.Value ? 0 : Convert.ToInt32(dr["IdEmpleado"]),
                            Usuario = dr["StrUsuario"].ToString(),
                            Clave = dr["StrClave"].ToString()
                        };
                    }
                }
            }
            return null;
        }
    }
}