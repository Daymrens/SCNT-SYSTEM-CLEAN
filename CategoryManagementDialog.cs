using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PerfumeInventory
{
    public class CategoryManagementDialog : Form
    {
        private ListBox lstCategories;
        private TextBox txtNewCategory;
        private Button btnAdd, btnRemove, btnClose;
        private static List<string> _categories = new List<string>
        {
            "Gourmand", "Floral", "Fresh", "Woody", "Spicy", "Citrus", "Aquatic",
            "Fruity", "Sweet", "Musk", "Amber", "Powdery", "Green", "Aromatic",
            "Berry", "Honey", "Tropical", "Clean", "Aldehydic", "Salty"
        };

        public static List<string> Categories => _categories;

        public CategoryManagementDialog()
        {
            InitializeComponent();
            LoadCategories();
        }

        private void InitializeComponent()
        {
            this.Text = "Category Management";
            this.Size = new Size(500, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(240, 240, 245);

            Label lblTitle = new Label
            {
                Text = "Manage Perfume Categories (Scents)",
                Location = new Point(20, 20),
                Size = new Size(450, 30),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(63, 81, 181)
            };

            Label lblInfo = new Label
            {
                Text = "Add, remove, or manage perfume scent categories. These categories can be selected when adding perfumes.",
                Location = new Point(20, 60),
                Size = new Size(450, 40),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray
            };

            Label lblCategories = new Label
            {
                Text = "Existing Categories:",
                Location = new Point(20, 110),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            lstCategories = new ListBox
            {
                Location = new Point(20, 140),
                Size = new Size(450, 300),
                Font = new Font("Segoe UI", 10),
                SelectionMode = SelectionMode.One
            };

            Label lblAdd = new Label
            {
                Text = "Add New Category:",
                Location = new Point(20, 455),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            txtNewCategory = new TextBox
            {
                Location = new Point(20, 485),
                Size = new Size(350, 25),
                Font = new Font("Segoe UI", 10)
            };

            btnAdd = new Button
            {
                Text = "Add",
                Location = new Point(380, 483),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnAdd.Click += BtnAdd_Click;

            btnRemove = new Button
            {
                Text = "Remove Selected",
                Location = new Point(20, 525),
                Size = new Size(150, 30),
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnRemove.Click += BtnRemove_Click;

            btnClose = new Button
            {
                Text = "Close",
                Location = new Point(380, 525),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(158, 158, 158),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnClose.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { 
                lblTitle, lblInfo, lblCategories, lstCategories, 
                lblAdd, txtNewCategory, btnAdd, btnRemove, btnClose 
            });
        }

        private void LoadCategories()
        {
            lstCategories.Items.Clear();
            foreach (var category in _categories.OrderBy(c => c))
            {
                lstCategories.Items.Add(category);
            }
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            string newCategory = txtNewCategory.Text.Trim();
            
            if (string.IsNullOrWhiteSpace(newCategory))
            {
                MessageBox.Show("Please enter a category name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_categories.Any(c => c.Equals(newCategory, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("This category already exists.", "Duplicate Category", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _categories.Add(newCategory);
            LoadCategories();
            txtNewCategory.Clear();
            MessageBox.Show("Category added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnRemove_Click(object? sender, EventArgs e)
        {
            if (lstCategories.SelectedItem == null)
            {
                MessageBox.Show("Please select a category to remove.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Are you sure you want to remove this category?\nNote: This won't affect existing perfumes.",
                "Confirm Remove",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                string category = lstCategories.SelectedItem.ToString() ?? "";
                _categories.Remove(category);
                LoadCategories();
                MessageBox.Show("Category removed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
