using System;
using Microsoft.Data.Sqlite;
using System.Drawing;
using System.Windows.Forms;
using PetCarePro.Data;
using PetCarePro.Models;

namespace PetCarePro.Forms
{
    public partial class PetForm : Form
    {
        private int? _petId;
        private TextBox txtName;
        private ComboBox cmbSpecies;
        private TextBox txtBreed;
        private NumericUpDown numAge;
        private ComboBox cmbGender;
        private TextBox txtChipNumber;
        private TextBox txtNotes;
        private ComboBox cmbOwner;
        private Button btnSave;
        private Button btnCancel;

        public PetForm(int? petId = null)
        {
            _petId = petId;
            InitializeComponent();
            LoadOwners();
            if (petId.HasValue)
            {
                LoadPet();
            }
        }

        private void InitializeComponent()
        {
            this.txtName = new TextBox();
            this.cmbSpecies = new ComboBox();
            this.txtBreed = new TextBox();
            this.numAge = new NumericUpDown();
            this.cmbGender = new ComboBox();
            this.txtChipNumber = new TextBox();
            this.txtNotes = new TextBox();
            this.cmbOwner = new ComboBox();
            this.btnSave = new Button();
            this.btnCancel = new Button();

            this.SuspendLayout();

            int yPos = 20;
            int labelWidth = 120;
            int controlWidth = 300;
            int spacing = 35;

            // Name
            Label lblName = new Label();
            lblName.Text = "Naam:";
            lblName.Location = new Point(20, yPos);
            lblName.Size = new Size(labelWidth, 20);
            this.txtName.Location = new Point(150, yPos);
            this.txtName.Size = new Size(controlWidth, 20);
            yPos += spacing;

            // Owner
            Label lblOwner = new Label();
            lblOwner.Text = "Eigenaar:";
            lblOwner.Location = new Point(20, yPos);
            lblOwner.Size = new Size(labelWidth, 20);
            this.cmbOwner.Location = new Point(150, yPos);
            this.cmbOwner.Size = new Size(controlWidth, 20);
            this.cmbOwner.DropDownStyle = ComboBoxStyle.DropDownList;
            yPos += spacing;

            // Species
            Label lblSpecies = new Label();
            lblSpecies.Text = "Soort:";
            lblSpecies.Location = new Point(20, yPos);
            lblSpecies.Size = new Size(labelWidth, 20);
            this.cmbSpecies.Location = new Point(150, yPos);
            this.cmbSpecies.Size = new Size(controlWidth, 20);
            this.cmbSpecies.Items.AddRange(new string[] { "Hond", "Kat", "Konijn", "Vogel", "Anders" });
            yPos += spacing;

            // Breed
            Label lblBreed = new Label();
            lblBreed.Text = "Ras:";
            lblBreed.Location = new Point(20, yPos);
            lblBreed.Size = new Size(labelWidth, 20);
            this.txtBreed.Location = new Point(150, yPos);
            this.txtBreed.Size = new Size(controlWidth, 20);
            yPos += spacing;

            // Age
            Label lblAge = new Label();
            lblAge.Text = "Leeftijd:";
            lblAge.Location = new Point(20, yPos);
            lblAge.Size = new Size(labelWidth, 20);
            this.numAge.Location = new Point(150, yPos);
            this.numAge.Size = new Size(controlWidth, 20);
            this.numAge.Minimum = 0;
            this.numAge.Maximum = 30;
            yPos += spacing;

            // Gender
            Label lblGender = new Label();
            lblGender.Text = "Geslacht:";
            lblGender.Location = new Point(20, yPos);
            lblGender.Size = new Size(labelWidth, 20);
            this.cmbGender.Location = new Point(150, yPos);
            this.cmbGender.Size = new Size(controlWidth, 20);
            this.cmbGender.Items.AddRange(new string[] { "Mannetje", "Vrouwtje", "Onbekend" });
            yPos += spacing;

            // Chip Number
            Label lblChip = new Label();
            lblChip.Text = "Chipnummer:";
            lblChip.Location = new Point(20, yPos);
            lblChip.Size = new Size(labelWidth, 20);
            this.txtChipNumber.Location = new Point(150, yPos);
            this.txtChipNumber.Size = new Size(controlWidth, 20);
            yPos += spacing;

            // Notes
            Label lblNotes = new Label();
            lblNotes.Text = "Opmerkingen:";
            lblNotes.Location = new Point(20, yPos);
            lblNotes.Size = new Size(labelWidth, 20);
            this.txtNotes.Location = new Point(150, yPos);
            this.txtNotes.Size = new Size(controlWidth, 100);
            this.txtNotes.Multiline = true;
            this.txtNotes.ScrollBars = ScrollBars.Vertical;
            yPos += 120;

            // Buttons
            this.btnSave.Text = "Opslaan";
            this.btnSave.Location = new Point(150, yPos);
            this.btnSave.Size = new Size(100, 30);
            this.btnSave.Click += BtnSave_Click;

            this.btnCancel.Text = "Annuleren";
            this.btnCancel.Location = new Point(260, yPos);
            this.btnCancel.Size = new Size(100, 30);
            this.btnCancel.Click += BtnCancel_Click;

            // Form
            this.Text = _petId.HasValue ? "Dier Bewerken" : "Nieuw Dier";
            this.Size = new Size(500, yPos + 80);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            this.Controls.AddRange(new Control[] {
                lblName, txtName,
                lblOwner, cmbOwner,
                lblSpecies, cmbSpecies,
                lblBreed, txtBreed,
                lblAge, numAge,
                lblGender, cmbGender,
                lblChip, txtChipNumber,
                lblNotes, txtNotes,
                btnSave, btnCancel
            });

            this.ResumeLayout(false);
        }

