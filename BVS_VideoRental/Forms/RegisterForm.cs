using System;
using System.Drawing;
using System.Windows.Forms;
using BVS_VideoRental.Data;
using Microsoft.Data.SqlClient;

namespace BVS_VideoRental.Forms
{
    public class RegisterForm : Form
    {
        private readonly TextBox txtUsername = new TextBox();
        private readonly TextBox txtPassword = new TextBox();
        private readonly TextBox txtConfirmPassword = new TextBox();
        private readonly Button btnRegister = new Button();
        private readonly Label lblError = new Label();

        public string? Username { get; private set; }
        public string? Password { get; private set; }

        public RegisterForm()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Form setup
            Text = "BVS Video Rental - Register";
            ClientSize = new Size(450, 450);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.White;
            Padding = new Padding(30);

            // Title label
            var lblTitle = new Label
            {
                Text = "Create New Account",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 80,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(50, 50, 50)
            };

            // Username field
            var pnlUsername = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                Padding = new Padding(0, 10, 0, 0)
            };

            var lblUser = new Label
            {
                Text = "USERNAME",
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(100, 100, 100)
            };

            txtUsername.Dock = DockStyle.Top;
            txtUsername.Height = 35;
            txtUsername.Font = new Font("Segoe UI", 11);
            txtUsername.BorderStyle = BorderStyle.FixedSingle;

            pnlUsername.Controls.Add(txtUsername);
            pnlUsername.Controls.Add(lblUser);

            // Password field
            var pnlPassword = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                Padding = new Padding(0, 10, 0, 0)
            };

            var lblPass = new Label
            {
                Text = "PASSWORD",
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(100, 100, 100)
            };

            txtPassword.Dock = DockStyle.Top;
            txtPassword.Height = 35;
            txtPassword.Font = new Font("Segoe UI", 11);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.PasswordChar = '•';

            pnlPassword.Controls.Add(txtPassword);
            pnlPassword.Controls.Add(lblPass);

            // Confirm Password field
            var pnlConfirmPassword = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                Padding = new Padding(0, 10, 0, 0)
            };

            var lblConfirmPass = new Label
            {
                Text = "CONFIRM PASSWORD",
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(100, 100, 100)
            };

            txtConfirmPassword.Dock = DockStyle.Top;
            txtConfirmPassword.Height = 35;
            txtConfirmPassword.Font = new Font("Segoe UI", 11);
            txtConfirmPassword.BorderStyle = BorderStyle.FixedSingle;
            txtConfirmPassword.PasswordChar = '•';

            pnlConfirmPassword.Controls.Add(txtConfirmPassword);
            pnlConfirmPassword.Controls.Add(lblConfirmPass);

            // Register button
            btnRegister.Text = "REGISTER";
            btnRegister.Dock = DockStyle.Top;
            btnRegister.Height = 45;
            btnRegister.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnRegister.BackColor = Color.FromArgb(0, 122, 204);
            btnRegister.ForeColor = Color.White;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Cursor = Cursors.Hand;
            btnRegister.Click += BtnRegister_Click;

            // Error label
            lblError.Dock = DockStyle.Top;
            lblError.Height = 40;
            lblError.ForeColor = Color.Red;
            lblError.TextAlign = ContentAlignment.MiddleCenter;
            lblError.Visible = false;

            // Add controls to form
            Controls.Add(btnRegister);
            Controls.Add(pnlConfirmPassword);
            Controls.Add(pnlPassword);
            Controls.Add(pnlUsername);
            Controls.Add(lblError);
            Controls.Add(lblTitle);
        }

        private async void BtnRegister_Click(object? sender, EventArgs e)
        {
            lblError.Visible = false;
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Text;
            var confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrEmpty(username))
            {
                ShowError("Please enter a username");
                return;
            }

            if (password != confirmPassword)
            {
                ShowError("Passwords do not match");
                return;
            }

            if (password.Length < 6)
            {
                ShowError("Password must be at least 6 characters");
                return;
            }

            btnRegister.Enabled = false;
            btnRegister.Text = "REGISTERING...";
            btnRegister.BackColor = Color.Gray;

            try
            {
                DatabaseHelper.AddUser(username, password, "Client");
                Username = username;
                Password = password;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                ShowError("Username already exists");
            }
            catch (SqlException ex)
            {
                ShowError($"Registration failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                ShowError($"An error occurred: {ex.Message}");
            }
            finally
            {
                btnRegister.Enabled = true;
                btnRegister.Text = "REGISTER";
                btnRegister.BackColor = Color.FromArgb(0, 122, 204);
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
        }
    }
}