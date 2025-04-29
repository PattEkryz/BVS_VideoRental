using System;
using System.Drawing;
using System.Windows.Forms;
using BVS_VideoRental.Data;

namespace BVS_VideoRental.Forms
{
    public class LoginForm : Form
    {
        private readonly TextBox txtUsername = new TextBox();
        private readonly TextBox txtPassword = new TextBox();
        private readonly Button btnLogin = new Button();
        private readonly LinkLabel linkRegister = new LinkLabel();
        private readonly Label lblError = new Label();

        public User? AuthenticatedUser { get; private set; }

        public LoginForm()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Form setup
            Text = "BVS Video Rental - Login";
            ClientSize = new Size(450, 350);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.White;
            Padding = new Padding(30);

            // Title label
            var lblTitle = new Label
            {
                Text = "Welcome to BVS Video Rental",
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

            // Login button
            btnLogin.Text = "LOGIN";
            btnLogin.Dock = DockStyle.Top;
            btnLogin.Height = 45;
            btnLogin.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnLogin.BackColor = Color.FromArgb(0, 122, 204);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Click += BtnLogin_Click;

            // Register link
            linkRegister.Text = "Don't have an account? Register here";
            linkRegister.Dock = DockStyle.Top;
            linkRegister.Padding = new Padding(0, 15, 0, 0);
            linkRegister.TextAlign = ContentAlignment.MiddleCenter;
            linkRegister.LinkColor = Color.FromArgb(0, 122, 204);
            linkRegister.ActiveLinkColor = Color.FromArgb(0, 86, 143);
            linkRegister.LinkClicked += LinkRegister_LinkClicked;

            // Error label
            lblError.Dock = DockStyle.Top;
            lblError.Height = 40;
            lblError.ForeColor = Color.Red;
            lblError.TextAlign = ContentAlignment.MiddleCenter;
            lblError.Visible = false;

            // Add controls to form
            Controls.Add(linkRegister);
            Controls.Add(btnLogin);
            Controls.Add(pnlPassword);
            Controls.Add(pnlUsername);
            Controls.Add(lblError);
            Controls.Add(lblTitle);
        }

        private async void BtnLogin_Click(object? sender, EventArgs e)
        {
            lblError.Visible = false;

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                ShowError("Please enter username");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ShowError("Please enter password");
                return;
            }

            btnLogin.Enabled = false;
            btnLogin.Text = "AUTHENTICATING...";
            btnLogin.BackColor = Color.Gray;

            try
            {
                AuthenticatedUser = DatabaseHelper.AuthenticateUser(txtUsername.Text, txtPassword.Text);

                if (AuthenticatedUser != null)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    ShowError("Invalid username or password");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Login error: {ex.Message}");
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "LOGIN";
                btnLogin.BackColor = Color.FromArgb(0, 122, 204);
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
        }

        private void LinkRegister_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            var registerForm = new RegisterForm();
            if (registerForm.ShowDialog() == DialogResult.OK)
            {
                txtUsername.Text = registerForm.Username;
                txtPassword.Focus();
            }
        }
    }
}