using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace BVS_VideoRental
{
    public partial class VideoManagementForm : Form
    {
        // UI Controls
        private DataGridView dgvVideos = new DataGridView();
        private TextBox txtTitle = new TextBox();
        private ComboBox cmbCategory = new ComboBox();
        private NumericUpDown numCopies = new NumericUpDown();
        private NumericUpDown numRentalDays = new NumericUpDown();
        private Button btnAdd = new Button();
        private Button btnDelete = new Button();
        private Button btnClear = new Button();

        private SqlConnection connection = new SqlConnection(DatabaseConfig.ConnectionString);

        public VideoManagementForm()
        {
            InitializeControls();
            LoadVideos();
        }

        private void InitializeControls()
        {
            // Form setup
            this.Text = "Video Management";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;

            // Main DataGridView
            dgvVideos.Dock = DockStyle.Fill;
            dgvVideos.AllowUserToAddRows = false;
            dgvVideos.AllowUserToDeleteRows = false;
            dgvVideos.ReadOnly = true;
            dgvVideos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVideos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.Controls.Add(dgvVideos);

            // Input Panel
            Panel inputPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 150
            };

            // Title controls
            Label lblTitle = new Label
            {
                Text = "Title:",
                Location = new Point(20, 20),
                Size = new Size(80, 20)
            };
            txtTitle.Location = new Point(100, 20);
            txtTitle.Size = new Size(200, 20);

            // Category controls
            Label lblCategory = new Label
            {
                Text = "Category:",
                Location = new Point(20, 50),
                Size = new Size(80, 20)
            };
            cmbCategory.Location = new Point(100, 50);
            cmbCategory.Size = new Size(100, 20);
            cmbCategory.Items.AddRange(new[] { "VCD", "DVD" });
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;

            // Copies controls
            Label lblCopies = new Label
            {
                Text = "Copies:",
                Location = new Point(20, 80),
                Size = new Size(80, 20)
            };
            numCopies.Location = new Point(100, 80);
            numCopies.Size = new Size(100, 20);
            numCopies.Minimum = 1;
            numCopies.Maximum = 100;
            numCopies.Value = 1;

            // Rental Days controls
            Label lblRentalDays = new Label
            {
                Text = "Rental Days:",
                Location = new Point(20, 110),
                Size = new Size(80, 20)
            };
            numRentalDays.Location = new Point(100, 110);
            numRentalDays.Size = new Size(100, 20);
            numRentalDays.Minimum = 1;
            numRentalDays.Maximum = 3;
            numRentalDays.Value = 1;

            // Buttons
            btnAdd.Text = "Add";
            btnAdd.Location = new Point(300, 20);
            btnAdd.Size = new Size(100, 30);
            btnAdd.Click += BtnAdd_Click;

            btnDelete.Text = "Delete";
            btnDelete.Location = new Point(300, 60);
            btnDelete.Size = new Size(100, 30);
            btnDelete.Click += BtnDelete_Click;

            btnClear.Text = "Clear";
            btnClear.Location = new Point(300, 100);
            btnClear.Size = new Size(100, 30);
            btnClear.Click += BtnClear_Click;

            // Add controls to input panel
            inputPanel.Controls.AddRange(new Control[] {
                lblTitle, txtTitle,
                lblCategory, cmbCategory,
                lblCopies, numCopies,
                lblRentalDays, numRentalDays,
                btnAdd, btnDelete, btnClear
            });

            this.Controls.Add(inputPanel);
        }

        private void LoadVideos()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Videos ORDER BY Title", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvVideos.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading videos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter a title", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                decimal price = cmbCategory.SelectedItem.ToString() == "VCD" ? 25m : 50m;

                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO Videos (Title, Category, TotalCopies, AvailableCopies, RentalDays, RentalPrice) " +
                        "VALUES (@title, @category, @copies, @copies, @rentalDays, @price)", conn);

                    cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@category", cmbCategory.SelectedItem);
                    cmd.Parameters.AddWithValue("@copies", (int)numCopies.Value);
                    cmd.Parameters.AddWithValue("@rentalDays", (int)numRentalDays.Value);
                    cmd.Parameters.AddWithValue("@price", price);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Video added successfully", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadVideos();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding video: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvVideos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a video to delete", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this video?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                int videoId = Convert.ToInt32(dgvVideos.SelectedRows[0].Cells["VideoID"].Value);

                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    // Check if video has rentals
                    SqlCommand checkCmd = new SqlCommand(
                        "SELECT COUNT(*) FROM Rentals WHERE VideoID=@videoId AND IsReturned=0", conn);
                    checkCmd.Parameters.AddWithValue("@videoId", videoId);
                    int rentalCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (rentalCount > 0)
                    {
                        MessageBox.Show("Cannot delete video with active rentals", "Delete Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    SqlCommand cmd = new SqlCommand("DELETE FROM Videos WHERE VideoID=@videoId", conn);
                    cmd.Parameters.AddWithValue("@videoId", videoId);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Video deleted successfully", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadVideos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting video: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtTitle.Clear();
            cmbCategory.SelectedIndex = -1;
            numCopies.Value = 1;
            numRentalDays.Value = 1;
            dgvVideos.ClearSelection();
        }
    }
}