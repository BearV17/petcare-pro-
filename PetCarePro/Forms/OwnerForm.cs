using System;
using Microsoft.Data.Sqlite;
using System.Drawing;
using System.Windows.Forms;
using PetCarePro.Data;

namespace PetCarePro.Forms
{
    public partial class OwnerForm : Form
    {
        private int? _ownerId;
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtEmail;
        private TextBox txtPhone;
        private TextBox txtAddress;
        private TextBox txtCity;
        private TextBox txtPostalCode;
        private Button btnSave;
        private Button btnCancel;

        public OwnerForm(int? ownerId = null)
        {
            _ownerId = ownerId;
            InitializeComponent();
            if (ownerId.HasValue)
            {
                LoadOwner();
            }
        }

        private void InitializeComponent()
        {
            this.txtFirstName = new TextBox();
            this.txtLastName = new TextBox();
            this.txtEmail = new TextBox();
            this.txtPhone = new TextBox();
            this.txtAddress = new TextBox();
            this.txtCity = new TextBox();
            this.txtPostalCode = new TextBox();
            this.btnSave = new Button();
            this.btnCancel = new Button();

            this.SuspendLayout();

            int yPos = 20;
            int labelWidth = 120;
            int controlWidth = 300;
            int spacing = 35;

            // First Name
            Label lblFirstName = new Label();
            lblFirstName.Text = "Voornaam:";
            lblFirstName.Location = new Point(20, yPos);
            lblFirstName.Size = new Size(labelWidth, 20);
            this.txtFirstName.Location = new Point(150, yPos);
            this.txtFirstName.Size = new Size(controlWidth, 20);
            yPos += spacing;

            // Last Name
            Label lblLastName = new Label();
            lblLastName.Text = "Achternaam:";
            lblLastName.Location = new Point(20, yPos);
            lblLastName.Size = new Size(labelWidth, 20);
            this.txtLastName.Location = new Point(150, yPos);
            this.txtLastName.Size = new Size(controlWidth, 20);
            yPos += spacing;

            // Email
            Label lblEmail = new Label();
            lblEmail.Text = "E-mail:";
            lblEmail.Location = new Point(20, yPos);
            lblEmail.Size = new Size(labelWidth, 20);
            this.txtEmail.Location = new Point(150, yPos);
            this.txtEmail.Size = new Size(controlWidth, 20);
            yPos += spacing;

            // Phone
            Label lblPhone = new Label();
            lblPhone.Text = "Telefoon:";
            lblPhone.Location = new Point(20, yPos);
            lblPhone.Size = new Size(labelWidth, 20);
            this.txtPhone.Location = new Point(150, yPos);
            this.txtPhone.Size = new Size(controlWidth, 20);
            yPos += spacing;

            // Address
            Label lblAddress = new Label();
            lblAddress.Text = "Adres:";
            lblAddress.Location = new Point(20, yPos);
            lblAddress.Size = new Size(labelWidth, 20);
            this.txtAddress.Location = new Point(150, yPos);
            this.txtAddress.Size = new Size(controlWidth, 20);
            yPos += spacing;

            // City
            Label lblCity = new Label();
            lblCity.Text = "Plaats:";
            lblCity.Location = new Point(20, yPos);
            lblCity.Size = new Size(labelWidth, 20);
            this.txtCity.Location = new Point(150, yPos);
            this.txtCity.Size = new Size(controlWidth, 20);
            yPos += spacing;

            // Postal Code
            Label lblPostalCode = new Label();
            lblPostalCode.Text = "Postcode:";
            lblPostalCode.Location = new Point(20, yPos);
            lblPostalCode.Size = new Size(labelWidth, 20);
            this.txtPostalCode.Location = new Point(150, yPos);
            this.txtPostalCode.Size = new Size(controlWidth, 20);
            yPos += spacing;

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
            this.Text = _ownerId.HasValue ? "Eigenaar Bewerken" : "Nieuwe Eigenaar";
            this.Size = new Size(500, yPos + 80);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            this.Controls.AddRange(new Control[] {
                lblFirstName, txtFirstName,
                lblLastName, txtLastName,
                lblEmail, txtEmail,
                lblPhone, txtPhone,
                lblAddress, txtAddress,
                lblCity, txtCity,
                lblPostalCode, txtPostalCode,
                btnSave, btnCancel
            });

            this.ResumeLayout(false);
        }

        private void LoadOwner()
        {
            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM Owners WHERE Id = @Id";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", _ownerId.Value);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtFirstName.Text = reader["FirstName"].ToString() ?? "";
                                txtLastName.Text = reader["LastName"].ToString() ?? "";
                                txtEmail.Text = reader["Email"].ToString() ?? "";
                                txtPhone.Text = reader["Phone"].ToString() ?? "";
                                txtAddress.Text = reader["Address"].ToString() ?? "";
                                txtCity.Text = reader["City"].ToString() ?? "";
                                txtPostalCode.Text = reader["PostalCode"].ToString() ?? "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij laden eigenaar: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Voornaam en achternaam zijn verplicht.", "Validatie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    if (_ownerId.HasValue)
                    {
                        // Update
                        string query = @"
                            UPDATE Owners 
                            SET FirstName = @FirstName, LastName = @LastName, Email = @Email, 
                                Phone = @Phone, Address = @Address, City = @City, PostalCode = @PostalCode
                            WHERE Id = @Id";
                        using (var command = new SqliteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Id", _ownerId.Value);
                            command.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                            command.Parameters.AddWithValue("@LastName", txtLastName.Text);
                            command.Parameters.AddWithValue("@Email", txtEmail.Text);
                            command.Parameters.AddWithValue("@Phone", txtPhone.Text);
                            command.Parameters.AddWithValue("@Address", txtAddress.Text);
                            command.Parameters.AddWithValue("@City", txtCity.Text);
                            command.Parameters.AddWithValue("@PostalCode", txtPostalCode.Text);
                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Insert
                        string query = @"
                            INSERT INTO Owners (FirstName, LastName, Email, Phone, Address, City, PostalCode)
                            VALUES (@FirstName, @LastName, @Email, @Phone, @Address, @City, @PostalCode)";
                        using (var command = new SqliteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                            command.Parameters.AddWithValue("@LastName", txtLastName.Text);
                            command.Parameters.AddWithValue("@Email", txtEmail.Text);
                            command.Parameters.AddWithValue("@Phone", txtPhone.Text);
                            command.Parameters.AddWithValue("@Address", txtAddress.Text);
                            command.Parameters.AddWithValue("@City", txtCity.Text);
                            command.Parameters.AddWithValue("@PostalCode", txtPostalCode.Text);
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
