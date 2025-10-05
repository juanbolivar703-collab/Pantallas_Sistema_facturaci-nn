using Pantallas_Sistema_facturaciónn.Data;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using Pantallas_Sistema_facturaciónn.ClasesSeguridad;


namespace Pantallas_Sistema_facturaciónn
{
    public partial class FormSeguridad : Form
    {

        private readonly SeguridadLogica seguridad = new SeguridadLogica();


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
                var u = seguridad.Authenticate(usuario, clave);
                if (u != null)
                {
                    _idSeguridadValidado = u.IdSeguridad;
                    txtUsuarioNuevo.Text = u.StrUsuario;
                    txtClaveNueva.Text = u.StrClave;
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
            catch (System.Exception ex)
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
                seguridad.UpdateCredentialsByIdSeguridad(_idSeguridadValidado.Value, nuevoUsuario, nuevaClave, Session.CurrentUser ?? Environment.UserName);
                MessageBox.Show("Usuario actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (string.Equals(txtUsuarioActual.Text.Trim(), Session.CurrentUser, System.StringComparison.OrdinalIgnoreCase))
                    Session.CurrentUser = nuevoUsuario;

                InicializarFormulario();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error al actualizar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
