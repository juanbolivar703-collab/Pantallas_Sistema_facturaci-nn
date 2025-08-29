using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturaciónn
{
    public partial class FormClientes : Form
    {
        public FormClientes()
        {
            InitializeComponent();
        }

        private void Nombre_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            MessageBox.Show("Cliente actualizado correctamente.",
                            "Confirmación",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormClientes_Load(object sender, EventArgs e)
        {

        }

        private bool ValidarCampos()
        {
            bool ok = true;
            errorProvider1.Clear(); 

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                errorProvider1.SetError(txtNombre, "Ingrese el nombre del cliente");
                if (ok) txtNombre.Focus();
                ok = false;
            }

            if (string.IsNullOrWhiteSpace(txtDocumento.Text))
            {
                errorProvider1.SetError(txtDocumento, "Ingrese el documento");
                if (ok) txtDocumento.Focus();
                ok = false;
            }

            if (string.IsNullOrWhiteSpace(txtDireccion.Text))
            {
                errorProvider1.SetError(txtDireccion, "Ingrese la dirección");
                if (ok) txtDireccion.Focus();
                ok = false;
            }

            if (string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                errorProvider1.SetError(txtTelefono, "Ingrese el teléfono");
                if (ok) txtTelefono.Focus();
                ok = false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                errorProvider1.SetError(txtEmail, "Ingrese el email");
                if (ok) txtEmail.Focus();
                ok = false;
            }

            return ok;
        }


    }
}
