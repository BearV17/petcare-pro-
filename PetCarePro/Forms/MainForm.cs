using System;
using System.Windows.Forms;
using PetCarePro.Forms;

namespace PetCarePro
{
    public partial class MainForm : Form
    {
        private int _userId;
        private string _username;
        private string _userRole;
        private TabControl _tabControl;

        public MainForm(int userId, string username, string userRole)
        {
            _userId = userId;
            _username = username;
            _userRole = userRole;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this._tabControl = new TabControl();
            this.SuspendLayout();

            // TabControl
            this._tabControl.Dock = DockStyle.Fill;
            this._tabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);

            // Create tabs
            TabPage tabPets = new TabPage("Dieren");
            tabPets.Controls.Add(new PetManagementForm());

            TabPage tabOwners = new TabPage("Eigenaren");
            tabOwners.Controls.Add(new OwnerManagementForm());

            TabPage tabStays = new TabPage("Verblijven");
            tabStays.Controls.Add(new StayManagementForm());

            TabPage tabCalendar = new TabPage("Kalender");
            tabCalendar.Controls.Add(new CalendarForm());

            this._tabControl.TabPages.Add(tabPets);
            this._tabControl.TabPages.Add(tabOwners);
            this._tabControl.TabPages.Add(tabStays);
            this._tabControl.TabPages.Add(tabCalendar);

            // Add settings tab only for administrators
            if (_userRole == "Administrator")
            {
                TabPage tabSettings = new TabPage("Instellingen");
                tabSettings.Controls.Add(new SettingsForm());
                this._tabControl.TabPages.Add(tabSettings);
            }

            // MainForm
            this.Text = $"PetCarePro - Welkom, {_username} ({_userRole})";
            this.Size = new System.Drawing.Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Controls.Add(this._tabControl);
            this.ResumeLayout(false);
        }
    }
}
