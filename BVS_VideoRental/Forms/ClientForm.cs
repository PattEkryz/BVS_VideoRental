using System;
using System.Drawing;
using System.Windows.Forms;

namespace BVS_VideoRental
{
    public class ClientForm : Form
    {
        public ClientForm()
        {
            Text = "Client Portal";
            Size = new Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;

            var lblWelcome = new Label
            {
                Text = "Welcome to Video Rental System",
                Font = new Font("Arial", 24),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Controls.Add(lblWelcome);
        }
    }
}