        private void LoadOwners()
        {
            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT Id, FirstName || ' ' || LastName AS FullName FROM Owners ORDER BY LastName, FirstName";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbOwner.Items.Add(new { Id = reader.GetInt32(0), Name = reader.GetString(1) });
                            }
                        }
                    }
                }
                cmbOwner.DisplayMember = "Name";
                cmbOwner.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij laden eigenaren: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPet()
        {
            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM Pets WHERE Id = @Id";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", _petId.Value);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtName.Text = reader["Name"].ToString() ?? "";
                                txtBreed.Text = reader["Breed"].ToString() ?? "";
                                numAge.Value = reader["Age"] != DBNull.Value ? Convert.ToInt32(reader["Age"]) : 0;
                                cmbGender.Text = reader["Gender"].ToString() ?? "";
                                txtChipNumber.Text = reader["ChipNumber"].ToString() ?? "";
                                txtNotes.Text = reader["Notes"].ToString() ?? "";
                                cmbSpecies.Text = reader["Species"].ToString() ?? "";

                                int ownerId = reader.GetInt32(reader.GetOrdinal("OwnerId"));
                                for (int i = 0; i < cmbOwner.Items.Count; i++)
                                {
                                    dynamic item = cmbOwner.Items[i];
                                    if (item.Id == ownerId)
                                    {
                                        cmbOwner.SelectedIndex = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij laden dier: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Naam is verplicht.", "Validatie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbOwner.SelectedItem == null)
            {
                MessageBox.Show("Selecteer een eigenaar.", "Validatie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                dynamic selectedOwner = cmbOwner.SelectedItem;
                int ownerId = selectedOwner.Id;

                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    if (_petId.HasValue)
                    {
                        // Update
                        string query = @"
                            UPDATE Pets 
                            SET Name = @Name, OwnerId = @OwnerId, Species = @Species, 
                                Breed = @Breed, Age = @Age, Gender = @Gender, 
                                ChipNumber = @ChipNumber, Notes = @Notes
                            WHERE Id = @Id";
                        using (var command = new SqliteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Id", _petId.Value);
                            command.Parameters.AddWithValue("@Name", txtName.Text);
                            command.Parameters.AddWithValue("@OwnerId", ownerId);
                            command.Parameters.AddWithValue("@Species", cmbSpecies.Text);
                            command.Parameters.AddWithValue("@Breed", txtBreed.Text);
                            command.Parameters.AddWithValue("@Age", numAge.Value > 0 ? (object)numAge.Value : DBNull.Value);
                            command.Parameters.AddWithValue("@Gender", cmbGender.Text);
                            command.Parameters.AddWithValue("@ChipNumber", txtChipNumber.Text);
                            command.Parameters.AddWithValue("@Notes", txtNotes.Text);
                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Insert
                        string query = @"
                            INSERT INTO Pets (Name, OwnerId, Species, Breed, Age, Gender, ChipNumber, Notes)
                            VALUES (@Name, @OwnerId, @Species, @Breed, @Age, @Gender, @ChipNumber, @Notes)";
                        using (var command = new SqliteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Name", txtName.Text);
                            command.Parameters.AddWithValue("@OwnerId", ownerId);
                            command.Parameters.AddWithValue("@Species", cmbSpecies.Text);
                            command.Parameters.AddWithValue("@Breed", txtBreed.Text);
                            command.Parameters.AddWithValue("@Age", numAge.Value > 0 ? (object)numAge.Value : DBNull.Value);
                            command.Parameters.AddWithValue("@Gender", cmbGender.Text);
                            command.Parameters.AddWithValue("@ChipNumber", txtChipNumber.Text);
                            command.Parameters.AddWithValue("@Notes", txtNotes.Text);
                            command.ExecuteNonQuery();
                        }
                    }
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij opslaan: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
