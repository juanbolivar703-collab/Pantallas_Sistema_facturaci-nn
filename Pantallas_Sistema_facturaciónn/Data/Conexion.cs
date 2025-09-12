using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace Pantallas_Sistema_facturaciónn.Data
{
    public static class Conexion
    {
        public static SqlConnection GetConnection()
        {
            string cn = ConfigurationManager.ConnectionStrings["cnFacturas"].ConnectionString;
            return new SqlConnection(cn);
        }
    }
}
