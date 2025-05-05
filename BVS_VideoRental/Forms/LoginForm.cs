using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace BVS_VideoRental
{
    public partial class LoginForm : Form
    {
        // UI Controls
        private TextBox txtUsername = new TextBox();
        private TextBox txtPassword = new TextBox();
        private Button btnLogin = new Button();
        private Button btnRegister = new Button();

        private SqlConnection connection;

        public LoginForm()
        {
            InitializeComponents();
            connection = new SqlConnection(GetConnectionString());
        }

        private void InitializeComponents()
        {
            // Form setup
            this.Text = "BVS Video Rental - Login";
            this.Size = new Size(350, 250);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Username Label and TextBox
            Label lblUsername = new Label
            {
                Text = "Username:",
                Location = new Point(50, 50),
                Size = new Size(70, 20)
            };

            txtUsername = new TextBox
            {
                Location = new Point(130, 50),
                Size = new Size(150, 20)
            };

            // Password Label and TextBox
            Label lblPassword = new Label
            {
                Text = "Password:",
                Location = new Point(50, 80),
                Size = new Size(70, 20)
            };

            txtPassword = new TextBox
            {
                Location = new Point(130, 80),
                Size = new Size(150, 20),
                PasswordChar = '*'
            };

            // Login Button
            btnLogin = new Button
            {
                Text = "Login",
                Location = new Point(130, 120),
                Size = new Size(70, 30)
            };
            btnLogin.Click += btnLogin_Click;

            // Register Button
            btnRegister = new Button
            {
                Text = "Register",
                Location = new Point(210, 120),
                Size = new Size(70, 30)
            };
            btnRegister.Click += btnRegister_Click;

            // Add controls to form
            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnRegister);
        }

        private static string GetConnectionString()
        {
            return "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\SKYE\\Documents\\BoggyVideoStore.mdf;Integrated Security=True;";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter both username and password", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        "SELECT UserID, IsAdmin FROM Users WHERE Username=@username AND Password=@password",
                        conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bool isAdmin = reader.GetBoolean(reader.GetOrdinal("IsAdmin"));
                                MainForm mainForm = new MainForm(isAdmin);
                                mainForm.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Invalid username or password", "Login Failed",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Login Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Login Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegistrationForm regForm = new RegistrationForm();
            regForm.ShowDialog();
        }
    }
}