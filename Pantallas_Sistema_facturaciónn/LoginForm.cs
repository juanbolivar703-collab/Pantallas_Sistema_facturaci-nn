using Pantallas_Sistema_facturaciónn.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pantallas_Sistema_facturaciónn.Clases;


namespace Pantallas_Sistema_facturaciónn
{
    public partial class LoginForm : Form
    {

        private readonly UsuariosLogica logica = new UsuariosLogica();


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
            errorProvider1.SetError(txtUsuario, "");
            errorProvider1.SetError(txtContrasena, "");

            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                errorProvider1.SetError(txtUsuario, "Ingrese un usuario");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtContrasena.Text))
            {
                errorProvider1.SetError(txtContrasena, "Ingrese una contraseña");
                return;
            }

            var usuario = logica.Validar(txtUsuario.Text.Trim(), txtContrasena.Text.Trim());
            if (usuario != null)
            {
                Session.CurrentUser = txtUsuario.Text.Trim();
                var frm = new FormPrincipal();
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos", "Error de autenticación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }




        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Application.Exit(); 

        }

        private void txtUsuario_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtContrasena_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
