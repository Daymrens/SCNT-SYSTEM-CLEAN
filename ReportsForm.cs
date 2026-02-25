using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PerfumeInventory
{
    public class ReportsForm : Form
    {
        private DataGridView dgvReport;
        private ComboBox cmbReportType;
        private DateTimePicker dtpFrom, dtpTo;
        private Button btnGenerate, btnExport;
        private Label lblSummary;
        private Panel pnlChart;
        private List<ChartData> chartData = new List<ChartData>();
        private string currentReportType = "";

        private class ChartData
        {
            public string Label { get; set; } = "";
            public decimal Value { get; set; }
            public int Quantity { get; set; }
            public Color Color { get; set; }
        }

        public ReportsForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Reports & Analytics";
            this.Size = new Size(1150, 800);
            this.BackColor = Color.FromArgb(248, 249, 250);

            Label lblTitle = new Label
            {
                Text = "📊 REPORTS",
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

            Label lblReportType = new Label { Text = "Report Type:", Location = new Point(20, 20), Width = 100, Font = new Font("Segoe UI", 10) };
            cmbReportType = new ComboBox { Location = new Point(125, 18), Width = 280, Height = 30, Font = new Font("Segoe UI", 10), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbReportType.Items.AddRange(new object[] {
                "Sales Summary",
                "Top 10 Best Sellers",
                "Slow Moving Inventory",
                "Low Stock Report",
                "Profit Analysis",
                "Customer Purchase History",
                "Supplier Performance",
                "Expiring Products",
                "Category Performance",
                "Monthly Trends",
                "Payment Methods Analysis"
            });
            cmbReportType.SelectedIndex = 0;

            Label lblFrom = new Label { Text = "From:", Location = new Point(20, 55), Width = 50, Font = new Font("Segoe UI", 10) };
            dtpFrom = new DateTimePicker { Location = new Point(75, 53), Width = 150, Font = new Font("Segoe UI", 10) };
            dtpFrom.Value = DateTime.Today.AddMonths(-1);

            Label lblTo = new Label { Text = "To:", Location = new Point(245, 55), Width = 30, Font = new Font("Segoe UI", 10) };
            dtpTo = new DateTimePicker { Location = new Point(280, 53), Width = 150, Font = new Font("Segoe UI", 10) };

            btnGenerate = new Button
            {
                Text = "Generate Report",
                Location = new Point(450, 50),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnGenerate.FlatAppearance.BorderSize = 0;
            btnGenerate.Click += BtnGenerate_Click;

            btnExport = new Button
            {
                Text = "Export to CSV",
                Location = new Point(620, 50),
                Size = new Size(140, 35),
                BackColor = Color.FromArgb(25, 135, 84),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnExport.FlatAppearance.BorderSize = 0;

            pnlFilter.Controls.AddRange(new Control[] { lblReportType, cmbReportType, lblFrom, dtpFrom, lblTo, dtpTo, btnGenerate, btnExport });

            // Summary Panel
            Panel pnlSummary = new Panel
            {
                Location = new Point(30, 195),
                Size = new Size(1090, 60),
                BackColor = Color.FromArgb(13, 110, 253)
            };

            lblSummary = new Label
            {
                Text = "Select a report type and click Generate Report",
                Location = new Point(20, 15),
                Size = new Size(1050, 30),
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft
            };

            pnlSummary.Controls.Add(lblSummary);

            // Chart Panel (Visual representation)
            pnlChart = new Panel
            {
                Location = new Point(30, 275),
                Size = new Size(1090, 160),
                BackColor = Color.White
            };

            Label lblChartTitle = new Label
            {
                Text = "📈 Visual Analytics",
                Location = new Point(20, 20),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            pnlChart.Controls.Add(lblChartTitle);
            pnlChart.Paint += PnlChart_Paint;

            // Report Grid
            Panel pnlReport = new Panel
            {
                Location = new Point(30, 455),
                Size = new Size(1090, 285),
                BackColor = Color.White
            };

            Label lblReportTitle = new Label
            {
                Text = "Report Data",
                Location = new Point(20, 20),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            dgvReport = new DataGridView
            {
                Location = new Point(20, 55),
                Size = new Size(1050, 210),
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
                ColumnHeadersHeight = 38,
                RowTemplate = { Height = 36 },
                EnableHeadersVisualStyles = false,
                CellBorderStyle = DataGridViewCellBorderStyle.None
            };

            pnlReport.Controls.AddRange(new Control[] { lblReportTitle, dgvReport });

            this.Controls.AddRange(new Control[] { lblTitle, pnlFilter, pnlSummary, pnlChart, pnlReport });
        }

        private void BtnGenerate_Click(object? sender, EventArgs e)
        {
            string reportType = cmbReportType.SelectedItem?.ToString() ?? "";

            switch (reportType)
            {
                case "Sales Summary":
                    GenerateSalesSummary();
                    break;
                case "Top 10 Best Sellers":
                    GenerateTopSellers();
                    break;
                case "Slow Moving Inventory":
                    GenerateSlowMoving();
                    break;
                case "Low Stock Report":
                    GenerateLowStock();
                    break;
                case "Profit Analysis":
                    GenerateProfitAnalysis();
                    break;
                case "Customer Purchase History":
                    GenerateCustomerHistory();
                    break;
                case "Supplier Performance":
                    GenerateSupplierPerformance();
                    break;
                case "Expiring Products":
                    GenerateExpiringProducts();
                    break;
                case "Category Performance":
                    GenerateCategoryPerformance();
                    break;
                case "Monthly Trends":
                    GenerateMonthlyTrends();
                    break;
                case "Payment Methods Analysis":
                    GeneratePaymentMethodsAnalysis();
                    break;
            }
        }

        private void GenerateSalesSummary()
        {
            var sales = DataStore.Sales.Where(s => s.SaleDate >= dtpFrom.Value && s.SaleDate <= dtpTo.Value).ToList();
            
            // Calculate total bottles sold and profit
            int totalBottles = 0;
            decimal totalProfit = 0;
            
            foreach (var sale in sales)
            {
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
            
            // Group by date and show bottles sold per scent
            var summary = sales.GroupBy(s => s.SaleDate.Date)
                .Select(g => new
                {
                    Date = g.Key.ToString("MM/dd/yyyy"),
                    TotalBottles = g.SelectMany(s => DataStore.SaleItems.Where(si => si.SaleId == s.Id))
                        .Sum(si => si.Quantity),
                    Revenue = "₱" + g.Sum(s => s.Total).ToString("N2"),
                    Profit = "₱" + g.SelectMany(s => DataStore.SaleItems.Where(si => si.SaleId == s.Id))
                        .Sum(si => {
                            var perfume = DataStore.Perfumes.FirstOrDefault(p => p.Id == si.PerfumeId);
                            return perfume != null ? (si.UnitPrice - perfume.CostPrice) * si.Quantity : 0;
                        }).ToString("N2"),
                    TopScent = g.SelectMany(s => DataStore.SaleItems.Where(si => si.SaleId == s.Id))
                        .GroupBy(si => si.PerfumeName)
                        .OrderByDescending(grp => grp.Sum(si => si.Quantity))
                        .Select(grp => grp.Key + " (" + grp.Sum(si => si.Quantity) + ")")
                        .FirstOrDefault() ?? "N/A"
                })
                .OrderByDescending(s => s.Date)
                .ToList();

            dgvReport.DataSource = summary;
            lblSummary.Text = $"Total Bottles Sold: {totalBottles} | Total Revenue: ₱{sales.Sum(s => s.Total):N2} | Profit: ₱{totalProfit:N2}";

            // Prepare chart data - show total bottles sold per day
            currentReportType = "Sales Summary";
            chartData.Clear();
            var dailySales = sales.GroupBy(s => s.SaleDate.Date).OrderBy(g => g.Key).Take(10).ToList();
            var colors = new[] { Color.FromArgb(33, 150, 243), Color.FromArgb(76, 175, 80), Color.FromArgb(255, 152, 0), 
                                Color.FromArgb(156, 39, 176), Color.FromArgb(244, 67, 54), Color.FromArgb(0, 188, 212),
                                Color.FromArgb(255, 193, 7), Color.FromArgb(103, 58, 183), Color.FromArgb(233, 30, 99), Color.FromArgb(63, 81, 181) };
            int colorIndex = 0;
            foreach (var day in dailySales)
            {
                // Calculate total bottles for this day
                int dayBottles = day.SelectMany(s => DataStore.SaleItems.Where(si => si.SaleId == s.Id))
                    .Sum(si => si.Quantity);
                    
                chartData.Add(new ChartData 
                { 
                    Label = day.Key.ToString("MM/dd"), 
                    Value = dayBottles,
                    Color = colors[colorIndex++ % colors.Length]
                });
            }
            pnlChart.Invalidate();
        }

        private void GenerateTopSellers()
        {
            var topSellers = DataStore.SaleItems
                .GroupBy(si => si.PerfumeId)
                .Select(g => new
                {
                    Product = DataStore.Perfumes.FirstOrDefault(p => p.Id == g.Key)?.Name ?? "Unknown",
                    Brand = DataStore.Perfumes.FirstOrDefault(p => p.Id == g.Key)?.Brand ?? "",
                    TotalSold = g.Sum(si => si.Quantity),
                    Revenue = "₱" + g.Sum(si => si.Subtotal).ToString("N2")
                })
                .OrderByDescending(p => p.TotalSold)
                .Take(10)
                .ToList();

            dgvReport.DataSource = topSellers;
            lblSummary.Text = $"Top 10 Best Selling Products";

            // Prepare chart data
            currentReportType = "Top Sellers";
            chartData.Clear();
            var colors = new[] { Color.FromArgb(76, 175, 80), Color.FromArgb(33, 150, 243), Color.FromArgb(255, 152, 0), 
                                Color.FromArgb(156, 39, 176), Color.FromArgb(244, 67, 54), Color.FromArgb(0, 188, 212),
                                Color.FromArgb(255, 193, 7), Color.FromArgb(103, 58, 183), Color.FromArgb(233, 30, 99), Color.FromArgb(63, 81, 181) };
            for (int i = 0; i < topSellers.Count; i++)
            {
                chartData.Add(new ChartData 
                { 
                    Label = topSellers[i].Product.Length > 15 ? topSellers[i].Product.Substring(0, 15) + "..." : topSellers[i].Product,
                    Value = topSellers[i].TotalSold,
                    Color = colors[i % colors.Length]
                });
            }
            pnlChart.Invalidate();
        }

        private void GenerateSlowMoving()
        {
            var slowMoving = DataStore.Perfumes
                .Where(p => p.StockLevel > 0)
                .Select(p => new
                {
                    p.Name,
                    p.Brand,
                    p.StockLevel,
                    TotalSold = DataStore.SaleItems.Where(si => si.PerfumeId == p.Id).Sum(si => (int?)si.Quantity) ?? 0,
                    DaysInStock = (DateTime.Now - p.CreatedDate).Days
                })
                .Where(p => p.TotalSold < 5)
                .OrderBy(p => p.TotalSold)
                .ToList();

            dgvReport.DataSource = slowMoving;
            lblSummary.Text = $"Slow Moving Items: {slowMoving.Count} products with less than 5 sales";
        }

        private void GenerateLowStock()
        {
            var lowStock = DataStore.Perfumes
                .Where(p => p.StockLevel <= p.LowStockThreshold)
                .Select(p => new
                {
                    p.Name,
                    p.Brand,
                    p.StockLevel,
                    p.LowStockThreshold,
                    Status = p.StockLevel == 0 ? "OUT OF STOCK" : "LOW STOCK",
                    Supplier = DataStore.Suppliers.FirstOrDefault(s => s.Id == p.SupplierId)?.Name ?? "Unknown"
                })
                .OrderBy(p => p.StockLevel)
                .ToList();

            dgvReport.DataSource = lowStock;
            lblSummary.Text = $"⚠️ {lowStock.Count} products need restocking";
        }

        private void GenerateProfitAnalysis()
        {
            // Get sales within date range
            var salesInRange = DataStore.Sales.Where(s => s.SaleDate >= dtpFrom.Value && s.SaleDate <= dtpTo.Value).Select(s => s.Id).ToList();
            
            var profitData = DataStore.SaleItems
                .Where(si => salesInRange.Contains(si.SaleId))
                .GroupBy(si => si.PerfumeId)
                .Select(g => {
                    var perfume = DataStore.Perfumes.FirstOrDefault(p => p.Id == g.Key);
                    var totalQty = g.Sum(si => si.Quantity);
                    var totalRevenue = g.Sum(si => si.Subtotal);
                    var totalCost = perfume != null ? perfume.CostPrice * totalQty : 0;
                    var totalProfit = totalRevenue - totalCost;
                    var profitMargin = totalRevenue > 0 ? (totalProfit / totalRevenue * 100) : 0;
                    
                    return new
                    {
                        Product = g.First().PerfumeName,
                        QuantitySold = totalQty,
                        Revenue = "₱" + totalRevenue.ToString("N2"),
                        Cost = "₱" + totalCost.ToString("N2"),
                        Profit = "₱" + totalProfit.ToString("N2"),
                        ProfitMargin = profitMargin.ToString("N1") + "%"
                    };
                })
                .OrderByDescending(p => p.QuantitySold)
                .ToList();

            dgvReport.DataSource = profitData;
            
            var totalRevenue = DataStore.SaleItems.Where(si => salesInRange.Contains(si.SaleId)).Sum(si => si.Subtotal);
            var totalCost = DataStore.SaleItems.Where(si => salesInRange.Contains(si.SaleId))
                .Sum(si => (DataStore.Perfumes.FirstOrDefault(p => p.Id == si.PerfumeId)?.CostPrice ?? 0) * si.Quantity);
            var totalProfit = totalRevenue - totalCost;
            var avgMargin = totalRevenue > 0 ? (totalProfit / totalRevenue * 100) : 0;
            
            lblSummary.Text = $"Total Profit: ₱{totalProfit:N2} | Avg Margin: {avgMargin:N2}%";

            // Prepare chart data - show profit vs cost
            currentReportType = "Profit Analysis";
            chartData.Clear();
            chartData.Add(new ChartData { Label = "Profit", Value = totalProfit, Color = Color.FromArgb(76, 175, 80) });
            chartData.Add(new ChartData { Label = "Cost", Value = totalCost, Color = Color.FromArgb(244, 67, 54) });
            pnlChart.Invalidate();
        }

        private void GenerateCustomerHistory()
        {
            // Get all customers with their purchase data
            var allCustomers = DataStore.Customers
                .Select(c => new
                {
                    c.Name,
                    c.Phone,
                    TotalPurchases = DataStore.Sales.Count(s => s.CustomerId == c.Id),
                    TotalSpent = DataStore.Sales.Where(s => s.CustomerId == c.Id).Sum(s => (decimal?)s.Total) ?? 0,
                    TotalQuantity = DataStore.SaleItems
                        .Where(si => DataStore.Sales.Any(s => s.Id == si.SaleId && s.CustomerId == c.Id))
                        .Sum(si => (int?)si.Quantity) ?? 0,
                    LastPurchase = DataStore.Sales.Where(s => s.CustomerId == c.Id).Max(s => (DateTime?)s.SaleDate)?.ToString("MM/dd/yyyy") ?? "Never"
                })
                .ToList();

            // Get resellers
            var resellers = DataStore.Resellers
                .Where(r => r.IsActive)
                .Select(r => new
                {
                    Name = r.Name,
                    Phone = r.Phone,
                    TotalPurchases = DataStore.Sales.Count(s => s.ResellerId == r.Id),
                    TotalSpent = DataStore.Sales.Where(s => s.ResellerId == r.Id).Sum(s => (decimal?)s.Total) ?? 0,
                    TotalQuantity = DataStore.SaleItems
                        .Where(si => DataStore.Sales.Any(s => s.Id == si.SaleId && s.ResellerId == r.Id))
                        .Sum(si => (int?)si.Quantity) ?? 0,
                    LastPurchase = DataStore.Sales.Where(s => s.ResellerId == r.Id).Max(s => (DateTime?)s.SaleDate)?.ToString("MM/dd/yyyy") ?? "Never"
                })
                .ToList();

            // Combine and categorize
            var combinedData = allCustomers
                .Select(c => new
                {
                    c.Name,
                    c.Phone,
                    c.TotalPurchases,
                    TotalSpent = "₱" + c.TotalSpent.ToString("N2"),
                    c.TotalQuantity,
                    CustomerType = c.TotalPurchases >= 5 ? "Regular Customer" : "New Customer",
                    c.LastPurchase
                })
                .Concat(resellers.Select(r => new
                {
                    r.Name,
                    r.Phone,
                    r.TotalPurchases,
                    TotalSpent = "₱" + r.TotalSpent.ToString("N2"),
                    r.TotalQuantity,
                    CustomerType = "Reseller",
                    r.LastPurchase
                }))
                .OrderByDescending(c => c.TotalPurchases)
                .ToList();

            dgvReport.DataSource = combinedData;
            
            var totalCustomers = allCustomers.Count;
            var totalResellers = resellers.Count;
            var regularCustomers = allCustomers.Count(c => c.TotalPurchases >= 5);
            var newCustomers = allCustomers.Count(c => c.TotalPurchases < 5 && c.TotalPurchases > 0);
            
            lblSummary.Text = $"Total: {totalCustomers + totalResellers} | Regular: {regularCustomers} | New: {newCustomers} | Resellers: {totalResellers}";

            // Prepare chart data - show PHP amount and total quantity for top customers/resellers
            currentReportType = "Customer History";
            chartData.Clear();
            
            // Get top 10 by total spent (combining customers and resellers)
            var topBySpent = allCustomers
                .Select(c => new { c.Name, Amount = c.TotalSpent, Quantity = c.TotalQuantity, Type = "Customer" })
                .Concat(resellers.Select(r => new { r.Name, Amount = r.TotalSpent, Quantity = r.TotalQuantity, Type = "Reseller" }))
                .Where(x => x.Amount > 0)
                .OrderByDescending(x => x.Amount)
                .Take(10)
                .ToList();

            var colors = new[] { 
                Color.FromArgb(33, 150, 243), Color.FromArgb(76, 175, 80), Color.FromArgb(255, 152, 0), 
                Color.FromArgb(156, 39, 176), Color.FromArgb(244, 67, 54), Color.FromArgb(0, 188, 212),
                Color.FromArgb(255, 193, 7), Color.FromArgb(103, 58, 183), Color.FromArgb(233, 30, 99), Color.FromArgb(63, 81, 181) 
            };
            
            for (int i = 0; i < topBySpent.Count; i++)
            {
                var customer = topBySpent[i];
                chartData.Add(new ChartData 
                { 
                    Label = customer.Name.Length > 12 ? customer.Name.Substring(0, 12) : customer.Name,
                    Value = customer.Amount,
                    Quantity = customer.Quantity,
                    Color = colors[i % colors.Length]
                });
            }
            pnlChart.Invalidate();
        }

        private void GenerateSupplierPerformance()
        {
            var supplierData = DataStore.Suppliers
                .Select(s => new
                {
                    s.Name,
                    s.ContactPerson,
                    s.Phone,
                    TotalProducts = DataStore.Perfumes.Count(p => p.SupplierId == s.Id),
                    TotalPurchaseOrders = DataStore.PurchaseOrders.Count(po => po.SupplierId == s.Id),
                    CompletedOrders = DataStore.PurchaseOrders.Count(po => po.SupplierId == s.Id && po.Status == "Completed")
                })
                .ToList();

            dgvReport.DataSource = supplierData;
            lblSummary.Text = $"Total Suppliers: {supplierData.Count}";
        }

        private void GenerateExpiringProducts()
        {
            var expiringProducts = DataStore.Perfumes
                .Where(p => p.ExpirationDate <= DateTime.Now.AddMonths(6))
                .Select(p => new
                {
                    p.Name,
                    p.Brand,
                    p.BatchNumber,
                    ExpirationDate = p.ExpirationDate.ToString("MM/dd/yyyy"),
                    DaysUntilExpiry = (p.ExpirationDate - DateTime.Now).Days,
                    p.StockLevel,
                    Status = (p.ExpirationDate - DateTime.Now).Days < 0 ? "EXPIRED" : (p.ExpirationDate - DateTime.Now).Days < 90 ? "CRITICAL" : "WARNING"
                })
                .OrderBy(p => p.DaysUntilExpiry)
                .ToList();

            dgvReport.DataSource = expiringProducts;
            lblSummary.Text = $"⚠️ {expiringProducts.Count} products expiring within 6 months";
            
            // Clear chart for this report
            currentReportType = "";
            chartData.Clear();
            pnlChart.Invalidate();
        }

        private void PnlChart_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (chartData.Count == 0)
            {
                // Show placeholder
                var placeholderText = "Generate a report to see visual analytics";
                var font = new Font("Segoe UI", 10, FontStyle.Italic);
                var size = g.MeasureString(placeholderText, font);
                g.DrawString(placeholderText, font, Brushes.Gray, 
                    (pnlChart.Width - size.Width) / 2, (pnlChart.Height - size.Height) / 2 + 20);
                return;
            }

            if (currentReportType == "Profit Analysis")
            {
                // Draw pie chart for profit analysis
                DrawPieChart(g);
            }
            else
            {
                // Draw bar chart for other reports
                DrawBarChart(g);
            }
        }

        private void DrawBarChart(Graphics g)
        {
            if (chartData.Count == 0) return;

            int chartX = 50;
            int chartY = 50;
            int chartWidth = 1000;
            int chartHeight = 80;
            int barWidth = Math.Min(80, chartWidth / chartData.Count - 10);

            // Draw axes
            g.DrawLine(Pens.Gray, chartX, chartY + chartHeight, chartX + chartWidth, chartY + chartHeight);
            g.DrawLine(Pens.Gray, chartX, chartY, chartX, chartY + chartHeight);

            // Find max for scaling
            decimal maxValue = chartData.Max(d => d.Value);
            if (maxValue == 0) maxValue = 1;

            // Draw bars
            for (int i = 0; i < chartData.Count; i++)
            {
                var data = chartData[i];
                int barHeight = (int)((double)data.Value / (double)maxValue * chartHeight);
                int x = chartX + i * (barWidth + 10) + 5;
                int y = chartY + chartHeight - barHeight;

                // Draw bar
                using (var brush = new SolidBrush(data.Color))
                {
                    g.FillRectangle(brush, x, y, barWidth, barHeight);
                }

                // Draw label
                var labelSize = g.MeasureString(data.Label, new Font("Segoe UI", 7));
                g.DrawString(data.Label, new Font("Segoe UI", 7), Brushes.Black, 
                    x + barWidth / 2 - labelSize.Width / 2, chartY + chartHeight + 5);

                // Draw value on bar (amount and quantity for Customer History)
                if (barHeight > 20)
                {
                    if (currentReportType == "Customer History")
                    {
                        // Show both amount and quantity
                        var amountText = "₱" + data.Value.ToString("N0");
                        var qtyText = data.Quantity + " pcs";
                        var amountSize = g.MeasureString(amountText, new Font("Segoe UI", 7, FontStyle.Bold));
                        var qtySize = g.MeasureString(qtyText, new Font("Segoe UI", 6));
                        
                        g.DrawString(amountText, new Font("Segoe UI", 7, FontStyle.Bold), Brushes.White,
                            x + barWidth / 2 - amountSize.Width / 2, y + 5);
                        g.DrawString(qtyText, new Font("Segoe UI", 6), Brushes.White,
                            x + barWidth / 2 - qtySize.Width / 2, y + 18);
                    }
                    else
                    {
                        var valueText = data.Value.ToString("N0");
                        var valueSize = g.MeasureString(valueText, new Font("Segoe UI", 7, FontStyle.Bold));
                        g.DrawString(valueText, new Font("Segoe UI", 7, FontStyle.Bold), Brushes.White,
                            x + barWidth / 2 - valueSize.Width / 2, y + 5);
                    }
                }
            }
        }

        private void DrawPieChart(Graphics g)
        {
            if (chartData.Count == 0) return;

            int pieX = 400;
            int pieY = 50;
            int pieSize = 80;

            decimal total = chartData.Sum(d => d.Value);
            if (total == 0) return;

            float startAngle = 0;
            for (int i = 0; i < chartData.Count; i++)
            {
                var data = chartData[i];
                float sweepAngle = (float)(data.Value / total * 360);

                using (var brush = new SolidBrush(data.Color))
                {
                    g.FillPie(brush, pieX, pieY, pieSize, pieSize, startAngle, sweepAngle);
                }

                startAngle += sweepAngle;
            }

            // Draw legend
            int legendX = pieX + pieSize + 30;
            int legendY = pieY;
            for (int i = 0; i < chartData.Count; i++)
            {
                var data = chartData[i];
                g.FillRectangle(new SolidBrush(data.Color), legendX, legendY + i * 25, 15, 15);
                g.DrawString($"{data.Label}: ₱{data.Value:N2}", new Font("Segoe UI", 9), Brushes.Black, legendX + 20, legendY + i * 25);
            }
        }

        private void GenerateCategoryPerformance()
        {
            var salesInRange = DataStore.Sales.Where(s => s.SaleDate >= dtpFrom.Value && s.SaleDate <= dtpTo.Value).Select(s => s.Id).ToList();
            
            var categoryData = DataStore.SaleItems
                .Where(si => salesInRange.Contains(si.SaleId))
                .GroupBy(si => {
                    var perfume = DataStore.Perfumes.FirstOrDefault(p => p.Id == si.PerfumeId);
                    return perfume?.Category ?? "Unknown";
                })
                .Select(g => new
                {
                    Category = g.Key,
                    TotalSold = g.Sum(si => si.Quantity),
                    Revenue = "₱" + g.Sum(si => si.Subtotal).ToString("N2"),
                    AvgPrice = "₱" + (g.Sum(si => si.Subtotal) / g.Sum(si => si.Quantity)).ToString("N2"),
                    Products = g.Select(si => si.PerfumeId).Distinct().Count()
                })
                .OrderByDescending(c => c.TotalSold)
                .ToList();

            dgvReport.DataSource = categoryData;
            lblSummary.Text = $"Category Performance Analysis - {categoryData.Count} categories";

            // Prepare chart data
            currentReportType = "Category Performance";
            chartData.Clear();
            var colors = new[] { Color.FromArgb(156, 39, 176), Color.FromArgb(233, 30, 99), Color.FromArgb(244, 67, 54), 
                                Color.FromArgb(255, 152, 0), Color.FromArgb(255, 193, 7), Color.FromArgb(76, 175, 80),
                                Color.FromArgb(0, 150, 136), Color.FromArgb(0, 188, 212), Color.FromArgb(33, 150, 243), Color.FromArgb(63, 81, 181) };
            for (int i = 0; i < Math.Min(categoryData.Count, 10); i++)
            {
                chartData.Add(new ChartData 
                { 
                    Label = categoryData[i].Category.Length > 12 ? categoryData[i].Category.Substring(0, 12) : categoryData[i].Category,
                    Value = categoryData[i].TotalSold,
                    Color = colors[i % colors.Length]
                });
            }
            pnlChart.Invalidate();
        }

        private void GenerateMonthlyTrends()
        {
            var monthlyData = new List<object>();
            
            for (int i = 11; i >= 0; i--)
            {
                var month = DateTime.Today.AddMonths(-i);
                var monthStart = new DateTime(month.Year, month.Month, 1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);
                
                var sales = DataStore.Sales.Where(s => s.SaleDate >= monthStart && s.SaleDate <= monthEnd).ToList();
                var totalBottles = sales.SelectMany(s => DataStore.SaleItems.Where(si => si.SaleId == s.Id)).Sum(si => (int?)si.Quantity) ?? 0;
                var revenue = sales.Sum(s => s.Total);
                var avgSale = sales.Count > 0 ? revenue / sales.Count : 0;
                
                monthlyData.Add(new
                {
                    Month = month.ToString("MMM yyyy"),
                    Transactions = sales.Count,
                    TotalBottles = totalBottles,
                    Revenue = "₱" + revenue.ToString("N2"),
                    AvgSale = "₱" + avgSale.ToString("N2"),
                    TopCategory = sales.SelectMany(s => DataStore.SaleItems.Where(si => si.SaleId == s.Id))
                        .GroupBy(si => DataStore.Perfumes.FirstOrDefault(p => p.Id == si.PerfumeId)?.Category ?? "Unknown")
                        .OrderByDescending(g => g.Sum(si => si.Quantity))
                        .Select(g => g.Key)
                        .FirstOrDefault() ?? "N/A"
                });
            }

            dgvReport.DataSource = monthlyData;
            
            var totalRevenue = DataStore.Sales.Sum(s => s.Total);
            var totalTransactions = DataStore.Sales.Count;
            lblSummary.Text = $"12-Month Trend Analysis | Total Revenue: ₱{totalRevenue:N2} | Transactions: {totalTransactions}";

            // Prepare chart data - show last 12 months revenue
            currentReportType = "Monthly Trends";
            chartData.Clear();
            var colors = new[] { Color.FromArgb(63, 81, 181), Color.FromArgb(33, 150, 243), Color.FromArgb(0, 188, 212), 
                                Color.FromArgb(0, 150, 136), Color.FromArgb(76, 175, 80), Color.FromArgb(139, 195, 74),
                                Color.FromArgb(255, 193, 7), Color.FromArgb(255, 152, 0), Color.FromArgb(255, 87, 34), 
                                Color.FromArgb(244, 67, 54), Color.FromArgb(233, 30, 99), Color.FromArgb(156, 39, 176) };
            
            for (int i = 11; i >= 0; i--)
            {
                var month = DateTime.Today.AddMonths(-i);
                var monthStart = new DateTime(month.Year, month.Month, 1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);
                var revenue = DataStore.Sales.Where(s => s.SaleDate >= monthStart && s.SaleDate <= monthEnd).Sum(s => s.Total);
                
                chartData.Add(new ChartData 
                { 
                    Label = month.ToString("MMM"),
                    Value = revenue,
                    Color = colors[11 - i]
                });
            }
            pnlChart.Invalidate();
        }

        private void GeneratePaymentMethodsAnalysis()
        {
            var salesInRange = DataStore.Sales.Where(s => s.SaleDate >= dtpFrom.Value && s.SaleDate <= dtpTo.Value).ToList();
            
            var paymentData = salesInRange
                .GroupBy(s => s.PaymentMethod)
                .Select(g => new
                {
                    PaymentMethod = g.Key,
                    Transactions = g.Count(),
                    TotalAmount = "₱" + g.Sum(s => s.Total).ToString("N2"),
                    AvgTransaction = "₱" + (g.Sum(s => s.Total) / g.Count()).ToString("N2"),
                    Percentage = ((decimal)g.Count() / salesInRange.Count * 100).ToString("N1") + "%"
                })
                .OrderByDescending(p => p.Transactions)
                .ToList();

            dgvReport.DataSource = paymentData;
            
            var totalAmount = salesInRange.Sum(s => s.Total);
            lblSummary.Text = $"Payment Methods Analysis | Total: ₱{totalAmount:N2} | Transactions: {salesInRange.Count}";

            // Prepare chart data - pie chart
            currentReportType = "Profit Analysis"; // Use pie chart
            chartData.Clear();
            var colors = new[] { Color.FromArgb(76, 175, 80), Color.FromArgb(33, 150, 243), Color.FromArgb(255, 152, 0), 
                                Color.FromArgb(156, 39, 176), Color.FromArgb(244, 67, 54) };
            
            for (int i = 0; i < paymentData.Count; i++)
            {
                var amount = decimal.Parse(paymentData[i].TotalAmount.Replace("₱", "").Replace(",", ""));
                chartData.Add(new ChartData 
                { 
                    Label = paymentData[i].PaymentMethod,
                    Value = amount,
                    Color = colors[i % colors.Length]
                });
            }
            pnlChart.Invalidate();
        }
    }
}