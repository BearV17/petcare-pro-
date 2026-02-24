using System;
using Microsoft.Data.Sqlite;
using System.Drawing;
using System.Windows.Forms;
using PetCarePro.Data;

namespace PetCarePro.Forms
{
    public partial class StayManagementForm : UserControl
    {
        private DataGridView dgvStays;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnCheckOut;
        private ComboBox cmbStatusFilter;

        public StayManagementForm()
        {
            InitializeComponent();
            LoadStays();
        }

        private void InitializeComponent()
        {
            this.dgvStays = new DataGridView();
            this.btnAdd = new Button();
            this.btnEdit = new Button();
            this.btnCheckOut = new Button();
            this.cmbStatusFilter = new ComboBox();
            Label lblFilter = new Label();

            this.SuspendLayout();

            // Filter
            lblFilter.Text = "Status filter:";
            lblFilter.Location = new Point(10, 10);
            lblFilter.Size = new Size(80, 20);

            this.cmbStatusFilter.Location = new Point(100, 10);
            this.cmbStatusFilter.Size = new Size(150, 20);
            this.cmbStatusFilter.Items.Add("Alle");
            this.cmbStatusFilter.Items.Add("Gepland");
            this.cmbStatusFilter.Items.Add("Actief");
            this.cmbStatusFilter.Items.Add("Voltooid");
            this.cmbStatusFilter.SelectedIndex = 0;
            this.cmbStatusFilter.SelectedIndexChanged += CmbStatusFilter_SelectedIndexChanged;

            // DataGridView
            this.dgvStays.AllowUserToAddRows = false;
            this.dgvStays.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvStays.Location = new Point(10, 45);
            this.dgvStays.Size = new Size(760, 400);
            this.dgvStays.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvStays.ReadOnly = true;
            this.dgvStays.DataError += (s, args) => { args.ThrowException = false; };

            // Buttons
            this.btnAdd.Text = "Nieuw Verblijf";
            this.btnAdd.Location = new Point(10, 460);
            this.btnAdd.Size = new Size(120, 30);
            this.btnAdd.Click += BtnAdd_Click;

            this.btnEdit.Text = "Bewerken";
            this.btnEdit.Location = new Point(140, 460);
            this.btnEdit.Size = new Size(100, 30);
            this.btnEdit.Click += BtnEdit_Click;

            this.btnCheckOut.Text = "Uitchecken";
            this.btnCheckOut.Location = new Point(250, 460);
            this.btnCheckOut.Size = new Size(100, 30);
            this.btnCheckOut.Click += BtnCheckOut_Click;

            this.Controls.Add(lblFilter);
            this.Controls.Add(this.cmbStatusFilter);
            this.Controls.Add(this.dgvStays);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnCheckOut);
            this.Size = new Size(800, 500);
            this.ResumeLayout(false);
        }

        private void LoadStays(string statusFilter = "Alle")
        {
            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = @"
                        SELECT s.Id, p.Name AS PetName, o.FirstName || ' ' || o.LastName AS OwnerName,
                               s.CheckInDate, s.CheckOutDate, s.Status, s.KennelNumber
                        FROM Stays s
                        INNER JOIN Pets p ON s.PetId = p.Id
                        INNER JOIN Owners o ON p.OwnerId = o.Id";

                    if (statusFilter != "Alle")
                    {
                        query += " WHERE s.Status = @Status";
                    }

                    query += " ORDER BY s.CheckInDate DESC";

                    using (var command = new SqliteCommand(query, connection))
                    {
                        if (statusFilter != "Alle")
                        {
                            command.Parameters.AddWithValue("@Status", statusFilter);
                        }
                        dgvStays.DataSource = DatabaseHelper.ExecuteDataTable(command);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij laden verblijven: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStays(cmbStatusFilter.SelectedItem?.ToString() ?? "Alle");
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            StayForm stayForm = new StayForm();
            if (stayForm.ShowDialog() == DialogResult.OK)
            {
                LoadStays(cmbStatusFilter.SelectedItem?.ToString() ?? "Alle");
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvStays.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecteer eerst een verblijf om te bewerken.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int stayId = Convert.ToInt32(dgvStays.SelectedRows[0].Cells["Id"].Value);
            StayForm stayForm = new StayForm(stayId);
            if (stayForm.ShowDialog() == DialogResult.OK)
            {
                LoadStays(cmbStatusFilter.SelectedItem?.ToString() ?? "Alle");
            }
        }

        private void BtnCheckOut_Click(object sender, EventArgs e)
        {
            if (dgvStays.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecteer eerst een verblijf om uit te checken.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int stayId = Convert.ToInt32(dgvStays.SelectedRows[0].Cells["Id"].Value);
                string status = dgvStays.SelectedRows[0].Cells["Status"].Value?.ToString() ?? "";

                if (status != "Actief")
                {
                    MessageBox.Show("Alleen actieve verblijven kunnen worden uitgecheckt.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = "UPDATE Stays SET CheckOutDate = @CheckOutDate, Status = 'Voltooid' WHERE Id = @Id";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", stayId);
                        command.Parameters.AddWithValue("@CheckOutDate", DateTime.Now);
                        command.ExecuteNonQuery();
                    }
                }
                LoadStays(cmbStatusFilter.SelectedItem?.ToString() ?? "Alle");
                MessageBox.Show("Dier succesvol uitgecheckt.", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij uitchecken: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
