using Pantallas_Sistema_facturaciónn.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Pantallas_Sistema_facturaciónn
{
    public partial class FormClientes : Form
    {
        public FormClientes()
        {
            InitializeComponent();
        }






        private void FormClientes_Load(object sender, EventArgs e)
        {
            CargarClientes();
            LimpiarCampos();
            dataGridViewClientes.SelectionChanged += dataGridViewClientes_SelectionChanged;

        }

        private void CargarClientes()
        {
            try
            {
                using (var cn = Conexion.GetConnection())
                using (var da = new SqlDataAdapter(
                    "SELECT IdCliente, StrNombre, NumDocumento, StrDireccion, StrTelefono, StrEmail FROM dbo.TBLCLIENTES ORDER BY StrNombre", cn))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewClientes.DataSource = dt;

                    if (dataGridViewClientes.Columns["IdCliente"] != null)
                        dataGridViewClientes.Columns["IdCliente"].Visible = false;

                    dataGridViewClientes.AutoResizeColumns();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar clientes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtDocumento.Text = "";
            txtDireccion.Text = "";
            txtTelefono.Text = "";
            txtEmail.Text = "";
            errorProvider1.Clear();
            txtDocumento.Focus();

            if (this.Controls.ContainsKey("btnEliminar"))
                btnEliminar.Enabled = false;

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

    
        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            string documento = txtDocumento.Text.Trim();
            string nombre = txtNombre.Text.Trim();
            string direccion = txtDireccion.Text.Trim();
            string telefono = txtTelefono.Text.Trim();
            string email = txtEmail.Text.Trim();

            try
            {
                using (var cn = Conexion.GetConnection())
                using (var cmd = cn.CreateCommand())
                {
                    cn.Open();

                    cmd.CommandText = "SELECT COUNT(1) FROM dbo.TBLCLIENTES WHERE NumDocumento = @doc";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@doc", documento);

                    int existe = Convert.ToInt32(cmd.ExecuteScalar());

                    if (existe > 0)
                    {
                        // UPDATE
                        cmd.Parameters.Clear();
                        cmd.CommandText = @"
                            UPDATE dbo.TBLCLIENTES
                            SET StrNombre = @n,
                                StrDireccion = @dir,
                                StrTelefono = @tel,
                                StrEmail = @mail,
                                DtmFechaModifica = GETDATE(),
                                StrUsuarioModifica = @usr
                            WHERE NumDocumento = @doc";
                        cmd.Parameters.AddWithValue("@n", nombre);
                        cmd.Parameters.AddWithValue("@dir", direccion);
                        cmd.Parameters.AddWithValue("@tel", telefono);
                        cmd.Parameters.AddWithValue("@mail", email);
                        cmd.Parameters.AddWithValue("@usr", Session.CurrentUser ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@doc", documento);

                        int filas = cmd.ExecuteNonQuery();
                        if (filas > 0)
                            MessageBox.Show("Cliente actualizado correctamente.", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("No se pudo actualizar el cliente (verifique).", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = @"
                            INSERT INTO dbo.TBLCLIENTES
                            (StrNombre, NumDocumento, StrDireccion, StrTelefono, StrEmail, DtmFechaModifica, StrUsuarioModifica)
                            VALUES (@n, @doc, @dir, @tel, @mail, GETDATE(), @usr)";
                        cmd.Parameters.AddWithValue("@n", nombre);
                        cmd.Parameters.AddWithValue("@doc", documento);
                        cmd.Parameters.AddWithValue("@dir", direccion);
                        cmd.Parameters.AddWithValue("@tel", telefono);
                        cmd.Parameters.AddWithValue("@mail", email);
                        cmd.Parameters.AddWithValue("@usr", Session.CurrentUser ?? (object)DBNull.Value);

                        int filas = cmd.ExecuteNonQuery();
                        if (filas > 0)
                            MessageBox.Show("Cliente registrado correctamente.", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("No se pudo registrar el cliente.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                CargarClientes();
                LimpiarCampos();
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show("Error SQL: " + sqlex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewClientes_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewClientes.CurrentRow == null || dataGridViewClientes.CurrentRow.IsNewRow)
                {
                    btnEliminar.Enabled = false;
                    return;
                }

                var row = dataGridViewClientes.CurrentRow;
                if (dataGridViewClientes.Columns["NumDocumento"] != null)
                    txtDocumento.Text = row.Cells["NumDocumento"].Value?.ToString();
                else if (row.Cells.Count > 1)
                    txtDocumento.Text = row.Cells[1].Value?.ToString();

                if (dataGridViewClientes.Columns["StrNombre"] != null)
                    txtNombre.Text = row.Cells["StrNombre"].Value?.ToString();
                else if (row.Cells.Count > 2)
                    txtNombre.Text = row.Cells[2].Value?.ToString();

                if (dataGridViewClientes.Columns["StrDireccion"] != null)
                    txtDireccion.Text = row.Cells["StrDireccion"].Value?.ToString();
                else if (row.Cells.Count > 3)
                    txtDireccion.Text = row.Cells[3].Value?.ToString();

                if (dataGridViewClientes.Columns["StrTelefono"] != null)
                    txtTelefono.Text = row.Cells["StrTelefono"].Value?.ToString();
                else if (row.Cells.Count > 4)
                    txtTelefono.Text = row.Cells[4].Value?.ToString();

                if (dataGridViewClientes.Columns["StrEmail"] != null)
                    txtEmail.Text = row.Cells["StrEmail"].Value?.ToString();
                else if (row.Cells.Count > 5)
                    txtEmail.Text = row.Cells[5].Value?.ToString();

                btnEliminar.Enabled = true;
            }
            catch
            {
                btnEliminar.Enabled = false;
            }
        }


        private void btnEliminar_Click(object sender, EventArgs e)
        {
            string documento = txtDocumento.Text.Trim();
            if (string.IsNullOrWhiteSpace(documento))
            {
                MessageBox.Show("Seleccione o ingrese el documento del cliente a eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var r = MessageBox.Show("¿Confirma eliminar el cliente con documento " + documento + "?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            try
            {
                using (var cn = Conexion.GetConnection())
                using (var cmd = cn.CreateCommand())
                {
                    cn.Open();

                    int idSeleccionado = -1;
                    if (dataGridViewClientes.CurrentRow != null && dataGridViewClientes.CurrentRow.Cells["IdCliente"] != null
                        && int.TryParse(dataGridViewClientes.CurrentRow.Cells["IdCliente"].Value?.ToString(), out int idtmp))
                    {
                        idSeleccionado = idtmp;
                    }

                    if (idSeleccionado > 0)
                    {
                        cmd.CommandText = "DELETE FROM dbo.TBLCLIENTES WHERE IdCliente = @id";
                        cmd.Parameters.AddWithValue("@id", idSeleccionado);
                    }
                    else
                    {
                        cmd.CommandText = "DELETE FROM dbo.TBLCLIENTES WHERE NumDocumento = @doc";
                        cmd.Parameters.AddWithValue("@doc", documento);
                    }

                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0)
                        MessageBox.Show("Cliente eliminado correctamente.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("No se encontró el cliente para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                CargarClientes();
                LimpiarCampos();
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show("Error SQL al eliminar: " + sqlex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            string documento = txtDocumento.Text.Trim();
            string nombre = txtNombre.Text.Trim();
            string direccion = txtDireccion.Text.Trim();
            string telefono = txtTelefono.Text.Trim();
            string email = txtEmail.Text.Trim();

            try
            {
                using (var cn = Conexion.GetConnection())
                using (var cmd = cn.CreateCommand())
                {
                    cn.Open();

                    cmd.CommandText = @"
                INSERT INTO dbo.TBLCLIENTES
                (StrNombre, NumDocumento, StrDireccion, StrTelefono, StrEmail, DtmFechaModifica, StrUsuarioModifica)
                VALUES (@n, @doc, @dir, @tel, @mail, GETDATE(), @usr)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@n", nombre);
                    cmd.Parameters.AddWithValue("@doc", documento);
                    cmd.Parameters.AddWithValue("@dir", direccion);
                    cmd.Parameters.AddWithValue("@tel", telefono);
                    cmd.Parameters.AddWithValue("@mail", email);
                    cmd.Parameters.AddWithValue("@usr", Session.CurrentUser ?? (object)DBNull.Value);

                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0)
                        MessageBox.Show("Cliente creado correctamente.", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("No se pudo crear el cliente.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                CargarClientes();

                foreach (DataGridViewRow r in dataGridViewClientes.Rows)
                {
                    if (r.IsNewRow) continue;
                    if (r.Cells["NumDocumento"]?.Value?.ToString().Trim() == documento)
                    {
                        r.Selected = true;
                        if (r.Cells.Count > 0) dataGridViewClientes.CurrentCell = r.Cells[0];
                        btnEliminar.Enabled = true;
                        break;
                    }
                }

            }
            catch (SqlException sqlex)
            {
                MessageBox.Show("Error SQL al insertar: " + sqlex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Nombre_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
    }
}
