using System;
using Microsoft.Data.Sqlite;
using System.Drawing;
using System.Windows.Forms;
using PetCarePro.Data;
using PetCarePro.Models;

namespace PetCarePro.Forms
{
    public partial class PetManagementForm : UserControl
    {
        private DataGridView dgvPets;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private TextBox txtSearch;
        private Button btnSearch;

        public PetManagementForm()
        {
            InitializeComponent();
            LoadPets();
        }

        private void InitializeComponent()
        {
            this.dgvPets = new DataGridView();
            this.btnAdd = new Button();
            this.btnEdit = new Button();
            this.btnDelete = new Button();
            this.txtSearch = new TextBox();
            this.btnSearch = new Button();
            Label lblSearch = new Label();

            this.SuspendLayout();

            // Search label
            lblSearch.Text = "Zoeken:";
            lblSearch.Location = new Point(10, 10);
            lblSearch.Size = new Size(60, 20);

            // Search textbox
            this.txtSearch.Location = new Point(80, 10);
            this.txtSearch.Size = new Size(200, 20);

            // Search button
            this.btnSearch.Text = "Zoek";
            this.btnSearch.Location = new Point(290, 10);
            this.btnSearch.Size = new Size(75, 25);
            this.btnSearch.Click += BtnSearch_Click;

            // DataGridView
            this.dgvPets.AllowUserToAddRows = false;
            this.dgvPets.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPets.Location = new Point(10, 45);
            this.dgvPets.Size = new Size(760, 400);
            this.dgvPets.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvPets.MultiSelect = false;
            this.dgvPets.ReadOnly = true;
            this.dgvPets.DataError += (s, args) => { args.ThrowException = false; };

            // Buttons
            this.btnAdd.Text = "Nieuw Dier";
            this.btnAdd.Location = new Point(10, 460);
            this.btnAdd.Size = new Size(100, 30);
            this.btnAdd.Click += BtnAdd_Click;

            this.btnEdit.Text = "Bewerken";
            this.btnEdit.Location = new Point(120, 460);
            this.btnEdit.Size = new Size(100, 30);
            this.btnEdit.Click += BtnEdit_Click;

            this.btnDelete.Text = "Verwijderen";
            this.btnDelete.Location = new Point(230, 460);
            this.btnDelete.Size = new Size(100, 30);
            this.btnDelete.Click += BtnDelete_Click;

            // UserControl
            this.Controls.Add(lblSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.dgvPets);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Size = new Size(800, 500);
            this.ResumeLayout(false);
        }

        private void LoadPets()
        {
            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = @"
                        SELECT p.Id, p.Name, p.Species, p.Breed, p.Age, p.Gender, 
                               o.FirstName || ' ' || o.LastName AS OwnerName
                        FROM Pets p
                        INNER JOIN Owners o ON p.OwnerId = o.Id
                        ORDER BY p.Name";

                    using (var command = new SqliteCommand(query, connection))
                    {
                        dgvPets.DataSource = DatabaseHelper.ExecuteDataTable(command);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij laden van dieren: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                LoadPets();
                return;
            }

            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = @"
                        SELECT p.Id, p.Name, p.Species, p.Breed, p.Age, p.Gender, 
                               o.FirstName || ' ' || o.LastName AS OwnerName
                        FROM Pets p
                        INNER JOIN Owners o ON p.OwnerId = o.Id
                        WHERE p.Name LIKE @Search OR p.Species LIKE @Search OR 
                              o.FirstName LIKE @Search OR o.LastName LIKE @Search
                        ORDER BY p.Name";

                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Search", $"%{searchTerm}%");
                        dgvPets.DataSource = DatabaseHelper.ExecuteDataTable(command);
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
            PetForm petForm = new PetForm();
            if (petForm.ShowDialog() == DialogResult.OK)
            {
                LoadPets();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvPets.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecteer eerst een dier om te bewerken.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int petId = Convert.ToInt32(dgvPets.SelectedRows[0].Cells["Id"].Value);
            PetForm petForm = new PetForm(petId);
            if (petForm.ShowDialog() == DialogResult.OK)
            {
                LoadPets();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPets.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecteer eerst een dier om te verwijderen.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Weet u zeker dat u dit dier wilt verwijderen?", "Bevestigen", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int petId = Convert.ToInt32(dgvPets.SelectedRows[0].Cells["Id"].Value);
                    using (var connection = DatabaseHelper.GetConnection())
                    {
                        connection.Open();
                        string query = "DELETE FROM Pets WHERE Id = @Id";
                        using (var command = new SqliteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Id", petId);
                            command.ExecuteNonQuery();
                        }
                    }
                    LoadPets();
                    MessageBox.Show("Dier succesvol verwijderd.", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fout bij verwijderen: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
