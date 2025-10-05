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
using Pantallas_Sistema_facturaciónn.ClasesEmpleados;


namespace Pantallas_Sistema_facturaciónn
{
    public partial class frmListaEmpleados : Form
    {

        private readonly EmpleadosLogica logica = new EmpleadosLogica();


        private void Cargar(string filtro = "")
        {
            dgvEmpleados.DataSource = logica.Listar(filtro);
            if (dgvEmpleados.Columns["IdEmpleado"] != null)
                dgvEmpleados.Columns["IdEmpleado"].Visible = false; // oculta la PK
            dgvEmpleados.AutoResizeColumns();
        }



        public frmListaEmpleados()
        {
            InitializeComponent();
        }


        private void frmListaEmpleados_Load(object sender, EventArgs e)
        {
            Cargar();
        }



        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            Cargar(txtBuscar.Text.Trim());

        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            var f = new frmEmpleados();
            if (f.ShowDialog() == DialogResult.OK) Cargar();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvEmpleados.CurrentRow == null) return;
            var emp = (DatosEmpleado)dgvEmpleados.CurrentRow.DataBoundItem; 
            var f = new frmEmpleados(emp); 
            if (f.ShowDialog() == DialogResult.OK) Cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvEmpleados.CurrentRow == null) return;
            var emp = (DatosEmpleado)dgvEmpleados.CurrentRow.DataBoundItem;
            if (MessageBox.Show($"¿Eliminar a {emp.Nombre}?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                logica.Eliminar(emp.IdEmpleado);
                Cargar();
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvEmpleados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
