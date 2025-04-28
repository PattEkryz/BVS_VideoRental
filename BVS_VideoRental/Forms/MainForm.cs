using System;
using System.Drawing;
using System.Windows.Forms;
using BVS_VideoRental.Forms;

namespace BVS_VideoRental
{
    public class MainForm : Form
    {
        public MainForm()
        {
            Text = "Admin Dashboard";
            Size = new Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;

            var menuStrip = new MenuStrip();
            var customersMenu = new ToolStripMenuItem("Customers");
            customersMenu.Click += (s, e) => new CustomerManagementForm().ShowDialog();
            menuStrip.Items.Add(customersMenu);

            Controls.Add(menuStrip);
        }
    }
}