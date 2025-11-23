using Pantallas_Sistema_facturaciónn.ClasesProductos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturaciónn.Formularios
{
    public partial class frmProductos : Form
    {
        private readonly ProductosLogica logica = new ProductosLogica();
        private int idProducto = 0; 

        public frmProductos()
        {
            InitializeComponent();
            CargarCategorias();
        }

        public frmProductos(int id)
        {
            InitializeComponent();
            idProducto = id;
            CargarCategorias();
            CargarProducto();
        }

        private void CargarCategorias()
        {
            DataTable dt = logica.ListarCategorias();
            cmbCategoria.DataSource = dt;
            cmbCategoria.DisplayMember = "StrDescripcion";
            cmbCategoria.ValueMember = "IdCategoria";
        }

        private void CargarProducto()
        {
            if (idProducto <= 0) return;

            var p = logica.Obtener(idProducto);
            if (p == null) return;

            txtNombre.Text = p.Nombre;
            txtCodigo.Text = p.CodigoReferencia;
            txtPrecioCompra.Text = p.PrecioCompra.ToString();
            txtPrecioVenta.Text = p.PrecioVenta.ToString();
            txtStock.Text = p.CantidadStock.ToString();
            txtRutaImagen.Text = p.RutaImagen;
            txtDetalles.Text = p.Detalles;

            if (p.IdCategoria.HasValue)
                cmbCategoria.SelectedValue = p.IdCategoria.Value;
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                ProductosLogica logica = new ProductosLogica();

                int? categoria = null;
                if (cmbCategoria.SelectedValue != null)
                {
                    categoria = Convert.ToInt32(cmbCategoria.SelectedValue);
                }

                DatosProducto p = new DatosProducto
                {
                    IdProducto = idProducto,
                    Nombre = txtNombre.Text,
                    CodigoReferencia = txtCodigo.Text,
                    PrecioCompra = decimal.Parse(txtPrecioCompra.Text),
                    PrecioVenta = decimal.Parse(txtPrecioVenta.Text),
                    CantidadStock = int.Parse(txtStock.Text),
                    RutaImagen = txtRutaImagen.Text,
                    Detalles = txtDetalles.Text,
                    IdCategoria = categoria
                };

                logica.Guardar(p, "Juanjo");

                MessageBox.Show("Producto guardado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private bool ValidarCampos()
        {
            if (txtNombre.Text.Trim() == "")
            {
                MessageBox.Show("El nombre es obligatorio.");
                return false;
            }

            if (txtCodigo.Text.Trim() == "")
            {
                MessageBox.Show("El código de referencia es obligatorio.");
                return false;
            }

            if (!decimal.TryParse(txtPrecioCompra.Text, out _))
            {
                MessageBox.Show("Precio de compra inválido.");
                return false;
            }

            if (!decimal.TryParse(txtPrecioVenta.Text, out _))
            {
                MessageBox.Show("Precio de venta inválido.");
                return false;
            }

            if (!int.TryParse(txtStock.Text, out _))
            {
                MessageBox.Show("Cantidad en stock inválida.");
                return false;
            }

            return true;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmProductos_Load(object sender, EventArgs e)
        {

        }
    }
}
