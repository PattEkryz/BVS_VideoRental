using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;


namespace BVS_VideoRental
{
    public partial class MainForm : Form
    {
        private bool isAdmin;

        // Controls that were missing
        private Button btnManageCustomers;
        private Button btnManageVideos;
        private Button btnRentVideos;
        private Button btnReports;
        private Button btnLogout;

        public MainForm(bool adminStatus)
        {
            
            isAdmin = adminStatus;
            InitializeControls();
            
        }

        private void InitializeControls()
        {
            // Initialize all controls that were missing
            btnManageCustomers = new Button { Text = "Customers", Name = "btnManageCustomers" };
            btnManageVideos = new Button { Text = "Videos", Name = "btnManageVideos" };
            btnRentVideos = new Button { Text = "Rent Videos", Name = "btnRentVideos" };
            btnReports = new Button { Text = "Reports", Name = "btnReports" };
            btnLogout = new Button { Text = "Logout", Name = "btnLogout" };

            // Add event handlers
            btnManageCustomers.Click += btnManageCustomers_Click;
            btnManageVideos.Click += btnManageVideos_Click;
            btnRentVideos.Click += btnRentVideos_Click;
            btnReports.Click += btnReports_Click;
            btnLogout.Click += btnLogout_Click;

            // Add controls to form
            this.Controls.Add(btnManageCustomers);
            this.Controls.Add(btnManageVideos);
            this.Controls.Add(btnRentVideos);
            this.Controls.Add(btnReports);
            this.Controls.Add(btnLogout);

            // Layout controls (simplified for example)
            btnManageCustomers.Location = new System.Drawing.Point(20, 20);
            btnManageVideos.Location = new System.Drawing.Point(20, 60);
            btnRentVideos.Location = new System.Drawing.Point(20, 100);
            btnReports.Location = new System.Drawing.Point(20, 140);
            btnLogout.Location = new System.Drawing.Point(20, 180);
        }

       

        private void btnManageCustomers_Click(object sender, EventArgs e)
        {
            using (CustomerManagementForm customerForm = new CustomerManagementForm())
            {
                customerForm.ShowDialog();
            }
        }

        private void btnManageVideos_Click(object sender, EventArgs e)
        {
            using (VideoManagementForm videoForm = new VideoManagementForm())
            {
                videoForm.ShowDialog();
            }
        }

        private void btnRentVideos_Click(object sender, EventArgs e)
        {
            using (RentalForm rentalForm = new RentalForm())
            {
                rentalForm.ShowDialog();
            }
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            using (ReportsForm reportsForm = new ReportsForm())
            {
                reportsForm.ShowDialog();
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            LoginForm login = new LoginForm();
            login.Show();
        }
    }
}