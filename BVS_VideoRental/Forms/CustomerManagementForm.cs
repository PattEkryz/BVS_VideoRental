using System.Drawing;
using System.Windows.Forms;
using BVS_VideoRental.Data;

namespace BVS_VideoRental.Forms
{
    public class CustomerManagementForm : Form
    {
        private readonly DataGridView dgvCustomers = new();
        private readonly Button btnAdd = new();
        private readonly Button btnEdit = new();
        private readonly Button btnDelete = new();

        public CustomerManagementForm()
        {
            InitializeUI();
            LoadCustomers();
        }

        private void InitializeUI()
        {
            Text = "Manage Customers";
            MinimumSize = new Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;

            var topPanel = new Panel
            {
                Height = 60,
                Dock = DockStyle.Top,
                BackColor = Color.SteelBlue
            };
            var titleLabel = new Label
            {
                Text = "Manage Customers",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            topPanel.Controls.Add(titleLabel);
            Controls.Add(topPanel);

            dgvCustomers.Dock = DockStyle.Fill;
            dgvCustomers.ReadOnly = true;
            dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Controls.Add(dgvCustomers);

            var bottomPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(20),
                BackColor = Color.LightGray
            };

            ConfigureButton(btnAdd, "Add", Color.MediumSeaGreen, BtnAdd_Click);
            ConfigureButton(btnEdit, "Edit", Color.Goldenrod, BtnEdit_Click);
            ConfigureButton(btnDelete, "Delete", Color.IndianRed, BtnDelete_Click);

            bottomPanel.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete });
            Controls.Add(bottomPanel);
        }

        private void ConfigureButton(Button button, string text, Color backColor, EventHandler clickEvent)
        {
            button.Text = text;
            button.Width = 100;
            button.Height = 40;
            button.BackColor = backColor;
            button.ForeColor = Color.White;
            button.Click += clickEvent;
        }

        private void LoadCustomers()
        {
            dgvCustomers.DataSource = DatabaseHelper.GetCustomers();
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            var detailsForm = new CustomerDetailsForm();
            if (detailsForm.ShowDialog() == DialogResult.OK)
            {
                DatabaseHelper.AddCustomer(detailsForm.CustomerName!, detailsForm.CustomerAddress!, detailsForm.CustomerPhone!);
                LoadCustomers();
            }
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count > 0)
            {
                var row = dgvCustomers.SelectedRows[0];
                int id = Convert.ToInt32(row.Cells["Id"].Value);
                string name = row.Cells["Name"].Value?.ToString() ?? "";
                string address = row.Cells["Address"].Value?.ToString() ?? "";
                string phone = row.Cells["Phone"].Value?.ToString() ?? "";

                var detailsForm = new CustomerDetailsForm(name, address, phone);
                if (detailsForm.ShowDialog() == DialogResult.OK)
                {
                    DatabaseHelper.UpdateCustomer(id, detailsForm.CustomerName!, detailsForm.CustomerAddress!, detailsForm.CustomerPhone!);
                    LoadCustomers();
                }
            }
            else
            {
                MessageBox.Show("Please select a customer to edit.");
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count > 0)
            {
                var result = MessageBox.Show("Are you sure you want to delete?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dgvCustomers.SelectedRows[0].Cells["Id"].Value);
                    DatabaseHelper.DeleteCustomer(id);
                    LoadCustomers();
                }
            }
            else
            {
                MessageBox.Show("Please select a customer to delete.");
            }
        }
    }
}
