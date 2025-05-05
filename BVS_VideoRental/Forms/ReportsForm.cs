using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace BVS_VideoRental
{
    public partial class ReportsForm : Form
    {
        // UI Controls
        private DataGridView dgvVideoReport = new DataGridView();
        private DataGridView dgvCustomerRentals = new DataGridView();
        private Button btnVideoReport = new Button();
        private Button btnCustomerRentals = new Button();
        private TabControl tabControl1 = new TabControl();
        private TabPage tabVideoReport = new TabPage();
        private TabPage tabCustomerRentals = new TabPage();

        private SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString);

        public ReportsForm()
        {
            InitializeComponents();
            LoadVideoReport();
        }

        private void InitializeComponents()
        {
            // Form setup
            this.Text = "BVS Video Rental - Reports";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;

            // Tab Control setup
            tabControl1.Dock = DockStyle.Fill;
            this.Controls.Add(tabControl1);

            // Video Report Tab
            tabVideoReport.Text = "Video Inventory";
            tabControl1.TabPages.Add(tabVideoReport);

            dgvVideoReport.Dock = DockStyle.Fill;
            dgvVideoReport.ReadOnly = true;
            dgvVideoReport.AllowUserToAddRows = false;
            dgvVideoReport.AllowUserToDeleteRows = false;
            tabVideoReport.Controls.Add(dgvVideoReport);

            // Customer Rentals Tab
            tabCustomerRentals.Text = "Current Rentals";
            tabControl1.TabPages.Add(tabCustomerRentals);

            dgvCustomerRentals.Dock = DockStyle.Fill;
            dgvCustomerRentals.ReadOnly = true;
            dgvCustomerRentals.AllowUserToAddRows = false;
            dgvCustomerRentals.AllowUserToDeleteRows = false;
            tabCustomerRentals.Controls.Add(dgvCustomerRentals);

            // Button Panel
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50
            };
            this.Controls.Add(buttonPanel);

            // Refresh Buttons
            btnVideoReport.Text = "Refresh Video Report";
            btnVideoReport.Location = new Point(20, 10);
            btnVideoReport.Click += (s, e) => LoadVideoReport();

            btnCustomerRentals.Text = "Refresh Rentals";
            btnCustomerRentals.Location = new Point(200, 10);
            btnCustomerRentals.Click += btnCustomerRentals_Click;

            buttonPanel.Controls.Add(btnVideoReport);
            buttonPanel.Controls.Add(btnCustomerRentals);
        }

        private void LoadVideoReport()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(
                        "SELECT v.Title, v.Category, " +
                        "v.AvailableCopies AS 'In', " +
                        "(v.TotalCopies - v.AvailableCopies) AS 'Out' " +
                        "FROM Videos v ORDER BY v.Title", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvVideoReport.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading video report: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCustomerRentals_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(
                        "SELECT c.FirstName + ' ' + c.LastName AS Customer, " +
                        "v.Title, v.Category, r.RentalDate, r.DueDate " +
                        "FROM Rentals r " +
                        "JOIN Customers c ON r.CustomerID = c.CustomerID " +
                        "JOIN Videos v ON r.VideoID = v.VideoID " +
                        "WHERE r.IsReturned = 0 " +
                        "ORDER BY c.LastName, r.DueDate", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvCustomerRentals.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customer rentals: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}