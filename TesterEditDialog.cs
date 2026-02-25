using System;
using System.Drawing;
using System.Windows.Forms;

namespace PerfumeInventory
{
    public class TesterEditDialog : Form
    {
        private TextBox txtName, txtBrand, txtCategory, txtNotes;
        private ComboBox cmbStatus;
        private Button btnSave, btnCancel;
        private Tester? _tester;

        public TesterEditDialog(Tester? tester = null)
        {
            _tester = tester;
            InitializeComponent();
            if (_tester != null)
            {
                LoadTesterData();
            }
        }

        private void InitializeComponent()
        {
            this.Text = _tester == null ? "Add Tester" : "Edit Tester";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            // Name
            Label lblName = new Label { Text = "Tester Name:", Location = new Point(20, 20), Size = new Size(120, 25), Font = new Font("Segoe UI", 10) };
            txtName = new TextBox { Location = new Point(150, 18), Size = new Size(310, 25), Font = new Font("Segoe UI", 10) };

            // Brand
            Label lblBrand = new Label { Text = "Brand:", Location = new Point(20, 60), Size = new Size(120, 25), Font = new Font("Segoe UI", 10) };
            txtBrand = new TextBox { Location = new Point(150, 58), Size = new Size(310, 25), Font = new Font("Segoe UI", 10) };

            // Category
            Label lblCategory = new Label { Text = "Category:", Location = new Point(20, 100), Size = new Size(120, 25), Font = new Font("Segoe UI", 10) };
            txtCategory = new TextBox { Location = new Point(150, 98), Size = new Size(310, 25), Font = new Font("Segoe UI", 10) };

            // Status
            Label lblStatus = new Label { Text = "Status:", Location = new Point(20, 140), Size = new Size(120, 25), Font = new Font("Segoe UI", 10) };
            cmbStatus = new ComboBox 
            { 
                Location = new Point(150, 138), 
                Size = new Size(310, 25), 
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbStatus.Items.AddRange(new object[] { "Available", "In Use", "Low", "Empty", "Damaged" });
            cmbStatus.SelectedIndex = 0;

            // Notes
            Label lblNotes = new Label { Text = "Notes:", Location = new Point(20, 180), Size = new Size(120, 25), Font = new Font("Segoe UI", 10) };
            txtNotes = new TextBox 
            { 
                Location = new Point(150, 178), 
                Size = new Size(310, 80), 
                Font = new Font("Segoe UI", 10),
                Multiline = true
            };

            // Buttons
            btnSave = new Button
            {
                Text = "💾 Save",
                Location = new Point(250, 280),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "❌ Cancel",
                Location = new Point(360, 280),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[] {
                lblName, txtName,
                lblBrand, txtBrand,
                lblCategory, txtCategory,
                lblStatus, cmbStatus,
                lblNotes, txtNotes,
                btnSave, btnCancel
            });
        }

        private void LoadTesterData()
        {
            if (_tester != null)
            {
                txtName.Text = _tester.Name;
                txtBrand.Text = _tester.Brand;
                txtCategory.Text = _tester.Category;
                cmbStatus.SelectedItem = _tester.Status;
                txtNotes.Text = _tester.Notes;
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a tester name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_tester == null)
            {
                // Add new tester
                _tester = new Tester
                {
                    Id = DataStore.GetNextId(DataStore.Testers),
                    Name = txtName.Text.Trim(),
                    Brand = txtBrand.Text.Trim(),
                    Category = txtCategory.Text.Trim(),
                    Status = cmbStatus.SelectedItem?.ToString() ?? "Available",
                    Notes = txtNotes.Text.Trim(),
                    CreatedDate = DateTime.Now
                };
                DataStore.Testers.Add(_tester);
            }
            else
            {
                // Update existing tester
                _tester.Name = txtName.Text.Trim();
                _tester.Brand = txtBrand.Text.Trim();
                _tester.Category = txtCategory.Text.Trim();
                _tester.Status = cmbStatus.SelectedItem?.ToString() ?? "Available";
                _tester.Notes = txtNotes.Text.Trim();
            }

            DataPersistence.SaveAllData();
            this.DialogResult = DialogResult.OK;
        }
    }
}
