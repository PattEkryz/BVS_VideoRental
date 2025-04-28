using System.Drawing;
using System.Windows.Forms;

namespace BVS_VideoRental.Forms
{
    public class CustomerDetailsForm : Form
    {
        private readonly TextBox txtName = new();
        private readonly TextBox txtAddress = new();
        private readonly TextBox txtPhone = new();
        private readonly Button btnSave = new();
        private readonly Button btnCancel = new();

        public string? CustomerName { get; private set; }
        public string? CustomerAddress { get; private set; }
        public string? CustomerPhone { get; private set; }

        public CustomerDetailsForm(string? name = "", string? address = "", string? phone = "")
        {
            InitializeUI();

            txtName.Text = name ?? "";
            txtAddress.Text = address ?? "";
            txtPhone.Text = phone ?? "";
        }

        private void InitializeUI()
        {
            Text = "Customer Details";
            MinimumSize = new Size(400, 300);
            StartPosition = FormStartPosition.CenterParent;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 4,
                ColumnCount = 2,
                Padding = new Padding(20),
                AutoSize = true
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));

            layout.Controls.Add(new Label { Text = "Name:", TextAlign = ContentAlignment.MiddleRight }, 0, 0);
            layout.Controls.Add(txtName, 1, 0);

            layout.Controls.Add(new Label { Text = "Address:", TextAlign = ContentAlignment.MiddleRight }, 0, 1);
            layout.Controls.Add(txtAddress, 1, 1);

            layout.Controls.Add(new Label { Text = "Phone:", TextAlign = ContentAlignment.MiddleRight }, 0, 2);
            layout.Controls.Add(txtPhone, 1, 2);

            var buttonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Fill
            };

            btnSave.Text = "Save";
            btnSave.Click += BtnSave_Click;

            btnCancel.Text = "Cancel";
            btnCancel.Click += (_, _) => DialogResult = DialogResult.Cancel;

            buttonPanel.Controls.Add(btnSave);
            buttonPanel.Controls.Add(btnCancel);

            layout.Controls.Add(buttonPanel, 1, 3);

            Controls.Add(layout);
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            CustomerName = txtName.Text.Trim();
            CustomerAddress = txtAddress.Text.Trim();
            CustomerPhone = txtPhone.Text.Trim();

            if (string.IsNullOrEmpty(CustomerName))
            {
                MessageBox.Show("Name is required.");
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
