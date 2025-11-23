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
    public partial class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new FormClientes();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show(this);
        }

        private void seguridadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new FormSeguridad();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show(this);
        }

        private void empleadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new frmListaEmpleados();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog(this);
        }

        private void productosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new Pantallas_Sistema_facturaciónn.Formularios.frmProductos();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show(this);
        }
    }
}
