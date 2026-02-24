using System;
using Microsoft.Data.Sqlite;
using System.Drawing;
using System.Windows.Forms;
using PetCarePro.Data;

namespace PetCarePro.Forms
{
    public partial class CalendarForm : UserControl
    {
        private MonthCalendar calendar;
        private ListBox lstStays;
        private Label lblSelectedDate;

        public CalendarForm()
        {
            InitializeComponent();
            calendar.DateSelected += Calendar_DateSelected;
            LoadStaysForDate(calendar.SelectionStart);
        }

        private void InitializeComponent()
        {
            this.calendar = new MonthCalendar();
            this.lstStays = new ListBox();
            this.lblSelectedDate = new Label();

            this.SuspendLayout();

            // Calendar
            this.calendar.Location = new Point(10, 10);
            this.calendar.Size = new Size(300, 200);

            // Label
            this.lblSelectedDate.AutoSize = true;
            this.lblSelectedDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblSelectedDate.Location = new Point(10, 220);
            this.lblSelectedDate.Size = new Size(200, 20);
            this.lblSelectedDate.Text = "Verblijven op: " + calendar.SelectionStart.ToShortDateString();

            // ListBox
            this.lstStays.Location = new Point(10, 250);
            this.lstStays.Size = new Size(760, 200);
            this.lstStays.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);

            this.Controls.Add(this.calendar);
            this.Controls.Add(this.lblSelectedDate);
            this.Controls.Add(this.lstStays);
            this.Size = new Size(800, 500);
            this.ResumeLayout(false);
        }

        private void Calendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            LoadStaysForDate(e.Start);
        }

        private void LoadStaysForDate(DateTime selectedDate)
        {
            lblSelectedDate.Text = "Verblijven op: " + selectedDate.ToShortDateString();
            lstStays.Items.Clear();

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
                        INNER JOIN Owners o ON p.OwnerId = o.Id
                        WHERE DATE(s.CheckInDate) <= @SelectedDate 
                          AND (s.CheckOutDate IS NULL OR DATE(s.CheckOutDate) >= @SelectedDate)
                          AND s.Status IN ('Gepland', 'Actief')
                        ORDER BY s.CheckInDate";

                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SelectedDate", selectedDate.ToString("yyyy-MM-dd"));
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string checkIn = DateTime.Parse(reader["CheckInDate"].ToString() ?? "").ToShortDateString();
                                string checkOut = reader["CheckOutDate"] != DBNull.Value 
                                    ? DateTime.Parse(reader["CheckOutDate"].ToString() ?? "").ToShortDateString() 
                                    : "Open";
                                string info = $"{reader["PetName"]} ({reader["OwnerName"]}) - {reader["Status"]} - Hok: {reader["KennelNumber"]} - In: {checkIn} - Uit: {checkOut}";
                                lstStays.Items.Add(info);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij laden verblijven: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
