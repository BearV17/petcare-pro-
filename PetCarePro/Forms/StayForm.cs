using System;
using Microsoft.Data.Sqlite;
using System.Drawing;
using System.Windows.Forms;
using PetCarePro.Data;

namespace PetCarePro.Forms
{
    public partial class StayForm : Form
    {
        private int? _stayId;
        private ComboBox cmbPet;
        private DateTimePicker dtpCheckIn;
        private DateTimePicker dtpCheckOut;
        private ComboBox cmbStatus;
        private TextBox txtKennelNumber;
        private TextBox txtNotes;
        private Button btnSave;
        private Button btnCancel;

        public StayForm(int? stayId = null)
        {
            _stayId = stayId;
            InitializeComponent();
            LoadPets();
            if (stayId.HasValue)
            {
                LoadStay();
            }
            else
            {
                dtpCheckIn.Value = DateTime.Now;
                cmbStatus.Text = "Gepland";
            }
        }

        private void InitializeComponent()
        {
            this.cmbPet = new ComboBox();
            this.dtpCheckIn = new DateTimePicker();
            this.dtpCheckOut = new DateTimePicker();
            this.cmbStatus = new ComboBox();
            this.txtKennelNumber = new TextBox();
            this.txtNotes = new TextBox();
            this.btnSave = new Button();
            this.btnCancel = new Button();

            this.SuspendLayout();

            int yPos = 20;
            int labelWidth = 120;
            int controlWidth = 300;
            int spacing = 35;

            // Pet
            Label lblPet = new Label();
            lblPet.Text = "Dier:";
            lblPet.Location = new Point(20, yPos);
            lblPet.Size = new Size(labelWidth, 20);
            this.cmbPet.Location = new Point(150, yPos);
            this.cmbPet.Size = new Size(controlWidth, 20);
            this.cmbPet.DropDownStyle = ComboBoxStyle.DropDownList;
            yPos += spacing;

            // Check In Date
            Label lblCheckIn = new Label();
            lblCheckIn.Text = "Incheckdatum:";
            lblCheckIn.Location = new Point(20, yPos);
            lblCheckIn.Size = new Size(labelWidth, 20);
            this.dtpCheckIn.Location = new Point(150, yPos);
            this.dtpCheckIn.Size = new Size(controlWidth, 20);
            this.dtpCheckIn.Format = DateTimePickerFormat.Short;
            yPos += spacing;

            // Check Out Date
            Label lblCheckOut = new Label();
            lblCheckOut.Text = "Uitcheckdatum:";
            lblCheckOut.Location = new Point(20, yPos);
            lblCheckOut.Size = new Size(labelWidth, 20);
            this.dtpCheckOut.Location = new Point(150, yPos);
            this.dtpCheckOut.Size = new Size(controlWidth, 20);
            this.dtpCheckOut.Format = DateTimePickerFormat.Short;
            this.dtpCheckOut.ShowCheckBox = true;
            yPos += spacing;

            // Status
            Label lblStatus = new Label();
            lblStatus.Text = "Status:";
            lblStatus.Location = new Point(20, yPos);
            lblStatus.Size = new Size(labelWidth, 20);
            this.cmbStatus.Location = new Point(150, yPos);
            this.cmbStatus.Size = new Size(controlWidth, 20);
            this.cmbStatus.Items.AddRange(new string[] { "Gepland", "Actief", "Voltooid", "Geannuleerd" });
            yPos += spacing;

            // Kennel Number
            Label lblKennel = new Label();
            lblKennel.Text = "Hoknummer:";
            lblKennel.Location = new Point(20, yPos);
            lblKennel.Size = new Size(labelWidth, 20);
            this.txtKennelNumber.Location = new Point(150, yPos);
            this.txtKennelNumber.Size = new Size(controlWidth, 20);
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
            this.Text = _stayId.HasValue ? "Verblijf Bewerken" : "Nieuw Verblijf";
            this.Size = new Size(500, yPos + 80);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            this.Controls.AddRange(new Control[] {
                lblPet, cmbPet,
                lblCheckIn, dtpCheckIn,
                lblCheckOut, dtpCheckOut,
                lblStatus, cmbStatus,
                lblKennel, txtKennelNumber,
                lblNotes, txtNotes,
                btnSave, btnCancel
            });

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
                        SELECT p.Id, p.Name || ' (' || o.FirstName || ' ' || o.LastName || ')' AS PetInfo
                        FROM Pets p
                        INNER JOIN Owners o ON p.OwnerId = o.Id
                        ORDER BY p.Name";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbPet.Items.Add(new { Id = reader.GetInt32(0), Info = reader.GetString(1) });
                            }
                        }
                    }
                }
                cmbPet.DisplayMember = "Info";
                cmbPet.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij laden dieren: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStay()
        {
            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM Stays WHERE Id = @Id";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", _stayId.Value);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int petId = reader.GetInt32(reader.GetOrdinal("PetId"));
                                for (int i = 0; i < cmbPet.Items.Count; i++)
                                {
                                    dynamic item = cmbPet.Items[i];
                                    if (item.Id == petId)
                                    {
                                        cmbPet.SelectedIndex = i;
                                        break;
                                    }
                                }

                                dtpCheckIn.Value = DateTime.Parse(reader["CheckInDate"].ToString() ?? DateTime.Now.ToString());
                                if (reader["CheckOutDate"] != DBNull.Value)
                                {
                                    dtpCheckOut.Value = DateTime.Parse(reader["CheckOutDate"].ToString() ?? "");
                                    dtpCheckOut.Checked = true;
                                }
                                else
                                {
                                    dtpCheckOut.Checked = false;
                                }

                                cmbStatus.Text = reader["Status"].ToString() ?? "";
                                txtKennelNumber.Text = reader["KennelNumber"].ToString() ?? "";
                                txtNotes.Text = reader["Notes"].ToString() ?? "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij laden verblijf: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (cmbPet.SelectedItem == null)
            {
                MessageBox.Show("Selecteer een dier.", "Validatie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dtpCheckOut.Checked && dtpCheckOut.Value < dtpCheckIn.Value)
            {
                MessageBox.Show("Uitcheckdatum moet na incheckdatum liggen.", "Validatie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                dynamic selectedPet = cmbPet.SelectedItem;
                int petId = selectedPet.Id;

                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    if (_stayId.HasValue)
                    {
                        // Update
                        string query = @"
                            UPDATE Stays 
                            SET PetId = @PetId, CheckInDate = @CheckInDate, 
                                CheckOutDate = @CheckOutDate, Status = @Status, 
                                KennelNumber = @KennelNumber, Notes = @Notes
                            WHERE Id = @Id";
                        using (var command = new SqliteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Id", _stayId.Value);
                            command.Parameters.AddWithValue("@PetId", petId);
                            command.Parameters.AddWithValue("@CheckInDate", dtpCheckIn.Value);
                            command.Parameters.AddWithValue("@CheckOutDate", dtpCheckOut.Checked ? (object)dtpCheckOut.Value : DBNull.Value);
                            command.Parameters.AddWithValue("@Status", cmbStatus.Text);
                            command.Parameters.AddWithValue("@KennelNumber", txtKennelNumber.Text);
                            command.Parameters.AddWithValue("@Notes", txtNotes.Text);
                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Insert
                        string query = @"
                            INSERT INTO Stays (PetId, CheckInDate, CheckOutDate, Status, KennelNumber, Notes)
                            VALUES (@PetId, @CheckInDate, @CheckOutDate, @Status, @KennelNumber, @Notes)";
                        using (var command = new SqliteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@PetId", petId);
                            command.Parameters.AddWithValue("@CheckInDate", dtpCheckIn.Value);
                            command.Parameters.AddWithValue("@CheckOutDate", dtpCheckOut.Checked ? (object)dtpCheckOut.Value : DBNull.Value);
                            command.Parameters.AddWithValue("@Status", cmbStatus.Text);
                            command.Parameters.AddWithValue("@KennelNumber", txtKennelNumber.Text);
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
