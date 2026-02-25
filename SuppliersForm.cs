using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PerfumeInventory
{
    public class SuppliersForm : Form
    {
        private DataGridView dgvSuppliers;
        private TextBox txtSearch;
        private Button btnAdd, btnEdit, btnDelete, btnRefresh;

        public SuppliersForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Supplier Management";
            this.Size = new Size(1150, 800);
            this.BackColor = Color.FromArgb(248, 249, 250);

            // Header
            Label lblTitle = new Label
            {
                Text = "📦 SUPPLIERS",
                Location = new Point(30, 30),
                Size = new Size(400, 35),
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            // Search Bar
            Panel pnlSearch = new Panel
            {
                Location = new Point(30, 85),
                Size = new Size(1090, 60),
                BackColor = Color.White
            };

            txtSearch = new TextBox 
            { 
                Location = new Point(20, 15), 
                Width = 400, 
                Height = 30,
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "🔍 Search by name, contact, phone, email..."
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            btnRefresh = CreateButton("↺ Refresh", new Point(440, 13), Color.FromArgb(108, 117, 125), 110);
            btnRefresh.Click += (s, e) => { txtSearch.Clear(); LoadData(); };
            btnRefresh.FlatAppearance.BorderSize = 0;

            btnAdd = CreateButton("➕ Add Supplier", new Point(940, 13), Color.FromArgb(76, 175, 80), 130);
            btnAdd.Click += BtnAdd_Click;
            btnAdd.FlatAppearance.BorderSize = 0;

            pnlSearch.Controls.AddRange(new Control[] { txtSearch, btnRefresh, btnAdd });

            // Data Grid
            Panel pnlGrid = new Panel
            {
                Location = new Point(30, 165),
                Size = new Size(1090, 585),
                BackColor = Color.White
            };

            dgvSuppliers = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(1050, 490),
                AutoGenerateColumns = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
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

            dgvSuppliers.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Supplier Name", Width = 200 },
                new DataGridViewTextBoxColumn { DataPropertyName = "ContactPerson", HeaderText = "Contact Person", Width = 180 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Phone", HeaderText = "Phone", Width = 140 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "Email", Width = 250 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Address", HeaderText = "Address", Width = 250 }
            });

            dgvSuppliers.CellDoubleClick += DgvSuppliers_CellDoubleClick;

            btnEdit = CreateButton("✏️ Edit", new Point(20, 525), Color.FromArgb(13, 110, 253), 100);
            btnEdit.Click += BtnEdit_Click;
            btnEdit.FlatAppearance.BorderSize = 0;

            btnDelete = CreateButton("🗑️ Delete", new Point(130, 525), Color.FromArgb(220, 53, 69), 100);
            btnDelete.Click += BtnDelete_Click;
            btnDelete.FlatAppearance.BorderSize = 0;

            Label lblHint = new Label
            {
                Text = "💡 Double-click a row to edit",
                Location = new Point(250, 530),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(108, 117, 125)
            };

            pnlGrid.Controls.AddRange(new Control[] { dgvSuppliers, btnEdit, btnDelete, lblHint });

            this.Controls.AddRange(new Control[] { lblTitle, pnlSearch, pnlGrid });
        }

        private Button CreateButton(string text, Point location, Color backColor, int width = 80)
        {
            var btn = new Button
            {
                Text = text,
                Location = location,
                Size = new Size(width, 35),
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Light(backColor, 0.2f);
            btn.MouseLeave += (s, e) => btn.BackColor = backColor;
            return btn;
        }

        private void LoadData()
        {
            string searchTerm = txtSearch.Text.ToLower();
            var suppliers = DataStore.Suppliers.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                suppliers = suppliers.Where(s =>
                    s.Name.ToLower().Contains(searchTerm) ||
                    s.ContactPerson.ToLower().Contains(searchTerm) ||
                    s.Phone.Contains(searchTerm) ||
                    s.Email.ToLower().Contains(searchTerm)
                );
            }

            dgvSuppliers.DataSource = null;
            dgvSuppliers.DataSource = suppliers.ToList();
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            LoadData();
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            var dialog = new SupplierEditDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (dgvSuppliers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a supplier to edit.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvSuppliers.SelectedRows[0].DataBoundItem is Supplier supplier)
            {
                var dialog = new SupplierEditDialog(supplier);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void DgvSuppliers_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvSuppliers.Rows[e.RowIndex].DataBoundItem is Supplier supplier)
            {
                var dialog = new SupplierEditDialog(supplier);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvSuppliers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a supplier to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this supplier?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes && dgvSuppliers.SelectedRows[0].DataBoundItem is Supplier supplier)
            {
                DataStore.Suppliers.RemoveAll(s => s.Id == supplier.Id);
                DataPersistence.SaveAllData();
                LoadData();
                MessageBox.Show("Supplier deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}