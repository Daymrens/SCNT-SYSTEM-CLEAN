using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PerfumeInventory
{
    public class InventoryForm : Form
    {
        private DataGridView dgvPerfumes;
        private TextBox txtSearch;
        private Button btnAdd, btnEdit, btnDelete, btnRefresh, btnManageCategories;
        private PictureBox picPerfume;
        private Panel pnlImageDisplay;

        public InventoryForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Inventory Management";
            this.Size = new Size(1200, 800);
            this.BackColor = Color.FromArgb(248, 249, 250);

            // Header
            Label lblTitle = new Label
            {
                Text = "🧴 INVENTORY",
                Location = new Point(30, 25),
                Size = new Size(400, 35),
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            // Search Bar
            Panel pnlSearch = new Panel
            {
                Location = new Point(30, 80),
                Size = new Size(1140, 60),
                BackColor = Color.White
            };

            txtSearch = new TextBox 
            { 
                Location = new Point(20, 15), 
                Width = 350,
                Height = 35,
                Font = new Font("Segoe UI", 11),
                PlaceholderText = "🔍 Search by name, brand, category..."
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            btnRefresh = CreateButton("↺ Refresh", new Point(390, 13), Color.FromArgb(108, 117, 125), 110);
            btnRefresh.Click += (s, e) => { txtSearch.Clear(); LoadData(); };

            btnAdd = CreateButton("➕ Add Product", new Point(860, 13), Color.FromArgb(25, 135, 84), 140);
            btnAdd.Click += BtnAdd_Click;

            btnManageCategories = CreateButton("📁 Categories", new Point(1010, 13), Color.FromArgb(111, 66, 193), 130);
            btnManageCategories.Click += BtnManageCategories_Click;

            pnlSearch.Controls.AddRange(new Control[] { txtSearch, btnRefresh, btnAdd, btnManageCategories });

            // Data Grid Panel (adjusted width to make room for image panel)
            Panel pnlGrid = new Panel
            {
                Location = new Point(30, 160),
                Size = new Size(800, 580),
                BackColor = Color.White
            };

            dgvPerfumes = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(760, 490),
                AutoGenerateColumns = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 10),
                GridColor = Color.FromArgb(240, 240, 240),
                ColumnHeadersHeight = 45,
                RowTemplate = { Height = 40 },
                EnableHeadersVisualStyles = false,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                AllowUserToResizeRows = false
            };

            dgvPerfumes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            dgvPerfumes.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(73, 80, 87);
            dgvPerfumes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvPerfumes.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            dgvPerfumes.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgvPerfumes.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 244, 253);
            dgvPerfumes.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dgvPerfumes.DefaultCellStyle.Padding = new Padding(10, 5, 5, 5);

            dgvPerfumes.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Product Name", Width = 180 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Brand", HeaderText = "Brand", Width = 120 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Category", HeaderText = "Category", Width = 100 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Size", HeaderText = "Size", Width = 80 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Gender", HeaderText = "Gender", Width = 80 },
                new DataGridViewTextBoxColumn { DataPropertyName = "SellingPrice", HeaderText = "Price", Width = 90, DefaultCellStyle = new DataGridViewCellStyle { Format = "₱#,##0.00" } },
                new DataGridViewTextBoxColumn { DataPropertyName = "StockLevel", HeaderText = "Stock", Width = 70 }
            });

            dgvPerfumes.CellDoubleClick += DgvPerfumes_CellDoubleClick;
            dgvPerfumes.SelectionChanged += DgvPerfumes_SelectionChanged;

            btnEdit = CreateButton("✏️ Edit", new Point(20, 525), Color.FromArgb(13, 110, 253), 110);
            btnEdit.Click += BtnEdit_Click;

            btnDelete = CreateButton("🗑️ Delete", new Point(140, 525), Color.FromArgb(220, 53, 69), 110);
            btnDelete.Click += BtnDelete_Click;

            Label lblHint = new Label
            {
                Text = "💡 Double-click a row to edit",
                Location = new Point(270, 530),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.FromArgb(108, 117, 125)
            };

            pnlGrid.Controls.AddRange(new Control[] { dgvPerfumes, btnEdit, btnDelete, lblHint });

            // Image Display Panel (right side)
            pnlImageDisplay = new Panel
            {
                Location = new Point(850, 160),
                Size = new Size(320, 580),
                BackColor = Color.White
            };

            Label lblImageTitle = new Label
            {
                Text = "Product Image",
                Location = new Point(20, 20),
                Size = new Size(280, 30),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                TextAlign = ContentAlignment.MiddleCenter
            };

            picPerfume = new PictureBox
            {
                Location = new Point(10, 70),
                Size = new Size(300, 480),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(248, 249, 250)
            };

            Label lblImageHint = new Label
            {
                Text = "Click a row to view image",
                Location = new Point(20, 555),
                Size = new Size(280, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.FromArgb(108, 117, 125),
                TextAlign = ContentAlignment.TopCenter
            };

            pnlImageDisplay.Controls.AddRange(new Control[] { lblImageTitle, picPerfume, lblImageHint });

            this.Controls.AddRange(new Control[] { lblTitle, pnlSearch, pnlGrid, pnlImageDisplay });
            
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
                btnManageCategories.Visible = false;
                
                // Add "View Testers" button for cashiers
                Button btnViewTesters = CreateButton("🧪 View Testers", new Point(20, 525), Color.FromArgb(102, 16, 242), 140);
                btnViewTesters.Click += BtnViewTesters_Click;
                
                // Find the grid panel and add the button
                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is Panel panel && panel.Controls.Contains(dgvPerfumes))
                    {
                        panel.Controls.Add(btnViewTesters);
                        break;
                    }
                }
                
                // Update hint text
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

        private void BtnViewTesters_Click(object? sender, EventArgs e)
        {
            // Find the parent MainForm and load TestersForm
            Control? parent = this.Parent;
            while (parent != null)
            {
                if (parent is Form parentForm && parentForm.GetType().Name == "MainForm")
                {
                    // Get the MainForm instance
                    var mainForm = parentForm as dynamic;
                    if (mainForm != null)
                    {
                        try
                        {
                            mainForm.LoadForm(new TestersForm());
                            return;
                        }
                        catch { }
                    }
                }
                parent = parent.Parent;
            }
            
            // Fallback: try to find MainForm through Application.OpenForms
            foreach (Form form in Application.OpenForms)
            {
                if (form is MainForm mainForm)
                {
                    mainForm.LoadForm(new TestersForm());
                    return;
                }
            }
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
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
            
            // Add hover effects
            Color hoverColor = ControlPaint.Dark(backColor, 0.1f);
            btn.MouseEnter += (s, e) => btn.BackColor = hoverColor;
            btn.MouseLeave += (s, e) => btn.BackColor = backColor;
            
            return btn;
        }

        private void LoadData()
        {
            string searchTerm = txtSearch.Text.ToLower();
            var perfumes = DataStore.Perfumes.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                perfumes = perfumes.Where(p =>
                    p.Name.ToLower().Contains(searchTerm) ||
                    p.Brand.ToLower().Contains(searchTerm) ||
                    p.Category.ToLower().Contains(searchTerm) ||
                    p.BatchNumber.ToLower().Contains(searchTerm)
                );
            }

            dgvPerfumes.DataSource = null;
            dgvPerfumes.DataSource = perfumes.ToList();
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            LoadData();
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            var dialog = new PerfumeEditDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (dgvPerfumes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a perfume to edit.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvPerfumes.SelectedRows[0].DataBoundItem is Perfume perfume)
            {
                var dialog = new PerfumeEditDialog(perfume);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void DgvPerfumes_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            // Prevent editing for cashiers
            if (DataStore.CurrentUser?.Role == "Cashier")
            {
                return;
            }
            
            if (e.RowIndex >= 0 && dgvPerfumes.Rows[e.RowIndex].DataBoundItem is Perfume perfume)
            {
                var dialog = new PerfumeEditDialog(perfume);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvPerfumes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a perfume to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this perfume?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes && dgvPerfumes.SelectedRows[0].DataBoundItem is Perfume perfume)
            {
                DataStore.Perfumes.RemoveAll(p => p.Id == perfume.Id);
                DataPersistence.SaveAllData();
                LoadData();
                MessageBox.Show("Perfume deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnManageCategories_Click(object? sender, EventArgs e)
        {
            var dialog = new CategoryManagementDialog();
            dialog.ShowDialog();
        }

        private void DgvPerfumes_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvPerfumes.SelectedRows.Count > 0 && dgvPerfumes.SelectedRows[0].DataBoundItem is Perfume perfume)
            {
                if (!string.IsNullOrEmpty(perfume.ImagePath) && File.Exists(perfume.ImagePath))
                {
                    try
                    {
                        // Dispose previous image to avoid file locks
                        if (picPerfume.Image != null)
                        {
                            var oldImage = picPerfume.Image;
                            picPerfume.Image = null;
                            oldImage.Dispose();
                        }
                        
                        // Load new image
                        using (var fs = new FileStream(perfume.ImagePath, FileMode.Open, FileAccess.Read))
                        {
                            picPerfume.Image = Image.FromStream(fs);
                        }
                    }
                    catch
                    {
                        picPerfume.Image = null;
                    }
                }
                else
                {
                    picPerfume.Image = null;
                }
            }
        }
    }
}
