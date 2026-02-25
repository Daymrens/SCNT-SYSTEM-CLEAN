using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PerfumeInventory
{
    public class CustomerEditDialog : Form
    {
        private TextBox txtName, txtPhone, txtEmail;
        private Label lblLoyaltyPoints, lblTotalPurchases;
        private DataGridView dgvPurchaseHistory;
        private Button btnSave, btnCancel;
        private Customer? _customer;

        public CustomerEditDialog(Customer? customer = null)
        {
            _customer = customer;
            InitializeComponent();
            if (_customer != null)
            {
                LoadCustomerData();
            }
        }

        private void InitializeComponent()
        {
            this.Text = _customer == null ? "Add Customer" : "Edit Customer";
            this.Size = new Size(700, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(248, 249, 250);

            // Title
            Label lblTitle = new Label
            {
                Text = _customer == null ? "➕ Add New Customer" : "✏️ Edit Customer",
                Location = new Point(30, 30),
                Size = new Size(640, 35),
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            // Main Panel
            Panel pnlMain = new Panel
            {
                Location = new Point(30, 85),
                Size = new Size(640, 570),
                BackColor = Color.White
            };

            int yPos = 20;
            AddFormField(pnlMain, "Name:", out txtName, ref yPos);
            AddFormField(pnlMain, "Phone:", out txtPhone, ref yPos);
            AddFormField(pnlMain, "Email:", out txtEmail, ref yPos);

            // Stats Panel
            Panel pnlStats = new Panel
            {
                Location = new Point(20, yPos),
                Size = new Size(600, 60),
                BackColor = Color.FromArgb(248, 249, 250)
            };

            Label lblPointsLabel = new Label 
            { 
                Text = "Loyalty Points:", 
                Location = new Point(20, 20), 
                Width = 120, 
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(108, 117, 125)
            };
            lblLoyaltyPoints = new Label 
            { 
                Text = "0", 
                Location = new Point(140, 18), 
                Width = 100, 
                Font = new Font("Segoe UI", 14, FontStyle.Bold), 
                ForeColor = Color.FromArgb(255, 193, 7)
            };

            Label lblPurchasesLabel = new Label 
            { 
                Text = "Total Purchases:", 
                Location = new Point(300, 20), 
                Width = 130, 
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(108, 117, 125)
            };
            lblTotalPurchases = new Label 
            { 
                Text = "₱0.00", 
                Location = new Point(430, 18), 
                Width = 150, 
                Font = new Font("Segoe UI", 14, FontStyle.Bold), 
                ForeColor = Color.FromArgb(25, 135, 84)
            };

            pnlStats.Controls.AddRange(new Control[] { lblPointsLabel, lblLoyaltyPoints, lblPurchasesLabel, lblTotalPurchases });
            pnlMain.Controls.Add(pnlStats);
            yPos += 80;

            if (_customer != null)
            {
                Label lblHistory = new Label 
                { 
                    Text = "Purchase History", 
                    Location = new Point(20, yPos), 
                    Width = 250, 
                    Font = new Font("Segoe UI", 13, FontStyle.Bold),
                    ForeColor = Color.FromArgb(33, 37, 41)
                };
                pnlMain.Controls.Add(lblHistory);
                yPos += 40;

                dgvPurchaseHistory = new DataGridView
                {
                    Location = new Point(20, yPos),
                    Size = new Size(600, 200),
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

                dgvPurchaseHistory.Columns.AddRange(new DataGridViewColumn[]
                {
                    new DataGridViewTextBoxColumn { DataPropertyName = "SaleDate", HeaderText = "Date", Width = 200 },
                    new DataGridViewTextBoxColumn { DataPropertyName = "Total", HeaderText = "Amount", Width = 200 },
                    new DataGridViewTextBoxColumn { DataPropertyName = "PaymentMethod", HeaderText = "Payment", Width = 180 }
                });

                pnlMain.Controls.Add(dgvPurchaseHistory);
                yPos += 210;
            }

            // Buttons
            btnSave = new Button
            {
                Text = "✓ Save",
                Location = new Point(20, yPos + 20),
                Size = new Size(120, 40),
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
                Location = new Point(160, yPos + 20),
                Size = new Size(120, 40),
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

            pnlMain.Controls.AddRange(new Control[] { btnSave, btnCancel });

            this.Controls.AddRange(new Control[] { lblTitle, pnlMain });
        }

        private void AddFormField(Panel panel, string label, out TextBox textBox, ref int yPos)
        {
            Label lbl = new Label 
            { 
                Text = label, 
                Location = new Point(20, yPos), 
                Width = 100, 
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(108, 117, 125)
            };
            textBox = new TextBox 
            { 
                Location = new Point(20, yPos + 25), 
                Width = 600, 
                Height = 35,
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle
            };
            panel.Controls.AddRange(new Control[] { lbl, textBox });
            yPos += 75;
        }

        private void LoadCustomerData()
        {
            if (_customer != null)
            {
                txtName.Text = _customer.Name;
                txtPhone.Text = _customer.Phone;
                txtEmail.Text = _customer.Email;
                lblLoyaltyPoints.Text = _customer.LoyaltyPoints.ToString();

                var history = DataStore.Sales.Where(s => s.CustomerId == _customer.Id).ToList();
                decimal totalPurchases = history.Sum(s => s.Total);
                lblTotalPurchases.Text = "₱" + totalPurchases.ToString("N2");

                if (dgvPurchaseHistory != null)
                {
                    dgvPurchaseHistory.DataSource = history;
                }
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter customer name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_customer == null)
            {
                _customer = new Customer
                {
                    Id = DataStore.GetNextId(DataStore.Customers),
                    LoyaltyPoints = 0
                };
                DataStore.Customers.Add(_customer);
            }

            _customer.Name = txtName.Text;
            _customer.Phone = txtPhone.Text;
            _customer.Email = txtEmail.Text;

            DataPersistence.SaveAllData();
            this.DialogResult = DialogResult.OK;
        }
    }
}
