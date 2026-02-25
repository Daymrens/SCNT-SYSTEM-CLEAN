using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PerfumeInventory
{
    public class PerfumeEditDialog : Form
    {
        private Perfume? perfume;
        private bool isEditMode;
        
        private TextBox txtName, txtBrand, txtBatchNumber, txtCostPrice, txtSellingPrice, txtResellerPrice, txtCost60ml, txtPrice60ml, txtStockLevel, txtLowStockThreshold;
        private ComboBox cmbGender, cmbSupplier;
        private CheckedListBox clbCategories;
        private DateTimePicker dtpExpiration;
        private PictureBox picPerfume;
        private Button btnSave, btnCancel, btnBrowseImage, btnRemoveImage;
        private string selectedImagePath = string.Empty;

        public PerfumeEditDialog(Perfume? existingPerfume = null)
        {
            perfume = existingPerfume;
            isEditMode = existingPerfume != null;
            InitializeComponent();
            if (isEditMode && perfume != null)
            {
                LoadPerfumeData();
            }
        }

        private void InitializeComponent()
        {
            this.Text = isEditMode ? "Edit Perfume" : "Add New Perfume";
            this.Size = new Size(900, 900);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(248, 249, 250);

            // Title
            Label lblTitle = new Label
            {
                Text = isEditMode ? "✏️ Edit Perfume" : "➕ Add New Perfume",
                Location = new Point(30, 30),
                Size = new Size(840, 35),
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            // Main Panel
            Panel pnlMain = new Panel
            {
                Location = new Point(30, 85),
                Size = new Size(840, 700),
                BackColor = Color.White
            };

            // Left Panel - Image
            Panel pnlImage = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(280, 560),
                BackColor = Color.FromArgb(248, 249, 250)
            };

            Label lblImage = new Label
            {
                Text = "Product Image",
                Location = new Point(10, 10),
                Size = new Size(260, 25),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                TextAlign = ContentAlignment.MiddleCenter
            };

            picPerfume = new PictureBox
            {
                Location = new Point(40, 50),
                Size = new Size(200, 200),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White
            };

            btnBrowseImage = new Button
            {
                Text = "📁 Browse Image",
                Location = new Point(40, 270),
                Size = new Size(200, 40),
                BackColor = Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnBrowseImage.FlatAppearance.BorderSize = 0;
            btnBrowseImage.Click += BtnBrowseImage_Click;
            btnBrowseImage.MouseEnter += (s, e) => btnBrowseImage.BackColor = Color.FromArgb(10, 88, 202);
            btnBrowseImage.MouseLeave += (s, e) => btnBrowseImage.BackColor = Color.FromArgb(13, 110, 253);

            btnRemoveImage = new Button
            {
                Text = "🗑️ Remove Image",
                Location = new Point(40, 320),
                Size = new Size(200, 35),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRemoveImage.FlatAppearance.BorderSize = 0;
            btnRemoveImage.Click += BtnRemoveImage_Click;
            btnRemoveImage.MouseEnter += (s, e) => btnRemoveImage.BackColor = Color.FromArgb(176, 42, 55);
            btnRemoveImage.MouseLeave += (s, e) => btnRemoveImage.BackColor = Color.FromArgb(220, 53, 69);

            pnlImage.Controls.AddRange(new Control[] { lblImage, picPerfume, btnBrowseImage, btnRemoveImage });

            // Right Panel - Form Fields
            Panel pnlForm = new Panel
            {
                Location = new Point(320, 20),
                Size = new Size(500, 660),
                BackColor = Color.White,
                AutoScroll = true
            };

            int yPos = 15;
            AddFormField(pnlForm, "Name:", out txtName, ref yPos);
            AddFormField(pnlForm, "Brand:", out txtBrand, ref yPos);
            
            // Categories (Multiple Selection)
            Label lblCategories = new Label 
            { 
                Text = "Categories (Scents):", 
                Location = new Point(15, yPos), 
                Width = 470, 
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(108, 117, 125)
            };
            clbCategories = new CheckedListBox
            {
                Location = new Point(15, yPos + 25),
                Width = 470,
                Height = 100,
                Font = new Font("Segoe UI", 10),
                CheckOnClick = true,
                BorderStyle = BorderStyle.FixedSingle
            };
            foreach (var category in CategoryManagementDialog.Categories.OrderBy(c => c))
            {
                clbCategories.Items.Add(category);
            }
            pnlForm.Controls.AddRange(new Control[] { lblCategories, clbCategories });
            yPos += 140;
            
            AddFormField(pnlForm, "Size:", out txtSize, ref yPos);
            AddComboField(pnlForm, "Gender:", out cmbGender, ref yPos, new[] { "Men", "Women", "Unisex" });
            AddFormField(pnlForm, "Batch Number:", out txtBatchNumber, ref yPos);
            
            Label lblExpiration = new Label 
            { 
                Text = "Expiration Date:", 
                Location = new Point(15, yPos), 
                Width = 470, 
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(108, 117, 125)
            };
            dtpExpiration = new DateTimePicker 
            { 
                Location = new Point(15, yPos + 25), 
                Width = 470, 
                Height = 35,
                Font = new Font("Segoe UI", 10) 
            };
            pnlForm.Controls.AddRange(new Control[] { lblExpiration, dtpExpiration });
            yPos += 75;

            Label lblSupplier = new Label 
            { 
                Text = "Supplier:", 
                Location = new Point(15, yPos), 
                Width = 470, 
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(108, 117, 125)
            };
            cmbSupplier = new ComboBox 
            { 
                Location = new Point(15, yPos + 25), 
                Width = 470, 
                Height = 35,
                Font = new Font("Segoe UI", 10), 
                DropDownStyle = ComboBoxStyle.DropDownList 
            };
            cmbSupplier.DataSource = DataStore.Suppliers.Select(s => new { s.Id, s.Name }).ToList();
            cmbSupplier.DisplayMember = "Name";
            cmbSupplier.ValueMember = "Id";
            pnlForm.Controls.AddRange(new Control[] { lblSupplier, cmbSupplier });
            yPos += 75;

            AddFormField(pnlForm, "Cost Price (₱):", out txtCostPrice, ref yPos);
            AddFormField(pnlForm, "Selling Price (₱):", out txtSellingPrice, ref yPos);
            AddFormField(pnlForm, "Reseller Price (₱):", out txtResellerPrice, ref yPos);
            AddFormField(pnlForm, "60ml Cost Price (₱):", out txtCost60ml, ref yPos);
            AddFormField(pnlForm, "60ml Selling Price (₱):", out txtPrice60ml, ref yPos);
            AddFormField(pnlForm, "Stock Level:", out txtStockLevel, ref yPos);
            AddFormField(pnlForm, "Low Stock Alert:", out txtLowStockThreshold, ref yPos);

            pnlMain.Controls.AddRange(new Control[] { pnlImage, pnlForm });

            // Buttons
            btnSave = new Button
            {
                Text = isEditMode ? "✓ Update" : "✓ Save",
                Location = new Point(30, 805),
                Size = new Size(130, 40),
                BackColor = Color.FromArgb(25, 135, 84),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            btnSave.MouseEnter += (s, e) => btnSave.BackColor = Color.FromArgb(20, 108, 67);
            btnSave.MouseLeave += (s, e) => btnSave.BackColor = Color.FromArgb(25, 135, 84);

            btnCancel = new Button
            {
                Text = "✕ Cancel",
                Location = new Point(180, 805),
                Size = new Size(130, 40),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            btnCancel.MouseEnter += (s, e) => btnCancel.BackColor = Color.FromArgb(90, 98, 104);
            btnCancel.MouseLeave += (s, e) => btnCancel.BackColor = Color.FromArgb(108, 117, 125);

            this.Controls.AddRange(new Control[] { lblTitle, pnlMain, btnSave, btnCancel });
        }

        private TextBox txtSize;

        private void AddFormField(Panel panel, string label, out TextBox textBox, ref int yPos)
        {
            Label lbl = new Label 
            { 
                Text = label, 
                Location = new Point(15, yPos), 
                Width = 470, 
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(108, 117, 125)
            };
            textBox = new TextBox 
            { 
                Location = new Point(15, yPos + 25), 
                Width = 470, 
                Height = 35,
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle
            };
            panel.Controls.AddRange(new Control[] { lbl, textBox });
            yPos += 75;
        }

        private void AddComboField(Panel panel, string label, out ComboBox comboBox, ref int yPos, string[] items)
        {
            Label lbl = new Label 
            { 
                Text = label, 
                Location = new Point(15, yPos), 
                Width = 470, 
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(108, 117, 125)
            };
            comboBox = new ComboBox 
            { 
                Location = new Point(15, yPos + 25), 
                Width = 470, 
                Height = 35,
                Font = new Font("Segoe UI", 10), 
                DropDownStyle = ComboBoxStyle.DropDownList 
            };
            comboBox.Items.AddRange(items);
            panel.Controls.AddRange(new Control[] { lbl, comboBox });
            yPos += 75;
        }

        private void LoadPerfumeData()
        {
            if (perfume == null) return;

            txtName.Text = perfume.Name;
            txtBrand.Text = perfume.Brand;
            
            // Load categories (split by comma or slash)
            var categories = perfume.Category.Split(new[] { ',', '/', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Trim())
                .ToList();
            for (int i = 0; i < clbCategories.Items.Count; i++)
            {
                string item = clbCategories.Items[i].ToString() ?? "";
                if (categories.Any(c => c.Equals(item, StringComparison.OrdinalIgnoreCase)))
                {
                    clbCategories.SetItemChecked(i, true);
                }
            }
            txtSize.Text = perfume.Size;
            cmbGender.Text = perfume.Gender;
            txtBatchNumber.Text = perfume.BatchNumber;
            dtpExpiration.Value = perfume.ExpirationDate;
            cmbSupplier.SelectedValue = perfume.SupplierId;
            txtCostPrice.Text = perfume.CostPrice.ToString();
            txtSellingPrice.Text = perfume.SellingPrice.ToString();
            txtResellerPrice.Text = perfume.ResellerPrice.ToString();
            txtCost60ml.Text = perfume.Cost60ml.ToString();
            txtPrice60ml.Text = perfume.Price60ml.ToString();
            txtStockLevel.Text = perfume.StockLevel.ToString();
            txtLowStockThreshold.Text = perfume.LowStockThreshold.ToString();
            
            if (!string.IsNullOrEmpty(perfume.ImagePath) && File.Exists(perfume.ImagePath))
            {
                try
                {
                    picPerfume.Image = Image.FromFile(perfume.ImagePath);
                    selectedImagePath = perfume.ImagePath;
                }
                catch { }
            }
        }

        private void BtnBrowseImage_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                ofd.Title = "Select Perfume Image";
                
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        picPerfume.Image = Image.FromFile(ofd.FileName);
                        selectedImagePath = ofd.FileName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnRemoveImage_Click(object? sender, EventArgs e)
        {
            picPerfume.Image = null;
            selectedImagePath = string.Empty;
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            if (isEditMode && perfume != null)
            {
                // Get selected categories
                var selectedCategories = clbCategories.CheckedItems.Cast<string>().ToList();
                if (selectedCategories.Count == 0)
                {
                    MessageBox.Show("Please select at least one category.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                perfume.Name = txtName.Text;
                perfume.Brand = txtBrand.Text;
                perfume.Category = string.Join("/", selectedCategories);
                perfume.Size = txtSize.Text;
                perfume.Gender = cmbGender.Text;
                perfume.BatchNumber = txtBatchNumber.Text;
                perfume.ExpirationDate = dtpExpiration.Value;
                perfume.SupplierId = (int)cmbSupplier.SelectedValue;
                perfume.CostPrice = decimal.Parse(txtCostPrice.Text);
                perfume.SellingPrice = decimal.Parse(txtSellingPrice.Text);
                perfume.ResellerPrice = decimal.Parse(txtResellerPrice.Text);
                perfume.Cost60ml = decimal.Parse(txtCost60ml.Text);
                perfume.Price60ml = decimal.Parse(txtPrice60ml.Text);
                perfume.StockLevel = int.Parse(txtStockLevel.Text);
                perfume.LowStockThreshold = int.Parse(txtLowStockThreshold.Text);
                perfume.ImagePath = selectedImagePath;

                DataPersistence.SaveAllData();
                MessageBox.Show("Perfume updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Get selected categories
                var selectedCategories = clbCategories.CheckedItems.Cast<string>().ToList();
                if (selectedCategories.Count == 0)
                {
                    MessageBox.Show("Please select at least one category.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var newPerfume = new Perfume
                {
                    Id = DataStore.GetNextId(DataStore.Perfumes),
                    Name = txtName.Text,
                    Brand = txtBrand.Text,
                    Category = string.Join("/", selectedCategories),
                    Size = txtSize.Text,
                    Gender = cmbGender.Text,
                    BatchNumber = txtBatchNumber.Text,
                    ExpirationDate = dtpExpiration.Value,
                    SupplierId = (int)cmbSupplier.SelectedValue,
                    CostPrice = decimal.Parse(txtCostPrice.Text),
                    SellingPrice = decimal.Parse(txtSellingPrice.Text),
                    ResellerPrice = decimal.Parse(txtResellerPrice.Text),
                    Cost60ml = decimal.Parse(txtCost60ml.Text),
                    Price60ml = decimal.Parse(txtPrice60ml.Text),
                    StockLevel = int.Parse(txtStockLevel.Text),
                    LowStockThreshold = int.Parse(txtLowStockThreshold.Text),
                    ImagePath = selectedImagePath
                };

                DataStore.Perfumes.Add(newPerfume);
                DataPersistence.SaveAllData();
                MessageBox.Show("Perfume added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.DialogResult = DialogResult.OK;
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!decimal.TryParse(txtCostPrice.Text, out _) || !decimal.TryParse(txtSellingPrice.Text, out _))
            {
                MessageBox.Show("Please enter valid prices.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!int.TryParse(txtStockLevel.Text, out _))
            {
                MessageBox.Show("Please enter a valid stock level.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (cmbSupplier.SelectedValue == null)
            {
                MessageBox.Show("Please select a supplier.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
    }
}
