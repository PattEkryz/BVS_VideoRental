using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace BVS_VideoRental
{
    public partial class RentalForm : Form
    {
        // UI Controls
        private ComboBox cmbCustomer = new ComboBox();
        private DataGridView dgvAvailableVideos = new DataGridView();
        private NumericUpDown numRentalDays = new NumericUpDown();
        private Button btnRent = new Button();
        private Button btnReturn = new Button();

        private SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString);

        public RentalForm()
        {
            InitializeComponents();
            LoadCustomers();
            LoadAvailableVideos();
        }

        private void InitializeComponents()
        {
            // Form setup
            this.Text = "BVS Video Rental - Rent Videos";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;

            // Customer selection
            Label lblCustomer = new Label
            {
                Text = "Customer:",
                Location = new Point(20, 20),
                Size = new Size(100, 20)
            };

            cmbCustomer.Location = new Point(120, 20);
            cmbCustomer.Size = new Size(300, 20);
            cmbCustomer.DropDownStyle = ComboBoxStyle.DropDownList;

            // Rental days selection
            Label lblRentalDays = new Label
            {
                Text = "Rental Days (1-3):",
                Location = new Point(20, 50),
                Size = new Size(100, 20)
            };

            numRentalDays.Location = new Point(120, 50);
            numRentalDays.Size = new Size(100, 20);
            numRentalDays.Minimum = 1;
            numRentalDays.Maximum = 3;
            numRentalDays.Value = 1;

            // Available videos grid
            dgvAvailableVideos.Location = new Point(20, 80);
            dgvAvailableVideos.Size = new Size(740, 400);
            dgvAvailableVideos.AllowUserToAddRows = false;
            dgvAvailableVideos.AllowUserToDeleteRows = false;
            dgvAvailableVideos.ReadOnly = true;
            dgvAvailableVideos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAvailableVideos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Buttons
            btnRent.Text = "Rent Video";
            btnRent.Location = new Point(20, 500);
            btnRent.Size = new Size(150, 40);
            btnRent.Click += btnRent_Click;

            btnReturn.Text = "Return Video";
            btnReturn.Location = new Point(180, 500);
            btnReturn.Size = new Size(150, 40);
            btnReturn.Click += btnReturn_Click;

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                lblCustomer, cmbCustomer,
                lblRentalDays, numRentalDays,
                dgvAvailableVideos,
                btnRent, btnReturn
            });
        }

        private void LoadCustomers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(
                        "SELECT CustomerID, FirstName + ' ' + LastName AS FullName FROM Customers ORDER BY LastName", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cmbCustomer.DataSource = dt;
                    cmbCustomer.DisplayMember = "FullName";
                    cmbCustomer.ValueMember = "CustomerID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAvailableVideos()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(
                        "SELECT VideoID, Title, Category, AvailableCopies FROM Videos WHERE AvailableCopies > 0 ORDER BY Title", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvAvailableVideos.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading available videos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRent_Click(object sender, EventArgs e)
        {
            if (dgvAvailableVideos.SelectedRows.Count == 0 || cmbCustomer.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a customer and a video to rent", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int customerId = Convert.ToInt32(cmbCustomer.SelectedValue);
            int videoId = Convert.ToInt32(dgvAvailableVideos.SelectedRows[0].Cells["VideoID"].Value);
            int rentalDays = Convert.ToInt32(numRentalDays.Value);
            DateTime dueDate = DateTime.Now.AddDays(rentalDays);

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Create rental record
                            SqlCommand rentCmd = new SqlCommand(
                                "INSERT INTO Rentals (CustomerID, VideoID, DueDate, IsReturned) " +
                                "VALUES (@customerId, @videoId, @dueDate, 0)", conn, transaction);

                            rentCmd.Parameters.AddWithValue("@customerId", customerId);
                            rentCmd.Parameters.AddWithValue("@videoId", videoId);
                            rentCmd.Parameters.AddWithValue("@dueDate", dueDate);
                            rentCmd.ExecuteNonQuery();

                            // Update available copies
                            SqlCommand updateCmd = new SqlCommand(
                                "UPDATE Videos SET AvailableCopies = AvailableCopies - 1 WHERE VideoID = @videoId",
                                conn, transaction);
                            updateCmd.Parameters.AddWithValue("@videoId", videoId);
                            updateCmd.ExecuteNonQuery();

                            transaction.Commit();
                            MessageBox.Show("Video rented successfully!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadAvailableVideos();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"Error processing rental: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            // Create ReturnVideoForm if it exists, or implement return functionality here
            MessageBox.Show("Return functionality will be implemented here", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadAvailableVideos();
        }
    }
}