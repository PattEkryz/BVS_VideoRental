using System;
using System.Drawing;
using System.Windows.Forms;
using BVS_VideoRental.Data;

namespace BVS_VideoRental.Forms
{
    public class LoginForm : Form
    {
        private readonly TextBox txtUsername = new();
        private readonly TextBox txtPassword = new();
        private readonly Button btnLogin = new();
        private readonly LinkLabel linkRegister = new();

        public User? AuthenticatedUser { get; private set; }

        public LoginForm()
        {
            // Basic form setup
            Text = "BVS Video Rental - Login";
            ClientSize = new Size(400, 250);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            // Main layout panel
            var mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                RowCount = 5,
                ColumnCount = 2
            };

            // Title label
            var lblTitle = new Label
            {
                Text = "Login to BVS Video Rental",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            mainPanel.Controls.Add(lblTitle, 0, 0);
            mainPanel.SetColumnSpan(lblTitle, 2);

            // Username controls
            mainPanel.Controls.Add(new Label { Text = "Username:", TextAlign = ContentAlignment.MiddleRight }, 0, 1);
            txtUsername.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(txtUsername, 1, 1);

            // Password controls
            mainPanel.Controls.Add(new Label { Text = "Password:", TextAlign = ContentAlignment.MiddleRight }, 0, 2);
            txtPassword.Dock = DockStyle.Fill;
            txtPassword.PasswordChar = '*';
            mainPanel.Controls.Add(txtPassword, 1, 2);

            // Login button
            btnLogin.Text = "Login";
            btnLogin.Dock = DockStyle.Fill;
            btnLogin.Click += BtnLogin_Click;
            mainPanel.Controls.Add(btnLogin, 0, 3);
            mainPanel.SetColumnSpan(btnLogin, 2);

            // Register link
            linkRegister.Text = "Register as new client";
            linkRegister.TextAlign = ContentAlignment.MiddleCenter;
            linkRegister.Dock = DockStyle.Fill;
            linkRegister.LinkClicked += LinkRegister_LinkClicked;
            mainPanel.Controls.Add(linkRegister, 0, 4);
            mainPanel.SetColumnSpan(linkRegister, 2);

            // Add main panel to form
            Controls.Add(mainPanel);
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            // [Keep your existing login logic]
        }

        private void LinkRegister_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            // [Keep your existing registration logic]
        }
    }
}