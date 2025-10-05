using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pantallas_Sistema_facturaciónn.ClasesEmpleados;

namespace Pantallas_Sistema_facturaciónn
{
    public partial class frmEmpleados : Form
    {


        private void label1_Click(object sender, EventArgs e)
        {

        }



        private readonly EmpleadosLogica logica = new EmpleadosLogica();
        private DatosEmpleado current;

        public frmEmpleados()
        {
            InitializeComponent();
            current = new DatosEmpleado();
        }

        public frmEmpleados(DatosEmpleado e) : this()
        {
            current = e;
            CargarCampos();
        }

        private void frmEmpleados_Load(object sender, EventArgs e)
        {
            try
            {
                cboRol.DataSource = logica.ListarRoles();
                cboRol.DisplayMember = "StrDescripcion";
                cboRol.ValueMember = "IdRolEmpleado";
            }
            catch
            {
            }

            dtpRetiro.Checked = false;
        }

        private void CargarCampos()
        {
            if (current == null) return;

            txtNombre.Text = current.Nombre ?? "";
            txtDocumento.Text = current.NumDocumento == 0 ? "" : current.NumDocumento.ToString();
            txtDireccion.Text = current.Direccion ?? "";
            txtTelefono.Text = current.Telefono ?? "";
            txtEmail.Text = current.Email ?? "";
            txtDatos.Text = current.DatosAdicionales ?? "";

            if (current.DtmIngreso.HasValue) dtpIngreso.Value = current.DtmIngreso.Value;
            if (current.DtmRetiro.HasValue)
            {
                dtpRetiro.Value = current.DtmRetiro.Value;
                dtpRetiro.Checked = true;
            }
            else
            {
                dtpRetiro.Checked = false;
            }

            if (current.IdRolEmpleado.HasValue)
            {
                try { cboRol.SelectedValue = current.IdRolEmpleado.Value; }
                catch { }
            }
        }

        private bool Validar()
        {
            err.Clear();
            bool ok = true;

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                err.SetError(txtNombre, "Requerido");
                ok = false;
            }

            if (!string.IsNullOrWhiteSpace(txtDocumento.Text))
            {
                if (!long.TryParse(txtDocumento.Text.Trim(), out _))
                {
                    err.SetError(txtDocumento, "Debe ser numérico");
                    ok = false;
                }
            }

            return ok;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Validar()) return;

            current.Nombre = txtNombre.Text.Trim();
            current.NumDocumento = string.IsNullOrWhiteSpace(txtDocumento.Text) ? 0 : Convert.ToInt64(txtDocumento.Text.Trim());
            current.Direccion = txtDireccion.Text.Trim();
            current.Telefono = txtTelefono.Text.Trim();
            current.Email = txtEmail.Text.Trim();
            current.DatosAdicionales = txtDatos.Text.Trim();
            current.DtmIngreso = dtpIngreso.Value;
            current.DtmRetiro = dtpRetiro.Checked ? (DateTime?)dtpRetiro.Value : null;
            current.IdRolEmpleado = cboRol.SelectedValue == null ? (int?)null : Convert.ToInt32(cboRol.SelectedValue);

            try
            {
                logica.Guardar(current, Environment.UserName);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
