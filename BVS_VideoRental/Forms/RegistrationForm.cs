using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace BVS_VideoRental
{
    public partial class RegistrationForm : Form
    {
        
        private TextBox txtUsername = new TextBox();
        private TextBox txtPassword = new TextBox();
        private TextBox txtFullName = new TextBox();
        private Button btnRegister = new Button();
        private Button btnCancel = new Button();

        private SqlConnection connection;

        public RegistrationForm()
        {
            
            InitializeControls();
            connection = new SqlConnection(DatabaseConfig.ConnectionString);
        }

        private void InitializeControls()
        {
            // Form setup
            this.Text = "BVS Video Rental - Registration";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Username controls
            Label lblUsername = new Label
            {
                Text = "Username:",
                Location = new Point(50, 50),
                Size = new Size(80, 20)
            };

            txtUsername = new TextBox
            {
                Location = new Point(150, 50),
                Size = new Size(200, 20)
            };

            // Password controls
            Label lblPassword = new Label
            {
                Text = "Password:",
                Location = new Point(50, 80),
                Size = new Size(80, 20)
            };

            txtPassword = new TextBox
            {
                Location = new Point(150, 80),
                Size = new Size(200, 20),
                PasswordChar = '*'
            };

            // Full Name controls
            Label lblFullName = new Label
            {
                Text = "Full Name:",
                Location = new Point(50, 110),
                Size = new Size(80, 20)
            };

            txtFullName = new TextBox
            {
                Location = new Point(150, 110),
                Size = new Size(200, 20)
            };

            // Buttons
            btnRegister = new Button
            {
                Text = "Register",
                Location = new Point(150, 150),
                Size = new Size(100, 30)
            };
            btnRegister.Click += btnRegister_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(260, 150),
                Size = new Size(100, 30)
            };
            btnCancel.Click += (s, e) => this.Close();

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                lblUsername, txtUsername,
                lblPassword, txtPassword,
                lblFullName, txtFullName,
                btnRegister, btnCancel
            });
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Please fill all required fields", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        "INSERT INTO Users (Username, Password, FullName) " +
                        "VALUES (@username, @password, @fullName)", conn))
                    {
                        cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", txtPassword.Text); // Hash in production
                        cmd.Parameters.AddWithValue("@fullName", txtFullName.Text.Trim());

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Registration successful!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // Unique constraint violation
                {
                    MessageBox.Show("Username already exists", "Registration Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Database error: {ex.Message}", "Registration Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}