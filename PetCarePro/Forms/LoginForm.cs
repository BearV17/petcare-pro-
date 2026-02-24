using System;
using Microsoft.Data.Sqlite;
using System.Windows.Forms;
using PetCarePro.Data;

namespace PetCarePro.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.txtUsername = new TextBox();
            this.txtPassword = new TextBox();
            this.btnLogin = new Button();
            this.lblUsername = new Label();
            this.lblPassword = new Label();
            this.lblTitle = new Label();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(100, 30);
            this.lblTitle.Size = new System.Drawing.Size(200, 26);
            this.lblTitle.Text = "PetCarePro";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblUsername
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(50, 100);
            this.lblUsername.Size = new System.Drawing.Size(58, 13);
            this.lblUsername.Text = "Gebruikersnaam:";

            // txtUsername
            this.txtUsername.Location = new System.Drawing.Point(50, 120);
            this.txtUsername.Size = new System.Drawing.Size(300, 20);
            this.txtUsername.TabIndex = 0;

            // lblPassword
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(50, 160);
            this.lblPassword.Size = new System.Drawing.Size(68, 13);
            this.lblPassword.Text = "Wachtwoord:";

            // txtPassword
            this.txtPassword.Location = new System.Drawing.Point(50, 180);
            this.txtPassword.Size = new System.Drawing.Size(300, 20);
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.TabIndex = 1;

            // btnLogin
            this.btnLogin.Location = new System.Drawing.Point(150, 230);
            this.btnLogin.Size = new System.Drawing.Size(100, 35);
            this.btnLogin.Text = "Inloggen";
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Click += BtnLogin_Click;

            // LoginForm
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnLogin);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "PetCarePro - Inloggen";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblUsername;
        private Label lblPassword;
        private Label lblTitle;

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vul alstublieft gebruikersnaam en wachtwoord in.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT Id, Username, Role FROM Users WHERE Username = @Username AND Password = @Password";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = reader.GetInt32(0);
                                string userRole = reader.GetString(2);

                                // Open main form
                                this.Hide();
                                MainForm mainForm = new MainForm(userId, username, userRole);
                                mainForm.FormClosed += (s, args) => this.Close();
                                mainForm.Show();
                            }
                            else
                            {
                                MessageBox.Show("Ongeldige gebruikersnaam of wachtwoord.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Er is een fout opgetreden: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
