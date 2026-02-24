using System;
using Microsoft.Data.Sqlite;
using System.Drawing;
using System.Windows.Forms;
using PetCarePro.Data;

namespace PetCarePro.Forms
{
    public partial class SettingsForm : UserControl
    {
        private Button btnManageUsers;
        private Label lblInfo;

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.btnManageUsers = new Button();
            this.lblInfo = new Label();

            this.SuspendLayout();

            // Info label
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblInfo.Location = new Point(20, 20);
            this.lblInfo.Size = new Size(500, 20);
            this.lblInfo.Text = "Instellingen - Alleen beschikbaar voor beheerders";

            // Manage Users button
            this.btnManageUsers.Text = "Gebruikersbeheer";
            this.btnManageUsers.Location = new Point(20, 60);
            this.btnManageUsers.Size = new Size(150, 30);
            this.btnManageUsers.Click += BtnManageUsers_Click;

            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnManageUsers);
            this.Size = new Size(800, 500);
            this.ResumeLayout(false);
        }

        private void BtnManageUsers_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Gebruikersbeheer functionaliteit kan hier worden toegevoegd.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
