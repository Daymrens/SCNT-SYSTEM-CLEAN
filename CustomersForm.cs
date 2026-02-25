using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PerfumeInventory
{
    public class CustomersForm : Form
    {
        private DataGridView dgvCustomers;
        private TextBox txtSearch;
        private Button btnAdd, btnEdit, btnDelete, btnRefresh;

        public CustomersForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Customers";
            this.Size = new Size(1150, 800);
            this.BackColor = Color.FromArgb(248, 249, 250);

            Label lblTitle = new Label
            {
                Text = "👤 CUSTOMERS",
                Location = new Point(30, 30),
                Size = new Size(400, 35),
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            // Main Panel
            Panel pnlMain = new Panel
            {
                Location = new Point(30, 85),
                Size = new Size(1090, 650),
                BackColor = Color.White
            };

            // Search bar
            txtSearch = new TextBox
            {
                Location = new Point(20, 20),
                Size = new Size(300, 35),
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(108, 117, 125)
            };
            txtSearch.Text = "🔍 Search by name, phone, email...";
            txtSearch.GotFocus += (s, e) => { if (txtSearch.Text.StartsWith("🔍")) txtSearch.Text = ""; txtSearch.ForeColor = Color.FromArgb(33, 37, 41); };
            txtSearch.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtSearch.Text)) { txtSearch.Text = "🔍 Search by name, phone, email..."; txtSearch.ForeColor = Color.FromArgb(108, 117, 125); } };
            txtSearch.TextChanged += (s, e) => { if (!txtSearch.Text.StartsWith("🔍")) BtnSearch_Click(s, e); };

            // Action buttons
            btnRefresh = new Button
            {
                Text = "↺ Refresh",
                Location = new Point(340, 18),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => LoadData();
            btnRefresh.MouseEnter += (s, e) => btnRefresh.BackColor = Color.FromArgb(90, 98, 104);
            btnRefresh.MouseLeave += (s, e) => btnRefresh.BackColor = Color.FromArgb(108, 117, 125);

            btnAdd = new Button
            {
                Text = "➕ Add Customer",
                Location = new Point(480, 18),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(25, 135, 84),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAdd_Click;
            btnAdd.MouseEnter += (s, e) => btnAdd.BackColor = Color.FromArgb(20, 108, 67);
            btnAdd.MouseLeave += (s, e) => btnAdd.BackColor = Color.FromArgb(25, 135, 84);

            btnEdit = new Button
            {
                Text = "✏️ Edit",
                Location = new Point(650, 18),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += BtnEdit_Click;
            btnEdit.MouseEnter += (s, e) => btnEdit.BackColor = Color.FromArgb(10, 88, 202);
            btnEdit.MouseLeave += (s, e) => btnEdit.BackColor = Color.FromArgb(13, 110, 253);

            btnDelete = new Button
            {
                Text = "🗑️ Delete",
                Location = new Point(770, 18),
                Size = new Size(110, 35),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += BtnDelete_Click;
            btnDelete.MouseEnter += (s, e) => btnDelete.BackColor = Color.FromArgb(176, 42, 55);
            btnDelete.MouseLeave += (s, e) => btnDelete.BackColor = Color.FromArgb(220, 53, 69);

            // DataGridView
            dgvCustomers = new DataGridView
            {
                Location = new Point(20, 70),
                Size = new Size(1050, 560),
                AutoGenerateColumns = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 10),
                GridColor = Color.FromArgb(222, 226, 230),
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(248, 249, 250),
                    ForeColor = Color.FromArgb(33, 37, 41),
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    Padding = new Padding(10),
                    SelectionBackColor = Color.FromArgb(248, 249, 250),
                    SelectionForeColor = Color.FromArgb(33, 37, 41),
                    Alignment = DataGridViewContentAlignment.MiddleLeft
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    SelectionBackColor = Color.FromArgb(13, 110, 253),
                    SelectionForeColor = Color.White,
                    Padding = new Padding(10, 8, 10, 8),
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(33, 37, 41)
                },
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle 
                { 
                    BackColor = Color.White,
                    SelectionBackColor = Color.FromArgb(13, 110, 253),
                    SelectionForeColor = Color.White,
                    Padding = new Padding(10, 8, 10, 8)
                },
                ColumnHeadersHeight = 45,
                RowTemplate = { Height = 40 },
                EnableHeadersVisualStyles = false,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            };

            dgvCustomers.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Name", Width = 250 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Phone", HeaderText = "Phone", Width = 180 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "Email", Width = 300 },
                new DataGridViewTextBoxColumn { DataPropertyName = "LoyaltyPoints", HeaderText = "Loyalty Points", Width = 150 }
            });

            dgvCustomers.CellDoubleClick += (s, e) => {
                if (e.RowIndex >= 0 && dgvCustomers.Rows[e.RowIndex].DataBoundItem is Customer customer)
                {
                    var dialog = new CustomerEditDialog(customer);
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        LoadData();
                    }
                }
            };

            Label lblHint = new Label
            {
                Text = "💡 Double-click a row to edit",
                Location = new Point(900, 25),
                Size = new Size(180, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.FromArgb(108, 117, 125)
            };

            pnlMain.Controls.AddRange(new Control[] { txtSearch, btnRefresh, btnAdd, btnEdit, btnDelete, dgvCustomers, lblHint });

            this.Controls.AddRange(new Control[] { lblTitle, pnlMain });
        }

        private void LoadData()
        {
            dgvCustomers.DataSource = null;
            dgvCustomers.DataSource = DataStore.Customers.ToList();
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            var dialog = new CustomerEditDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer to edit.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvCustomers.SelectedRows[0].DataBoundItem is Customer customer)
            {
                var dialog = new CustomerEditDialog(customer);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this customer?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes && dgvCustomers.SelectedRows[0].DataBoundItem is Customer customer)
            {
                DataStore.Customers.RemoveAll(c => c.Id == customer.Id);
                DataPersistence.SaveAllData();
                LoadData();
                MessageBox.Show("Customer deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnSearch_Click(object? sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.ToLower();
            if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm.StartsWith("🔍"))
            {
                LoadData();
                return;
            }

            var filtered = DataStore.Customers.Where(c =>
                c.Name.ToLower().Contains(searchTerm) ||
                c.Phone.Contains(searchTerm) ||
                c.Email.ToLower().Contains(searchTerm)
            ).ToList();

            dgvCustomers.DataSource = null;
            dgvCustomers.DataSource = filtered;
        }
    }
}