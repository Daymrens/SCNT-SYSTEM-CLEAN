using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PerfumeInventory
{
    public class SalesHistoryForm : Form
    {
        private DataGridView dgvSales, dgvSaleItems;
        private DateTimePicker dtpFrom, dtpTo;
        private Button btnFilter, btnViewDetails, btnExport, btnPrint, btnEdit, btnDelete;
        private ComboBox cmbFilterType;
        private Label lblTotalSales, lblTotalRevenue, lblTotalProfit, lblAvgSale;

        public SalesHistoryForm()
        {
            InitializeComponent();
            LoadData();
            
            // Reload data when form is activated (becomes visible)
            this.Activated += (s, e) => LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Sales History";
            this.Size = new Size(1150, 800);
            this.BackColor = Color.FromArgb(248, 249, 250);

            Label lblTitle = new Label
            {
                Text = "📈 SALES HISTORY",
                Location = new Point(30, 30),
                Size = new Size(400, 35),
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            // Filter Panel
            Panel pnlFilter = new Panel
            {
                Location = new Point(30, 85),
                Size = new Size(1090, 90),
                BackColor = Color.White
            };

            Label lblFrom = new Label { Text = "From:", Location = new Point(20, 20), Width = 50, Font = new Font("Segoe UI", 10) };
            dtpFrom = new DateTimePicker { Location = new Point(75, 18), Width = 150, Font = new Font("Segoe UI", 10) };
            dtpFrom.Value = new DateTime(2025, 6, 1);

            Label lblTo = new Label { Text = "To:", Location = new Point(245, 20), Width = 30, Font = new Font("Segoe UI", 10) };
            dtpTo = new DateTimePicker { Location = new Point(280, 18), Width = 150, Font = new Font("Segoe UI", 10) };
            dtpTo.Value = new DateTime(2026, 2, 28);

            Label lblFilterType = new Label { Text = "Type:", Location = new Point(450, 20), Width = 40, Font = new Font("Segoe UI", 10) };
            cmbFilterType = new ComboBox { Location = new Point(495, 18), Width = 120, Font = new Font("Segoe UI", 10), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbFilterType.Items.AddRange(new object[] { "All", "Customer", "Reseller", "Walk-in" });
            cmbFilterType.SelectedIndex = 0;

            btnFilter = new Button
            {
                Text = "Filter",
                Location = new Point(635, 15),
                Size = new Size(90, 35),
                BackColor = Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnFilter.FlatAppearance.BorderSize = 0;
            btnFilter.Click += BtnFilter_Click;

            btnExport = new Button
            {
                Text = "Export CSV",
                Location = new Point(745, 15),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(25, 135, 84),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Click += BtnExport_Click;

            btnPrint = new Button
            {
                Text = "Print",
                Location = new Point(885, 15),
                Size = new Size(90, 35),
                BackColor = Color.FromArgb(102, 16, 242),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.Click += BtnPrint_Click;

            lblTotalSales = new Label { Text = "Total Bottles: 0", Location = new Point(20, 60), Width = 180, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(33, 37, 41) };
            lblTotalRevenue = new Label { Text = "Revenue: ₱0.00", Location = new Point(210, 60), Width = 200, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(33, 37, 41) };
            lblTotalProfit = new Label { Text = "Profit: ₱0.00", Location = new Point(420, 60), Width = 200, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(25, 135, 84) };
            lblAvgSale = new Label { Text = "Avg Sale: ₱0.00", Location = new Point(630, 60), Width = 200, Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(13, 110, 253) };

            pnlFilter.Controls.AddRange(new Control[] { lblFrom, dtpFrom, lblTo, dtpTo, lblFilterType, cmbFilterType, btnFilter, btnExport, btnPrint, lblTotalSales, lblTotalRevenue, lblTotalProfit, lblAvgSale });

            // Sales Grid
            Panel pnlSales = new Panel
            {
                Location = new Point(30, 195),
                Size = new Size(1090, 300),
                BackColor = Color.White
            };

            Label lblSalesTitle = new Label
            {
                Text = "Sales Transactions",
                Location = new Point(20, 20),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            dgvSales = new DataGridView
            {
                Location = new Point(20, 55),
                Size = new Size(1050, 190),
                AutoGenerateColumns = true,
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

            dgvSales.SelectionChanged += DgvSales_SelectionChanged;

            btnViewDetails = new Button
            {
                Text = "View Details",
                Location = new Point(20, 255),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnViewDetails.FlatAppearance.BorderSize = 0;
            btnViewDetails.Click += BtnViewDetails_Click;

            // Admin-only buttons
            btnEdit = new Button
            {
                Text = "✏️ Edit",
                Location = new Point(150, 255),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.FromArgb(33, 37, 41),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = DataStore.CurrentUser?.Role == "Admin"
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += BtnEdit_Click;

            btnDelete = new Button
            {
                Text = "🗑️ Delete",
                Location = new Point(260, 255),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = DataStore.CurrentUser?.Role == "Admin"
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += BtnDelete_Click;

            pnlSales.Controls.AddRange(new Control[] { lblSalesTitle, dgvSales, btnViewDetails, btnEdit, btnDelete });

            // Sale Items Grid
            Panel pnlItems = new Panel
            {
                Location = new Point(30, 515),
                Size = new Size(1090, 225),
                BackColor = Color.White
            };

            Label lblItemsTitle = new Label
            {
                Text = "Sale Items",
                Location = new Point(20, 20),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            dgvSaleItems = new DataGridView
            {
                Location = new Point(20, 55),
                Size = new Size(1050, 150),
                AutoGenerateColumns = true,
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

            pnlItems.Controls.AddRange(new Control[] { lblItemsTitle, dgvSaleItems });

            this.Controls.AddRange(new Control[] { lblTitle, pnlFilter, pnlSales, pnlItems });
        }


        private void LoadData()
        {
            var salesQuery = DataStore.Sales
                .Where(s => s.SaleDate.Date >= dtpFrom.Value.Date && s.SaleDate.Date <= dtpTo.Value.Date);

            // Apply type filter
            if (cmbFilterType.SelectedIndex > 0)
            {
                string filterType = cmbFilterType.SelectedItem.ToString() ?? "All";
                if (filterType == "Customer")
                    salesQuery = salesQuery.Where(s => s.CustomerId.HasValue);
                else if (filterType == "Reseller")
                    salesQuery = salesQuery.Where(s => s.ResellerId.HasValue);
                else if (filterType == "Walk-in")
                    salesQuery = salesQuery.Where(s => !s.CustomerId.HasValue && !s.ResellerId.HasValue);
            }

            var salesData = salesQuery
                .Select(s => new
                {
                    s.Id,
                    s.SaleDate,
                    Buyer = s.ResellerId.HasValue
                        ? "RESELLER: " + (DataStore.Resellers.FirstOrDefault(r => r.Id == s.ResellerId)?.Name ?? "Unknown")
                        : (s.CustomerId.HasValue 
                            ? DataStore.Customers.FirstOrDefault(c => c.Id == s.CustomerId)?.Name ?? "Walk-in"
                            : "Walk-in"),
                    Type = s.ResellerId.HasValue ? "Reseller" : (s.CustomerId.HasValue ? "Customer" : "Walk-in"),
                    s.Subtotal,
                    s.Discount,
                    s.Total,
                    s.PaymentMethod,
                    CashierName = DataStore.Users.FirstOrDefault(u => u.Id == s.UserId)?.FullName ?? "Unknown"
                })
                .OrderByDescending(s => s.SaleDate)
                .ToList();

            dgvSales.DataSource = salesData;

            // Format the grid
            if (dgvSales.Columns.Count > 0)
            {
                dgvSales.Columns["Id"].Width = 50;
                dgvSales.Columns["SaleDate"].Width = 150;
                dgvSales.Columns["Buyer"].Width = 200;
                dgvSales.Columns["Type"].Width = 80;
                dgvSales.Columns["Subtotal"].DefaultCellStyle.Format = "₱#,##0.00";
                dgvSales.Columns["Discount"].DefaultCellStyle.Format = "₱#,##0.00";
                dgvSales.Columns["Total"].DefaultCellStyle.Format = "₱#,##0.00";
            }

            // Calculate total bottles sold (sum of all quantities)
            var filteredSales = salesQuery.ToList();
            int totalBottles = 0;
            decimal totalRevenue = 0;
            decimal totalProfit = 0;

            foreach (var sale in filteredSales)
            {
                totalRevenue += sale.Total;
                
                var items = DataStore.SaleItems.Where(si => si.SaleId == sale.Id);
                foreach (var item in items)
                {
                    totalBottles += item.Quantity;
                    
                    var perfume = DataStore.Perfumes.FirstOrDefault(p => p.Id == item.PerfumeId);
                    if (perfume != null)
                    {
                        totalProfit += (item.UnitPrice - perfume.CostPrice) * item.Quantity;
                    }
                }
            }

            lblTotalSales.Text = $"Total Bottles: {totalBottles}";
            lblTotalRevenue.Text = $"Revenue: ₱{totalRevenue:N2}";
            lblTotalProfit.Text = $"Profit: ₱{totalProfit:N2}";
            lblAvgSale.Text = filteredSales.Count > 0 ? $"Avg Sale: ₱{(totalRevenue / filteredSales.Count):N2}" : "Avg Sale: ₱0.00";
        }

        private void BtnFilter_Click(object? sender, EventArgs e)
        {
            LoadData();
        }

        private void DgvSales_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvSales.SelectedRows.Count > 0 && dgvSales.SelectedRows[0].Cells["Id"].Value != null)
            {
                int saleId = Convert.ToInt32(dgvSales.SelectedRows[0].Cells["Id"].Value);
                var items = DataStore.SaleItems.Where(si => si.SaleId == saleId)
                    .Select(si => new
                    {
                        si.PerfumeName,
                        si.Quantity,
                        UnitPrice = "₱" + si.UnitPrice.ToString("N2"),
                        Subtotal = "₱" + si.Subtotal.ToString("N2")
                    })
                    .ToList();
                dgvSaleItems.DataSource = items;
            }
        }

        private void BtnViewDetails_Click(object? sender, EventArgs e)
        {
            if (dgvSales.SelectedRows.Count > 0)
            {
                int saleId = (int)dgvSales.SelectedRows[0].Cells["Id"].Value;
                var sale = DataStore.Sales.FirstOrDefault(s => s.Id == saleId);
                if (sale != null)
                {
                    var items = DataStore.SaleItems.Where(si => si.SaleId == saleId).ToList();
                    string itemsList = string.Join("\n", items.Select(i => $"- {i.PerfumeName} x{i.Quantity} = ₱{i.Subtotal:N2}"));
                    
                    string buyerInfo = "";
                    if (sale.ResellerId.HasValue)
                    {
                        var reseller = DataStore.Resellers.FirstOrDefault(r => r.Id == sale.ResellerId);
                        buyerInfo = $"RESELLER: {reseller?.Name ?? "Unknown"}\nBusiness: {reseller?.BusinessName ?? "N/A"}";
                    }
                    else if (sale.CustomerId.HasValue)
                    {
                        var customer = DataStore.Customers.FirstOrDefault(c => c.Id == sale.CustomerId);
                        buyerInfo = $"Customer: {customer?.Name ?? "Walk-in"}";
                    }
                    else
                    {
                        buyerInfo = "Customer: Walk-in";
                    }
                    
                    MessageBox.Show(
                        $"Receipt ID: {sale.Id}\n" +
                        $"Date: {sale.SaleDate}\n" +
                        $"{buyerInfo}\n\n" +
                        $"Items:\n{itemsList}\n\n" +
                        $"Subtotal: ₱{sale.Subtotal:N2}\n" +
                        $"Discount: -₱{sale.Discount:N2}\n" +
                        $"Total: ₱{sale.Total:N2}\n" +
                        $"Payment: {sale.PaymentMethod}",
                        "Sale Details",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
        }
        
        private void BtnExport_Click(object? sender, EventArgs e)
        {
            try
            {
                var saveDialog = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv",
                    FileName = $"Sales_Report_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var writer = new System.IO.StreamWriter(saveDialog.FileName))
                    {
                        // Write header
                        writer.WriteLine("Date,Buyer,Type,Subtotal,Discount,Total,Payment Method,Cashier");

                        // Write data
                        foreach (DataGridViewRow row in dgvSales.Rows)
                        {
                            if (row.IsNewRow) continue;
                            
                            var values = new System.Collections.Generic.List<string>();
                            for (int i = 1; i < row.Cells.Count; i++) // Skip ID column
                            {
                                var value = row.Cells[i].Value?.ToString() ?? "";
                                values.Add($"\"{value}\"");
                            }
                            writer.WriteLine(string.Join(",", values));
                        }
                    }

                    MessageBox.Show("Sales report exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPrint_Click(object? sender, EventArgs e)
        {
            MessageBox.Show(
                $"Sales Report\n" +
                $"Period: {dtpFrom.Value:MM/dd/yyyy} - {dtpTo.Value:MM/dd/yyyy}\n\n" +
                $"{lblTotalSales.Text}\n" +
                $"{lblTotalRevenue.Text}\n" +
                $"{lblTotalProfit.Text}\n" +
                $"{lblAvgSale.Text}\n\n" +
                $"Total Transactions: {dgvSales.Rows.Count}",
                "Print Preview",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (dgvSales.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a sale to edit.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var rowData = dgvSales.SelectedRows[0].DataBoundItem;
            if (rowData == null) return;

            var idProperty = rowData.GetType().GetProperty("Id");
            if (idProperty == null) return;

            int saleId = (int)idProperty.GetValue(rowData)!;
            var sale = DataStore.Sales.FirstOrDefault(s => s.Id == saleId);

            if (sale == null)
            {
                MessageBox.Show("Sale not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Create edit dialog
            var editDialog = new Form
            {
                Text = "Edit Sale",
                Size = new Size(500, 400),
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(248, 249, 250),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblTitle = new Label
            {
                Text = $"Edit Sale #{sale.Id}",
                Location = new Point(20, 20),
                Size = new Size(450, 30),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            Label lblSubtotal = new Label { Text = "Subtotal:", Location = new Point(20, 70), Width = 100, Font = new Font("Segoe UI", 10) };
            TextBox txtSubtotal = new TextBox { Location = new Point(130, 68), Width = 150, Font = new Font("Segoe UI", 10), Text = sale.Subtotal.ToString("F2") };

            Label lblDiscount = new Label { Text = "Discount:", Location = new Point(20, 110), Width = 100, Font = new Font("Segoe UI", 10) };
            TextBox txtDiscount = new TextBox { Location = new Point(130, 108), Width = 150, Font = new Font("Segoe UI", 10), Text = sale.Discount.ToString("F2") };

            Label lblTotal = new Label { Text = "Total:", Location = new Point(20, 150), Width = 100, Font = new Font("Segoe UI", 10) };
            TextBox txtTotal = new TextBox { Location = new Point(130, 148), Width = 150, Font = new Font("Segoe UI", 10), Text = sale.Total.ToString("F2") };

            Label lblPayment = new Label { Text = "Payment:", Location = new Point(20, 190), Width = 100, Font = new Font("Segoe UI", 10) };
            ComboBox cmbPayment = new ComboBox { Location = new Point(130, 188), Width = 150, Font = new Font("Segoe UI", 10), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbPayment.Items.AddRange(new object[] { "Cash", "Credit Card", "Debit Card", "E-Wallet" });
            cmbPayment.SelectedItem = sale.PaymentMethod;

            Label lblDate = new Label { Text = "Date:", Location = new Point(20, 230), Width = 100, Font = new Font("Segoe UI", 10) };
            DateTimePicker dtpDate = new DateTimePicker { Location = new Point(130, 228), Width = 150, Font = new Font("Segoe UI", 10), Value = sale.SaleDate };

            Button btnSave = new Button
            {
                Text = "Save Changes",
                Location = new Point(130, 280),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(25, 135, 84),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, ev) =>
            {
                if (decimal.TryParse(txtSubtotal.Text, out decimal subtotal) &&
                    decimal.TryParse(txtDiscount.Text, out decimal discount) &&
                    decimal.TryParse(txtTotal.Text, out decimal total))
                {
                    sale.Subtotal = subtotal;
                    sale.Discount = discount;
                    sale.Total = total;
                    sale.PaymentMethod = cmbPayment.Text;
                    sale.SaleDate = dtpDate.Value;

                    DataPersistence.SaveAllData();
                    LoadData();
                    editDialog.Close();
                    MessageBox.Show("Sale updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Please enter valid numbers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(290, 280),
                Size = new Size(100, 40),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, ev) => editDialog.Close();

            editDialog.Controls.AddRange(new Control[] { lblTitle, lblSubtotal, txtSubtotal, lblDiscount, txtDiscount, lblTotal, txtTotal, lblPayment, cmbPayment, lblDate, dtpDate, btnSave, btnCancel });
            editDialog.ShowDialog();
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvSales.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a sale to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Are you sure you want to delete this sale?\n\nThis action cannot be undone and will also delete all associated sale items.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes) return;

            var rowData = dgvSales.SelectedRows[0].DataBoundItem;
            if (rowData == null) return;

            var idProperty = rowData.GetType().GetProperty("Id");
            if (idProperty == null) return;

            int saleId = (int)idProperty.GetValue(rowData)!;
            var sale = DataStore.Sales.FirstOrDefault(s => s.Id == saleId);

            if (sale == null)
            {
                MessageBox.Show("Sale not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Delete associated sale items
            var saleItems = DataStore.SaleItems.Where(si => si.SaleId == saleId).ToList();
            foreach (var item in saleItems)
            {
                DataStore.SaleItems.Remove(item);
            }

            // Delete the sale
            DataStore.Sales.Remove(sale);
            DataPersistence.SaveAllData();

            LoadData();
            MessageBox.Show("Sale deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
