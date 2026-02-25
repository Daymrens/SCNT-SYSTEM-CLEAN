using System;
using System.Drawing;
using System.Windows.Forms;

namespace PerfumeInventory
{
    public class SupplierEditDialog : Form
    {
        private TextBox txtName, txtContactPerson, txtPhone, txtEmail, txtAddress;
        private Button btnSave, btnCancel;
        private Supplier? _supplier;

        public SupplierEditDialog(Supplier? supplier = null)
        {
            _supplier = supplier;
            InitializeComponent();
            if (_supplier != null)
            {
                LoadSupplierData();
            }
        }

        private void InitializeComponent()
        {
            this.Text = _supplier == null ? "Add Supplier" : "Edit Supplier";
            this.Size = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(240, 240, 245);

            Label lblTitle = new Label
            {
                Text = _supplier == null ? "Add New Supplier" : "Edit Supplier",
                Location = new Point(20, 20),
                Size = new Size(450, 30),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(63, 81, 181)
            };

            int yPos = 70;
            AddFormField("Supplier Name:", out txtName, ref yPos);
            AddFormField("Contact Person:", out txtContactPerson, ref yPos);
            AddFormField("Phone:", out txtPhone, ref yPos);
            AddFormField("Email:", out txtEmail, ref yPos);
            
            Label lblAddress = new Label { Text = "Address:", Location = new Point(20, yPos), Width = 100, Font = new Font("Segoe UI", 10) };
            txtAddress = new TextBox { Location = new Point(20, yPos + 25), Width = 440, Height = 60, Font = new Font("Segoe UI", 10), Multiline = true };
            this.Controls.AddRange(new Control[] { lblAddress, txtAddress });
            yPos += 95;

            btnSave = new Button
            {
                Text = "Save",
                Location = new Point(260, yPos),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(370, yPos),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(158, 158, 158),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[] { lblTitle, btnSave, btnCancel });
        }

        private void AddFormField(string label, out TextBox textBox, ref int yPos)
        {
            Label lbl = new Label { Text = label, Location = new Point(20, yPos), Width = 150, Font = new Font("Segoe UI", 10) };
            textBox = new TextBox { Location = new Point(20, yPos + 25), Width = 440, Font = new Font("Segoe UI", 10) };
            this.Controls.AddRange(new Control[] { lbl, textBox });
            yPos += 65;
        }

        private void LoadSupplierData()
        {
            if (_supplier != null)
            {
                txtName.Text = _supplier.Name;
                txtContactPerson.Text = _supplier.ContactPerson;
                txtPhone.Text = _supplier.Phone;
                txtEmail.Text = _supplier.Email;
                txtAddress.Text = _supplier.Address;
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter supplier name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_supplier == null)
            {
                _supplier = new Supplier
                {
                    Id = DataStore.GetNextId(DataStore.Suppliers)
                };
                DataStore.Suppliers.Add(_supplier);
            }

            _supplier.Name = txtName.Text;
            _supplier.ContactPerson = txtContactPerson.Text;
            _supplier.Phone = txtPhone.Text;
            _supplier.Email = txtEmail.Text;
            _supplier.Address = txtAddress.Text;

            DataPersistence.SaveAllData();
            this.DialogResult = DialogResult.OK;
        }
    }
}
