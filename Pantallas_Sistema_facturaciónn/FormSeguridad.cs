using Pantallas_Sistema_facturaciónn.Data;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturaciónn
{
    public partial class FormSeguridad : Form
    {
        private int? _idSeguridadValidado = null;

        public FormSeguridad()
        {
            InitializeComponent();
        }

        private void FormSeguridad_Load(object sender, EventArgs e)
        {
            InicializarFormulario();
        }

        private void InicializarFormulario()
        {
            txtUsuarioActual.Text = "";
            txtClaveActual.Text = "";

            txtUsuarioNuevo.Text = "";
            txtClaveNueva.Text = "";
            txtUsuarioNuevo.Enabled = false;
            txtClaveNueva.Enabled = false;

            btnActualizar.Enabled = false;
            errorProvider1?.Clear();
            _idSeguridadValidado = null;

            txtUsuarioActual.Focus();
        }

        private bool ValidarAcceso()
        {
            bool ok = true;
            errorProvider1?.Clear();

            if (string.IsNullOrWhiteSpace(txtUsuarioActual.Text))
            {
                errorProvider1.SetError(txtUsuarioActual, "Ingrese el usuario actual");
                if (ok) txtUsuarioActual.Focus();
                ok = false;
            }

            if (string.IsNullOrWhiteSpace(txtClaveActual.Text))
            {
                errorProvider1.SetError(txtClaveActual, "Ingrese la contraseña actual");
                if (ok) txtClaveActual.Focus();
                ok = false;
            }

            return ok;
        }

        private void btnAcceder_Click(object sender, EventArgs e)
        {
            if (!ValidarAcceso()) return;

            string usuario = txtUsuarioActual.Text.Trim();
            string clave = txtClaveActual.Text.Trim();

            try
            {
                using (var cn = Conexion.GetConnection())
                using (var cmd = cn.CreateCommand())
                {
                    cn.Open();
                    cmd.CommandText = @"
                        SELECT TOP 1 IdSeguridad, StrUsuario, StrClave
                        FROM dbo.TBLSEGURIDAD
                        WHERE StrUsuario = @u AND StrClave = @p";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@u", usuario);
                    cmd.Parameters.AddWithValue("@p", clave);

                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            _idSeguridadValidado = rdr["IdSeguridad"] != DBNull.Value ? Convert.ToInt32(rdr["IdSeguridad"]) : (int?)null;

                            txtUsuarioNuevo.Text = rdr["StrUsuario"]?.ToString();
                            txtClaveNueva.Text = rdr["StrClave"]?.ToString();

                            txtUsuarioNuevo.Enabled = true;
                            txtClaveNueva.Enabled = true;
                            btnActualizar.Enabled = true;

                            MessageBox.Show("Acceso correcto. Ahora puede cambiar usuario y/o contraseña.", "Acceso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Usuario o contraseña actuales incorrectos.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            InicializarFormulario();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error de base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarNuevos()
        {
            bool ok = true;
            errorProvider1?.Clear();

            if (string.IsNullOrWhiteSpace(txtUsuarioNuevo.Text))
            {
                errorProvider1.SetError(txtUsuarioNuevo, "Ingrese el nuevo usuario");
                if (ok) txtUsuarioNuevo.Focus();
                ok = false;
            }

            if (string.IsNullOrWhiteSpace(txtClaveNueva.Text))
            {
                errorProvider1.SetError(txtClaveNueva, "Ingrese la nueva contraseña");
                if (ok) txtClaveNueva.Focus();
                ok = false;
            }

            return ok;
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (_idSeguridadValidado == null)
            {
                MessageBox.Show("Primero autentíquese con usuario y contraseña actuales (Acceder).", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidarNuevos()) return;

            string nuevoUsuario = txtUsuarioNuevo.Text.Trim();
            string nuevaClave = txtClaveNueva.Text.Trim();

            try
            {
                using (var cn = Conexion.GetConnection())
                using (var cmd = cn.CreateCommand())
                {
                    cn.Open();
                    cmd.CommandText = @"
                        UPDATE dbo.TBLSEGURIDAD
                        SET StrUsuario = @nu,
                            StrClave = @np,
                            DtmFechaModifica = GETDATE(),
                            StrUsuarioModifica = @editor
                        WHERE IdSeguridad = @id";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@nu", nuevoUsuario);
                    cmd.Parameters.AddWithValue("@np", nuevaClave);
                    cmd.Parameters.AddWithValue("@editor", Session.CurrentUser ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", _idSeguridadValidado.Value);

                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0)
                    {
                        MessageBox.Show("Usuario actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if (string.Equals(txtUsuarioActual.Text.Trim(), Session.CurrentUser, StringComparison.OrdinalIgnoreCase))
                            Session.CurrentUser = nuevoUsuario;

                        InicializarFormulario();
                    }
                    else
                    {
                        MessageBox.Show("No se realizó ninguna actualización.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error SQL: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
