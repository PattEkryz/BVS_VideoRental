using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace BVS_VideoRental
{
    public partial class CustomerManagementForm : Form
    {
        private DataGridView dgvCustomers;
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtEmail;
        private TextBox txtPhone;
        private TextBox txtAddress;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnClear;

        public CustomerManagementForm()
        {
            InitializeControls();
            LoadCustomers();
        }

        private void InitializeControls()
        {
            this.Text = "Customer Management";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;

            dgvCustomers = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 300,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            this.Controls.Add(dgvCustomers);

            Panel inputPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 180
            };

            txtFirstName = new TextBox { Width = 200, Top = 30, Left = 100 };
            txtLastName = new TextBox { Width = 200, Top = 60, Left = 100 };
            txtEmail = new TextBox { Width = 200, Top = 90, Left = 100 };
            txtPhone = new TextBox { Width = 200, Top = 120, Left = 100 };
            txtAddress = new TextBox { Width = 200, Top = 150, Left = 100 };

            inputPanel.Controls.Add(new Label { Text = "First Name:", Top = 30, Left = 20 });
            inputPanel.Controls.Add(new Label { Text = "Last Name:", Top = 60, Left = 20 });
            inputPanel.Controls.Add(new Label { Text = "Email:", Top = 90, Left = 20 });
            inputPanel.Controls.Add(new Label { Text = "Phone:", Top = 120, Left = 20 });
            inputPanel.Controls.Add(new Label { Text = "Address:", Top = 150, Left = 20 });

            inputPanel.Controls.Add(txtFirstName);
            inputPanel.Controls.Add(txtLastName);
            inputPanel.Controls.Add(txtEmail);
            inputPanel.Controls.Add(txtPhone);
            inputPanel.Controls.Add(txtAddress);

            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50
            };

            btnAdd = new Button { Text = "Add", Width = 80, Top = 10, Left = 20 };
            btnUpdate = new Button { Text = "Update", Width = 80, Top = 10, Left = 110 };
            btnDelete = new Button { Text = "Delete", Width = 80, Top = 10, Left = 200 };
            btnClear = new Button { Text = "Clear", Width = 80, Top = 10, Left = 290 };

            buttonPanel.Controls.Add(btnAdd);
            buttonPanel.Controls.Add(btnUpdate);
            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnClear);

            this.Controls.Add(inputPanel);
            this.Controls.Add(buttonPanel);

            btnAdd.Click += BtnAdd_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;
            btnClear.Click += BtnClear_Click;
            dgvCustomers.SelectionChanged += DgvCustomers_SelectionChanged;
        }

        private void LoadCustomers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Customers ORDER BY LastName, FirstName", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvCustomers.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customers: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvCustomers.SelectedRows[0];
                txtFirstName.Text = row.Cells["FirstName"].Value.ToString();
                txtLastName.Text = row.Cells["LastName"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? "";
                txtPhone.Text = row.Cells["Phone"].Value?.ToString() ?? "";
                txtAddress.Text = row.Cells["Address"].Value?.ToString() ?? "";
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("First Name and Last Name are required", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO Customers (FirstName, LastName, Email, Phone, Address) " +
                        "VALUES (@firstName, @lastName, @email, @phone, @address)",
                        conn);

                    cmd.Parameters.AddWithValue("@firstName", txtFirstName.Text.Trim());
                    cmd.Parameters.AddWithValue("@lastName", txtLastName.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(txtEmail.Text) ? DBNull.Value : (object)txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@phone", string.IsNullOrWhiteSpace(txtPhone.Text) ? DBNull.Value : (object)txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@address", string.IsNullOrWhiteSpace(txtAddress.Text) ? DBNull.Value : (object)txtAddress.Text.Trim());

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Customer added successfully", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCustomers();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding customer: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer to update", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("First Name and Last Name are required", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int customerId = Convert.ToInt32(dgvCustomers.SelectedRows[0].Cells["CustomerID"].Value);

                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE Customers SET FirstName=@firstName, LastName=@lastName, " +
                        "Email=@email, Phone=@phone, Address=@address " +
                        "WHERE CustomerID=@customerId",
                        conn);

                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    cmd.Parameters.AddWithValue("@firstName", txtFirstName.Text.Trim());
                    cmd.Parameters.AddWithValue("@lastName", txtLastName.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(txtEmail.Text) ? DBNull.Value : (object)txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@phone", string.IsNullOrWhiteSpace(txtPhone.Text) ? DBNull.Value : (object)txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@address", string.IsNullOrWhiteSpace(txtAddress.Text) ? DBNull.Value : (object)txtAddress.Text.Trim());

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Customer updated successfully", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating customer: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer to delete", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this customer?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                int customerId = Convert.ToInt32(dgvCustomers.SelectedRows[0].Cells["CustomerID"].Value);

                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    SqlCommand checkCmd = new SqlCommand(
                        "SELECT COUNT(*) FROM Rentals WHERE CustomerID=@customerId",
                        conn);
                    checkCmd.Parameters.AddWithValue("@customerId", customerId);
                    int rentalCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (rentalCount > 0)
                    {
                        MessageBox.Show("Cannot delete customer with active rentals", "Delete Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    SqlCommand cmd = new SqlCommand(
                        "DELETE FROM Customers WHERE CustomerID=@customerId",
                        conn);
                    cmd.Parameters.AddWithValue("@customerId", customerId);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Customer deleted successfully", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCustomers();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting customer: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
            dgvCustomers.ClearSelection();
        }

        private void ClearFields()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
        }
    }
}
