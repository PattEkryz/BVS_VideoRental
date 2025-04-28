using System;
using System.Drawing;
using System.Windows.Forms;
using BVS_VideoRental.Data;
using Microsoft.Data.SqlClient;

namespace BVS_VideoRental.Forms
{
    public class RegisterForm : Form
    {
        private readonly TextBox txtUsername = new();
        private readonly TextBox txtPassword = new();
        private readonly TextBox txtConfirmPassword = new();
        private readonly Button btnRegister = new();

        public string? Username { get; private set; }
        public string? Password { get; private set; }

        public RegisterForm()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            // [Keep all your existing UI code]
        }

        private void BtnRegister_Click(object? sender, EventArgs e)
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Text;
            var confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Please enter a username");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match");
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters");
                return;
            }

            try
            {
                DatabaseHelper.AddUser(username, password, "Client");
                MessageBox.Show("Registration successful! You can now login.");
                Username = username;
                Password = password;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Registration failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}