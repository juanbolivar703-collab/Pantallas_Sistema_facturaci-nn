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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnValidar_Click(object sender, EventArgs e)
        {
            bool camposValidos = true;

            errorProvider1.SetError(txtUsuario, "");
            errorProvider1.SetError(txtContrasena, "");

            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                errorProvider1.SetError(txtUsuario, "Ingrese un usuario");
                camposValidos = false;
            }

            if (string.IsNullOrWhiteSpace(txtContrasena.Text))
            {
                errorProvider1.SetError(txtContrasena, "Ingrese una contraseña");
                camposValidos = false;
            }

            if (camposValidos)
            {
                FormPrincipal frm = new FormPrincipal();
                frm.Show();
                this.Hide();
            }
        }
        

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Cierra toda la aplicación

        }
    }
}
