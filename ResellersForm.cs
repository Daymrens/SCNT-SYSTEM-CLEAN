using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PerfumeInventory
{
    public class ResellersForm : Form
    {
        private DataGridView dgvResellers;
        private TextBox txtSearch;
        private Button btnRefresh, btnAdd, btnEdit, btnDelete;

        public ResellersForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Reseller Management";
            this.Size = new Size(1150, 800);
            this.BackColor = Color.FromArgb(248, 249, 250);

            Label lblTitle = new Label
            {
                Text = "🤝 RESELLERS",
                Location = new Point(30, 30),
                Size = new Size(400, 35),
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            // Main Panel
            Panel pnlMain = new Panel
            {
                Location = new Point(30, 85),
                Size = new Size(1090, 665),
                BackColor = Color.White
            };

            // Search Bar
            txtSearch = new TextBox 
            { 
                Location = new Point(20, 20), 
                Width = 500, 
                Height = 35, 
                Font = new Font("Segoe UI", 11), 
                PlaceholderText = "🔍 Search by name, business, phone, email..." 
            };
            txtSearch.TextChanged += (s, e) => FilterResellers();

            // Action Buttons
            btnRefresh = CreateButton("↺ Refresh", new Point(540, 18), Color.FromArgb(108, 117, 125));
            btnAdd = CreateButton("➕ Add Reseller", new Point(660, 18), Color.FromArgb(25, 135, 84));
            btnEdit = CreateButton("✏️ Edit", new Point(820, 18), Color.FromArgb(13, 110, 253));
            btnDelete = CreateButton("🗑️ Delete", new Point(940, 18), Color.FromArgb(220, 53, 69));

            btnRefresh.Click += (s, e) => LoadData();
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;

            // DataGridView
            dgvResellers = new DataGridView
            {
                Location = new Point(20, 75),
                Size = new Size(1050, 560),
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

            dgvResellers.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Contact", Width = 200 },
                new DataGridViewTextBoxColumn { DataPropertyName = "BusinessName", HeaderText = "Business", Width = 250 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Phone", HeaderText = "Phone", Width = 150 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "Email", Width = 220 },
                new DataGridViewTextBoxColumn { DataPropertyName = "DiscountRate", HeaderText = "Discount %", Width = 100 },
                new DataGridViewCheckBoxColumn { DataPropertyName = "IsActive", HeaderText = "Active", Width = 80 }
            });

            dgvResellers.CellDoubleClick += (s, e) => {
                if (e.RowIndex >= 0 && dgvResellers.Rows[e.RowIndex].DataBoundItem is Reseller reseller)
                {
                    var dialog = new ResellerEditDialog(reseller);
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        LoadData();
                    }
                }
            };

            // Hint Label
            Label lblHint = new Label
            {
                Text = "💡 Double-click a row to edit",
                Location = new Point(20, 640),
                Size = new Size(400, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.FromArgb(108, 117, 125)
            };

            pnlMain.Controls.AddRange(new Control[] { txtSearch, btnRefresh, btnAdd, btnEdit, btnDelete, dgvResellers, lblHint });

            this.Controls.AddRange(new Control[] { lblTitle, pnlMain });
        }

        private Button CreateButton(string text, Point location, Color backColor)
        {
            var btn = new Button
            {
                Text = text,
                Location = location,
                Size = new Size(110, 35),
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
            dgvResellers.DataSource = null;
            dgvResellers.DataSource = DataStore.Resellers.ToList();
        }

        private void FilterResellers()
        {
            string searchTerm = txtSearch.Text.ToLower();
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                LoadData();
                return;
            }

            var filtered = DataStore.Resellers.Where(r =>
                r.Name.ToLower().Contains(searchTerm) ||
                r.BusinessName.ToLower().Contains(searchTerm) ||
                r.Phone.ToLower().Contains(searchTerm) ||
                r.Email.ToLower().Contains(searchTerm)
            ).ToList();

            dgvResellers.DataSource = null;
            dgvResellers.DataSource = filtered;
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            var dialog = new ResellerEditDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (dgvResellers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a reseller to edit.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvResellers.SelectedRows[0].DataBoundItem is Reseller reseller)
            {
                var dialog = new ResellerEditDialog(reseller);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvResellers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a reseller to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this reseller?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes && dgvResellers.SelectedRows[0].DataBoundItem is Reseller reseller)
            {
                DataStore.Resellers.RemoveAll(r => r.Id == reseller.Id);
                DataPersistence.SaveAllData();
                LoadData();
                MessageBox.Show("Reseller deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
