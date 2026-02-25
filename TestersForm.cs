using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PerfumeInventory
{
    public class TestersForm : Form
    {
        private DataGridView dgvTesters;
        private Button btnAdd, btnEdit, btnDelete, btnRefresh;
        private TextBox txtSearch;
        private Label lblTotal;

        public TestersForm()
        {
            InitializeComponent();
            LoadTesters();
        }

        private void InitializeComponent()
        {
            this.Text = "Testers Management";
            this.Size = new Size(1000, 700);
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Title
            Label lblTitle = new Label
            {
                Text = "🧪 TESTERS",
                Location = new Point(30, 30),
                Size = new Size(400, 35),
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            // Search Panel
            Panel pnlSearch = new Panel
            {
                Location = new Point(30, 85),
                Size = new Size(940, 60),
                BackColor = Color.White
            };

            txtSearch = new TextBox
            {
                Location = new Point(20, 15),
                Size = new Size(400, 30),
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "🔍 Search by name, brand, category..."
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            btnRefresh = new Button
            {
                Text = "↺ Refresh",
                Location = new Point(440, 13),
                Size = new Size(110, 35),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += BtnRefresh_Click;

            lblTotal = new Label
            {
                Text = "Total Testers: 0",
                Location = new Point(570, 18),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 135, 84)
            };

            pnlSearch.Controls.AddRange(new Control[] { txtSearch, btnRefresh, lblTotal });

            // Buttons Panel
            Panel pnlButtons = new Panel
            {
                Location = new Point(30, 165),
                Size = new Size(940, 60),
                BackColor = Color.White
            };

            btnAdd = new Button
            {
                Text = "➕ Add Tester",
                Location = new Point(20, 13),
                Size = new Size(130, 35),
                BackColor = Color.FromArgb(25, 135, 84),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAdd_Click;

            btnEdit = new Button
            {
                Text = "✏️ Edit",
                Location = new Point(160, 13),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += BtnEdit_Click;

            btnDelete = new Button
            {
                Text = "🗑️ Delete",
                Location = new Point(270, 13),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += BtnDelete_Click;

            Label lblHint = new Label
            {
                Text = "💡 Double-click a row to edit",
                Location = new Point(390, 18),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(108, 117, 125)
            };

            pnlButtons.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete, lblHint });

            // DataGridView
            dgvTesters = new DataGridView
            {
                Location = new Point(30, 245),
                Size = new Size(940, 415),
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

            dgvTesters.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Tester Name", DataPropertyName = "Name", Width = 280 });
            dgvTesters.Columns.Add(new DataGridViewTextBoxColumn { Name = "Brand", HeaderText = "Brand", DataPropertyName = "Brand", Width = 150 });
            dgvTesters.Columns.Add(new DataGridViewTextBoxColumn { Name = "Category", HeaderText = "Category", DataPropertyName = "Category", Width = 150 });
            dgvTesters.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status", DataPropertyName = "Status", Width = 120 });
            dgvTesters.Columns.Add(new DataGridViewTextBoxColumn { Name = "Notes", HeaderText = "Notes", DataPropertyName = "Notes", Width = 210 });

            dgvTesters.DoubleClick += DgvTesters_DoubleClick;

            this.Controls.AddRange(new Control[] { lblTitle, pnlSearch, pnlButtons, dgvTesters });
            
            // Apply role-based access control
            ApplyRoleBasedAccess();
        }

        private void ApplyRoleBasedAccess()
        {
            // Check if current user is a Cashier
            if (DataStore.CurrentUser?.Role == "Cashier")
            {
                // Hide edit/delete/add buttons for cashiers
                btnAdd.Visible = false;
                btnEdit.Visible = false;
                btnDelete.Visible = false;
                
                // Update hint text to indicate view-only mode
                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is Panel panel)
                    {
                        foreach (Control innerCtrl in panel.Controls)
                        {
                            if (innerCtrl is Label lbl && lbl.Text.Contains("Double-click"))
                            {
                                lbl.Text = "📖 View-Only Mode";
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void LoadTesters()
        {
            var testers = DataStore.Testers.Select(t => new
            {
                t.Id,
                t.Name,
                t.Brand,
                t.Category,
                t.Status,
                t.Notes
            }).ToList();

            dgvTesters.DataSource = testers;
            lblTotal.Text = $"Total Testers: {testers.Count}";
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            string search = txtSearch.Text.ToLower();
            var filtered = DataStore.Testers
                .Where(t => t.Name.ToLower().Contains(search) || 
                           t.Brand.ToLower().Contains(search) ||
                           t.Category.ToLower().Contains(search))
                .Select(t => new
                {
                    t.Id,
                    t.Name,
                    t.Brand,
                    t.Category,
                    t.Status,
                    t.Notes
                }).ToList();

            dgvTesters.DataSource = filtered;
            lblTotal.Text = $"Total Testers: {filtered.Count}";
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadTesters();
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            var dialog = new TesterEditDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadTesters();
            }
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (dgvTesters.SelectedRows.Count > 0)
            {
                var rowData = dgvTesters.SelectedRows[0].DataBoundItem;
                if (rowData != null)
                {
                    // Get the Id from the anonymous type using reflection
                    var idProperty = rowData.GetType().GetProperty("Id");
                    if (idProperty != null)
                    {
                        int testerId = (int)idProperty.GetValue(rowData)!;
                        var tester = DataStore.Testers.FirstOrDefault(t => t.Id == testerId);
                        if (tester != null)
                        {
                            var dialog = new TesterEditDialog(tester);
                            if (dialog.ShowDialog() == DialogResult.OK)
                            {
                                LoadTesters();
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a tester to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DgvTesters_DoubleClick(object? sender, EventArgs e)
        {
            // Prevent editing for cashiers
            if (DataStore.CurrentUser?.Role == "Cashier")
            {
                return;
            }
            
            BtnEdit_Click(sender, e);
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvTesters.SelectedRows.Count > 0)
            {
                var result = MessageBox.Show("Are you sure you want to delete this tester?", "Confirm Delete", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    var rowData = dgvTesters.SelectedRows[0].DataBoundItem;
                    if (rowData != null)
                    {
                        // Get the Id from the anonymous type using reflection
                        var idProperty = rowData.GetType().GetProperty("Id");
                        if (idProperty != null)
                        {
                            int testerId = (int)idProperty.GetValue(rowData)!;
                            var tester = DataStore.Testers.FirstOrDefault(t => t.Id == testerId);
                            if (tester != null)
                            {
                                DataStore.Testers.Remove(tester);
                                DataPersistence.SaveAllData();
                                LoadTesters();
                                MessageBox.Show("Tester deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a tester to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
