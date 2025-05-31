using CadParcial2Ydzn;
using ClnParcial2Ydzn;
using System;
using System.Windows.Forms;

namespace CpParcial2Ydzn
{
    public partial class FrmSerie : Form
    {
        private bool esNuevo = false;

        public FrmSerie()
        {
            InitializeComponent();
        }

        private void FrmSerie_Load(object sender, EventArgs e)
        {
            cmbEstado.Items.Clear();
            cmbEstado.Items.Add(new EstadoItem { Texto = "En Emision", Valor = 1 });
            cmbEstado.Items.Add(new EstadoItem { Texto = "Estreno", Valor = 0 });
            cmbEstado.Items.Add(new EstadoItem { Texto = "Cancelada", Valor = -1 });
            cmbEstado.DisplayMember = "Texto";
            cmbEstado.ValueMember = "Valor";
            cmbEstado.SelectedIndex = 0;
            cmbIdiomaOriginal.Items.AddRange(new string[] { "Espanol", "Ingles", "Frances", "Aleman", "Japones" });
            cmbIdiomaOriginal.SelectedIndex = 0;
            ListarSeries();
        }

        private class EstadoItem
        {
            public string Texto { get; set; }
            public int Valor { get; set; }
            public override string ToString() => Texto;
        }

        private void ListarSeries(string filtro = "")
        {
            var lista = string.IsNullOrWhiteSpace(filtro) ? SerieCln.Listar() : SerieCln.Buscar(filtro);
            dgvSeries.DataSource = lista;
            dgvSeries.Columns["id"].Visible = false;
            dgvSeries.Columns["estado"].Visible = false;
            dgvSeries.Columns["estadoTexto"].HeaderText = "Estado";
            dgvSeries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSeries.Columns["urlTrailer"].HeaderText = "URL Trailer";
            dgvSeries.Columns["idiomaOriginal"].HeaderText = "Idioma";
        }


        private void LimpiarCampos()
        {
            txtId.Clear();
            txtTitulo.Clear();
            txtSinopsis.Clear();
            txtDirector.Clear();
            txtEpisodios.Clear();
            dtpFechaEstreno.Value = DateTime.Today;
            cmbEstado.SelectedIndex = 0;
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            esNuevo = true;
            txtTitulo.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                MessageBox.Show("El campo Título es obligatorio.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var serie = new Serie
            {
                titulo = txtTitulo.Text,
                sinopsis = txtSinopsis.Text,
                director = txtDirector.Text,
                episodios = int.Parse(txtEpisodios.Text),
                fechaEstreno = dtpFechaEstreno.Value,
                estado = (short)((EstadoItem)cmbEstado.SelectedItem).Valor,
                urlTrailer = txtUrlTrailer.Text,
                idiomaOriginal = cmbIdiomaOriginal.SelectedItem.ToString()
            };

            if (esNuevo)
            {
                SerieCln.Insertar(serie);
                MessageBox.Show("Serie creada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                serie.id = int.Parse(txtId.Text);
                SerieCln.Actualizar(serie);
                MessageBox.Show("Serie actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ListarSeries();
            LimpiarCampos();
        }


        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtId.Text))
                {
                    MessageBox.Show("Seleccione una serie para editar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var serie = new Serie
                {
                    id = int.Parse(txtId.Text),
                    titulo = txtTitulo.Text.Trim(),
                    sinopsis = txtSinopsis.Text.Trim(),
                    director = txtDirector.Text.Trim(),
                    episodios = int.Parse(txtEpisodios.Text.Trim()),
                    fechaEstreno = dtpFechaEstreno.Value,
                    estado = (short)((EstadoItem)cmbEstado.SelectedItem).Valor,
                    urlTrailer = txtUrlTrailer.Text,
                    idiomaOriginal = cmbIdiomaOriginal.SelectedItem.ToString()

                };

                SerieCln.Actualizar(serie);

                MessageBox.Show("Serie actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ListarSeries();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvSeries.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgvSeries.CurrentRow.Cells["id"].Value);
                string titulo = dgvSeries.CurrentRow.Cells["titulo"].Value.ToString();

                DialogResult confirm = MessageBox.Show(
                    $"¿Estás seguro que deseas eliminar la serie \"{titulo}\"?",
                    "Confirmar eliminacion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (confirm == DialogResult.Yes)
                {
                    SerieCln.Eliminar(id);
                    MessageBox.Show("Serie eliminada correctamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarSeries();
                    LimpiarCampos();
                }
            }
            else
            {
                MessageBox.Show("Por favor selecciona una serie para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim();
            ListarSeries(filtro);
        }

        private void btnCrear_Click_1(object sender, EventArgs e)
        {
            try
            {
                var serie = new Serie
                {
                    titulo = txtTitulo.Text,
                    sinopsis = txtSinopsis.Text,
                    director = txtDirector.Text,
                    episodios = int.Parse(txtEpisodios.Text),
                    fechaEstreno = dtpFechaEstreno.Value,
                    estado = (short)Convert.ToInt32(cmbEstado.SelectedValue),
                    urlTrailer = txtUrlTrailer.Text,
                    idiomaOriginal = cmbIdiomaOriginal.SelectedItem.ToString()
                };

                SerieCln.Insertar(serie);
                MessageBox.Show("Serie creada correctamente");
                LimpiarCampos();
                ListarSeries();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear: " + ex.Message);
            }
        }

        private void dgvSeries_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSeries.Rows[e.RowIndex];

                txtId.Text = row.Cells["id"].Value.ToString();
                txtTitulo.Text = row.Cells["titulo"].Value.ToString();
                txtSinopsis.Text = row.Cells["sinopsis"].Value.ToString();
                txtDirector.Text = row.Cells["director"].Value.ToString();
                txtEpisodios.Text = row.Cells["episodios"].Value.ToString();
                dtpFechaEstreno.Value = Convert.ToDateTime(row.Cells["fechaEstreno"].Value);
                txtUrlTrailer.Text = row.Cells["urlTrailer"].Value?.ToString();
                cmbIdiomaOriginal.SelectedItem = row.Cells["idiomaOriginal"].Value?.ToString();


                // Buscar el índice del estado en el ComboBox por su valor
                short estado = Convert.ToInt16(row.Cells["estado"].Value);
                for (int i = 0; i < cmbEstado.Items.Count; i++)
                {
                    var item = (EstadoItem)cmbEstado.Items[i];
                    if (item.Valor == estado)
                    {
                        cmbEstado.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void cmbIdiomaOriginal_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
