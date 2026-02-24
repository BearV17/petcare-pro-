using System;
using Microsoft.Data.Sqlite;
using System.Drawing;
using System.Windows.Forms;
using PetCarePro.Data;

namespace PetCarePro.Forms
{
    public partial class OwnerManagementForm : UserControl
    {
        private DataGridView dgvOwners;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private TextBox txtSearch;

        public OwnerManagementForm()
        {
            InitializeComponent();
            LoadOwners();
        }

        private void InitializeComponent()
        {
            this.dgvOwners = new DataGridView();
            this.btnAdd = new Button();
            this.btnEdit = new Button();
            this.btnDelete = new Button();
            this.txtSearch = new TextBox();
            Label lblSearch = new Label();
            Button btnSearch = new Button();

            this.SuspendLayout();

            // Search
            lblSearch.Text = "Zoeken:";
            lblSearch.Location = new Point(10, 10);
            lblSearch.Size = new Size(60, 20);

            this.txtSearch.Location = new Point(80, 10);
            this.txtSearch.Size = new Size(200, 20);

            btnSearch.Text = "Zoek";
            btnSearch.Location = new Point(290, 10);
            btnSearch.Size = new Size(75, 25);
            btnSearch.Click += BtnSearch_Click;

            // DataGridView
            this.dgvOwners.AllowUserToAddRows = false;
            this.dgvOwners.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOwners.Location = new Point(10, 45);
            this.dgvOwners.Size = new Size(760, 400);
            this.dgvOwners.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvOwners.ReadOnly = true;
            this.dgvOwners.DataError += (s, args) => { args.ThrowException = false; };

            // Buttons
            this.btnAdd.Text = "Nieuwe Eigenaar";
            this.btnAdd.Location = new Point(10, 460);
            this.btnAdd.Size = new Size(120, 30);
            this.btnAdd.Click += BtnAdd_Click;

            this.btnEdit.Text = "Bewerken";
            this.btnEdit.Location = new Point(140, 460);
            this.btnEdit.Size = new Size(100, 30);
            this.btnEdit.Click += BtnEdit_Click;

            this.btnDelete.Text = "Verwijderen";
            this.btnDelete.Location = new Point(250, 460);
            this.btnDelete.Size = new Size(100, 30);
            this.btnDelete.Click += BtnDelete_Click;

            this.Controls.Add(lblSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(btnSearch);
            this.Controls.Add(this.dgvOwners);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Size = new Size(800, 500);
            this.ResumeLayout(false);
        }

        private void LoadOwners()
        {
            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT Id, FirstName, LastName, Email, Phone, City FROM Owners ORDER BY LastName, FirstName";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        dgvOwners.DataSource = DatabaseHelper.ExecuteDataTable(command);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij laden eigenaren: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                LoadOwners();
                return;
            }

            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = @"
                        SELECT Id, FirstName, LastName, Email, Phone, City 
                        FROM Owners 
                        WHERE FirstName LIKE @Search OR LastName LIKE @Search OR Email LIKE @Search
                        ORDER BY LastName, FirstName";

                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Search", $"%{searchTerm}%");
                        dgvOwners.DataSource = DatabaseHelper.ExecuteDataTable(command);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij zoeken: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            OwnerForm ownerForm = new OwnerForm();
            if (ownerForm.ShowDialog() == DialogResult.OK)
            {
                LoadOwners();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvOwners.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecteer eerst een eigenaar om te bewerken.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int ownerId = Convert.ToInt32(dgvOwners.SelectedRows[0].Cells["Id"].Value);
            OwnerForm ownerForm = new OwnerForm(ownerId);
            if (ownerForm.ShowDialog() == DialogResult.OK)
            {
                LoadOwners();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvOwners.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecteer eerst een eigenaar om te verwijderen.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Weet u zeker dat u deze eigenaar wilt verwijderen? Alle bijbehorende dieren worden ook verwijderd!", "Bevestigen", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    int ownerId = Convert.ToInt32(dgvOwners.SelectedRows[0].Cells["Id"].Value);
                    using (var connection = DatabaseHelper.GetConnection())
                    {
                        connection.Open();
                        string query = "DELETE FROM Owners WHERE Id = @Id";
                        using (var command = new SqliteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Id", ownerId);
                            command.ExecuteNonQuery();
                        }
                    }
                    LoadOwners();
                    MessageBox.Show("Eigenaar succesvol verwijderd.", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fout bij verwijderen: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
