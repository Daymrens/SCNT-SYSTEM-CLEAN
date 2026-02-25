using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PerfumeInventory
{
    public class ResellerEditDialog : Form
    {
        private TextBox txtName, txtBusinessName, txtPhone, txtEmail, txtAddress, txtDiscountRate;
        private CheckBox chkIsActive;
        private Label lblTotalPurchases;
        private DataGridView dgvPurchaseHistory;
        private Button btnSave, btnCancel;
        private Reseller? _reseller;

        public ResellerEditDialog(Reseller? reseller = null)
        {
            _reseller = reseller;
            InitializeComponent();
            if (_reseller != null)
            {
                LoadResellerData();
            }
        }

        private void InitializeComponent()
        {
            this.Text = _reseller == null ? "Add Reseller" : "Edit Reseller";
            this.Size = new Size(720, 750);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(248, 249, 250);

            // Title Label (outside scrollable area)
            Label lblTitle = new Label
            {
                Text = _reseller == null ? "➕ ADD NEW RESELLER" : "✏️ EDIT RESELLER",
                Location = new Point(30, 20),
                Size = new Size(660, 35),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                BackColor = Color.White
            };

            // Scrollable Panel for form content
            Panel pnlScrollable = new Panel
            {
                Location = new Point(30, 65),
                Size = new Size(660, 570),
                BackColor = Color.White,
                AutoScroll = true
            };

            // Content Panel inside scrollable panel
            Panel pnlContent = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(620, 1000), // Large enough to hold all content
                BackColor = Color.White,
                AutoSize = true
            };

            int yPos = 20;
            AddFormFieldToPanel(pnlContent, "Contact Name:", out txtName, ref yPos);
            AddFormFieldToPanel(pnlContent, "Business Name:", out txtBusinessName, ref yPos);
            AddFormFieldToPanel(pnlContent, "Phone:", out txtPhone, ref yPos);
            AddFormFieldToPanel(pnlContent, "Email:", out txtEmail, ref yPos);
            AddFormFieldToPanel(pnlContent, "Address:", out txtAddress, ref yPos);
            AddFormFieldToPanel(pnlContent, "Discount Rate (%):", out txtDiscountRate, ref yPos);
            txtDiscountRate.Text = "0";

            chkIsActive = new CheckBox 
            { 
                Location = new Point(20, yPos), 
                Checked = true, 
                Font = new Font("Segoe UI", 11), 
                Text = "✓ Active Reseller",
                ForeColor = Color.FromArgb(33, 37, 41)
            };
            pnlContent.Controls.Add(chkIsActive);
            yPos += 50;

            // Stats Panel
            if (_reseller != null)
            {
                Panel pnlStats = new Panel
                {
                    Location = new Point(20, yPos),
                    Size = new Size(580, 60),
                    BackColor = Color.FromArgb(248, 249, 250)
                };

                Label lblStatsTitle = new Label
                {
                    Text = "Total Purchases",
                    Location = new Point(15, 10),
                    Size = new Size(200, 20),
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.FromArgb(108, 117, 125)
                };

                lblTotalPurchases = new Label
                {
                    Text = "₱0.00",
                    Location = new Point(15, 30),
                    Size = new Size(200, 25),
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = Color.FromArgb(25, 135, 84)
                };

                pnlStats.Controls.AddRange(new Control[] { lblStatsTitle, lblTotalPurchases });
                pnlContent.Controls.Add(pnlStats);
                yPos += 75;

                // Purchase History
                Label lblHistory = new Label 
                { 
                    Text = "Purchase History", 
                    Location = new Point(20, yPos), 
                    Width = 200, 
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.FromArgb(33, 37, 41)
                };
                pnlContent.Controls.Add(lblHistory);
                yPos += 35;

                dgvPurchaseHistory = new DataGridView
                {
                    Location = new Point(20, yPos),
                    Size = new Size(580, 180),
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
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        Padding = new Padding(10),
                        SelectionBackColor = Color.FromArgb(248, 249, 250),
                        SelectionForeColor = Color.FromArgb(33, 37, 41)
                    },
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
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
                    new DataGridViewTextBoxColumn { DataPropertyName = "SaleDate", HeaderText = "Date", Width = 180 },
                    new DataGridViewTextBoxColumn { DataPropertyName = "Total", HeaderText = "Amount", Width = 180 },
                    new DataGridViewTextBoxColumn { DataPropertyName = "PaymentMethod", HeaderText = "Payment", Width = 180 }
                });

                pnlContent.Controls.Add(dgvPurchaseHistory);
                yPos += 200;
            }

            // Set content panel height based on content
            pnlContent.Height = yPos + 20;

            pnlScrollable.Controls.Add(pnlContent);

            // Buttons Panel (outside scrollable area, fixed at bottom)
            Panel pnlButtons = new Panel
            {
                Location = new Point(30, 645),
                Size = new Size(660, 60),
                BackColor = Color.White
            };

            btnSave = new Button
            {
                Text = _reseller == null ? "✓ Save" : "✓ Update",
                Location = new Point(440, 10),
                Size = new Size(100, 40),
                BackColor = Color.FromArgb(25, 135, 84),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            btnSave.MouseEnter += (s, e) => btnSave.BackColor = ControlPaint.Light(Color.FromArgb(25, 135, 84), 0.2f);
            btnSave.MouseLeave += (s, e) => btnSave.BackColor = Color.FromArgb(25, 135, 84);

            btnCancel = new Button
            {
                Text = "✕ Cancel",
                Location = new Point(550, 10),
                Size = new Size(100, 40),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            btnCancel.MouseEnter += (s, e) => btnCancel.BackColor = ControlPaint.Light(Color.FromArgb(108, 117, 125), 0.2f);
            btnCancel.MouseLeave += (s, e) => btnCancel.BackColor = Color.FromArgb(108, 117, 125);

            pnlButtons.Controls.AddRange(new Control[] { btnSave, btnCancel });

            this.Controls.AddRange(new Control[] { lblTitle, pnlScrollable, pnlButtons });
        }

        private void AddFormField(string label, out TextBox textBox, ref int yPos)
        {
            Label lbl = new Label 
            { 
                Text = label, 
                Location = new Point(20, yPos), 
                Width = 200, 
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(33, 37, 41)
            };
            textBox = new TextBox 
            { 
                Location = new Point(20, yPos + 25), 
                Width = 600, 
                Height = 30,
                Font = new Font("Segoe UI", 11) 
            };
            
            // Add to parent panel instead of form
            var parent = this.Controls.Count > 0 && this.Controls[0] is Panel ? (Panel)this.Controls[0] : null;
            if (parent != null)
            {
                parent.Controls.AddRange(new Control[] { lbl, textBox });
            }
            else
            {
                this.Controls.AddRange(new Control[] { lbl, textBox });
            }
            
            yPos += 75;
        }

        private void AddFormFieldToPanel(Panel panel, string label, out TextBox textBox, ref int yPos)
        {
            Label lbl = new Label 
            { 
                Text = label, 
                Location = new Point(20, yPos), 
                Width = 200, 
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(33, 37, 41)
            };
            textBox = new TextBox 
            { 
                Location = new Point(20, yPos + 25), 
                Width = 580, 
                Height = 30,
                Font = new Font("Segoe UI", 11) 
            };
            
            panel.Controls.AddRange(new Control[] { lbl, textBox });
            
            yPos += 75;
        }

        private void LoadResellerData()
        {
            if (_reseller != null)
            {
                txtName.Text = _reseller.Name;
                txtBusinessName.Text = _reseller.BusinessName;
                txtPhone.Text = _reseller.Phone;
                txtEmail.Text = _reseller.Email;
                txtAddress.Text = _reseller.Address;
                txtDiscountRate.Text = _reseller.DiscountRate.ToString();
                chkIsActive.Checked = _reseller.IsActive;

                var history = DataStore.Sales.Where(s => s.ResellerId == _reseller.Id).ToList();
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
                MessageBox.Show("Please enter contact name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtBusinessName.Text))
            {
                MessageBox.Show("Please enter business name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtDiscountRate.Text, out decimal discount) || discount < 0 || discount > 100)
            {
                MessageBox.Show("Please enter a valid discount rate (0-100).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_reseller == null)
            {
                _reseller = new Reseller
                {
                    Id = DataStore.GetNextId(DataStore.Resellers),
                    TotalPurchases = 0
                };
                DataStore.Resellers.Add(_reseller);
            }

            _reseller.Name = txtName.Text;
            _reseller.BusinessName = txtBusinessName.Text;
            _reseller.Phone = txtPhone.Text;
            _reseller.Email = txtEmail.Text;
            _reseller.Address = txtAddress.Text;
            _reseller.DiscountRate = discount;
            _reseller.IsActive = chkIsActive.Checked;

            DataPersistence.SaveAllData();
            this.DialogResult = DialogResult.OK;
        }
    }
}
