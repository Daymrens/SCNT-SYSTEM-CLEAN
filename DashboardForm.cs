using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PerfumeInventory
{
    public class DashboardForm : Form
    {
        private Panel pnlHeader, pnlStats, pnlCharts;
        private Label lblWelcome;
        private Label lblTotalSalesValue, lblTotalRevenueValue, lblLowStockValue, lblTotalProductsValue;
        private DataGridView dgvRecentSales, dgvLowStock, dgvTopSelling, dgvMonthlyTrends, dgvResellerStats;
        private Panel pnlSalesChart, pnlProfitChart;

        public DashboardForm()
        {
            InitializeComponent();
            LoadDashboardData();
        }

        private void InitializeComponent()
        {
            this.Text = "Dashboard";
            this.Size = new Size(1200, 800);
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.AutoScroll = true;

            // Header
            pnlHeader = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(1200, 70),
                BackColor = Color.White
            };

            lblWelcome = new Label
            {
                Text = $"Welcome back, {DataStore.CurrentUser?.FullName}! 👋",
                Location = new Point(30, 20),
                Size = new Size(600, 30),
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            pnlHeader.Controls.Add(lblWelcome);

            // Stats Panel
            pnlStats = new Panel
            {
                Location = new Point(30, 90),
                Size = new Size(1140, 110),
                BackColor = Color.Transparent
            };

            CreateStatCard("Total Products", "0", Color.FromArgb(13, 110, 253), 0, out lblTotalProductsValue);
            CreateStatCard("Total Bottles Sold", "0", Color.FromArgb(25, 135, 84), 290, out lblTotalSalesValue);
            CreateStatCard("Total Revenue", "₱0.00", Color.FromArgb(255, 193, 7), 580, out lblTotalRevenueValue);
            CreateStatCard("Low Stock", "0", Color.FromArgb(220, 53, 69), 870, out lblLowStockValue);

            // Charts Panel
            pnlCharts = new Panel
            {
                Location = new Point(30, 220),
                Size = new Size(1140, 260),
                BackColor = Color.Transparent
            };

            CreateSalesChart();
            CreateProfitChart();

            // Low Stock Panel
            Color lowStockColor = Color.FromArgb(220, 53, 69);
            Panel pnlLowStock = CreateModernCard(new Point(30, 500), new Size(360, 250), "⚠️ Low Stock Alert", lowStockColor);
            dgvLowStock = CreateModernDataGrid(new Point(15, 55), new Size(330, 180));
            pnlLowStock.Controls.Add(dgvLowStock);

            // Top Selling Panel
            Color topSellingColor = Color.FromArgb(25, 135, 84);
            Panel pnlTopSelling = CreateModernCard(new Point(410, 500), new Size(360, 250), "🏆 Top Selling", topSellingColor);
            dgvTopSelling = CreateModernDataGrid(new Point(15, 55), new Size(330, 180));
            pnlTopSelling.Controls.Add(dgvTopSelling);

            // Reseller Stats Panel
            Color resellerColor = Color.FromArgb(111, 66, 193);
            Panel pnlResellerStats = CreateModernCard(new Point(790, 500), new Size(380, 250), "🤝 Reseller Performance", resellerColor);
            dgvResellerStats = CreateModernDataGrid(new Point(15, 55), new Size(350, 180));
            pnlResellerStats.Controls.Add(dgvResellerStats);

            // Monthly Trends Panel
            Color trendsColor = Color.FromArgb(13, 110, 253);
            Panel pnlMonthlyTrends = CreateModernCard(new Point(30, 770), new Size(560, 270), "📅 Monthly Trends", trendsColor);
            dgvMonthlyTrends = CreateModernDataGrid(new Point(15, 55), new Size(530, 200));
            pnlMonthlyTrends.Controls.Add(dgvMonthlyTrends);

            // Recent Sales Panel
            Color salesColor = Color.FromArgb(13, 202, 240);
            Panel pnlRecentSales = CreateModernCard(new Point(610, 770), new Size(560, 270), "📊 Recent Sales", salesColor);
            dgvRecentSales = CreateModernDataGrid(new Point(15, 55), new Size(530, 200));
            pnlRecentSales.Controls.Add(dgvRecentSales);

            this.Controls.AddRange(new Control[] {
                pnlHeader, pnlStats, pnlCharts,
                pnlLowStock, pnlTopSelling, pnlResellerStats,
                pnlMonthlyTrends, pnlRecentSales
            });
        }

        private Panel CreateModernCard(Point location, Size size, string title, Color accentColor)
        {
            Panel card = new Panel
            {
                Location = location,
                Size = size,
                BackColor = Color.White
            };

            Label lblTitle = new Label
            {
                Text = title,
                Location = new Point(15, 15),
                Size = new Size(size.Width - 30, 30),
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = accentColor
            };

            card.Controls.Add(lblTitle);
            return card;
        }

        private DataGridView CreateModernDataGrid(Point location, Size size)
        {
            var dgv = new DataGridView
            {
                Location = location,
                Size = size,
                AutoGenerateColumns = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 9),
                ColumnHeadersHeight = 35,
                RowTemplate = { Height = 32 },
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(240, 240, 240),
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                AllowUserToResizeRows = false
            };

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(73, 80, 87);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 244, 253);
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dgv.DefaultCellStyle.Padding = new Padding(8, 4, 4, 4);

            return dgv;
        }

        private Panel CreateCardPanel(Point location, Size size, Color accentColor)
        {
            Panel outerPanel = new Panel
            {
                Location = location,
                Size = size,
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.None
            };

            // Colored top bar
            Panel topBar = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(size.Width, 4),
                BackColor = accentColor
            };

            // Main content panel
            Panel contentPanel = new Panel
            {
                Location = new Point(0, 4),
                Size = new Size(size.Width, size.Height - 4),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            // Add shadow effect
            outerPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                
                // Draw shadow
                using (var shadowBrush = new SolidBrush(Color.FromArgb(15, 0, 0, 0)))
                {
                    e.Graphics.FillRectangle(shadowBrush, 2, 2, size.Width - 2, size.Height - 2);
                }
                
                // Draw border
                using (var pen = new Pen(Color.FromArgb(230, 230, 235), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, size.Width - 1, size.Height - 1);
                }
            };

            outerPanel.Controls.AddRange(new Control[] { topBar, contentPanel });
            return outerPanel;
        }


        private DataGridView CreateStyledDataGrid(Point location, Size size, Color accentColor)
        {
            var dgv = new DataGridView
            {
                Location = location,
                Size = size,
                AutoGenerateColumns = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 9),
                GridColor = Color.FromArgb(240, 242, 245),
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = accentColor,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    Padding = new Padding(8, 5, 8, 5),
                    SelectionBackColor = accentColor,
                    SelectionForeColor = Color.White,
                    Alignment = DataGridViewContentAlignment.MiddleLeft
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    SelectionBackColor = Color.FromArgb(240, 248, 255),
                    SelectionForeColor = Color.FromArgb(33, 37, 41),
                    Padding = new Padding(8, 4, 8, 4),
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(52, 58, 64)
                },
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle 
                { 
                    BackColor = Color.FromArgb(248, 250, 252),
                    SelectionBackColor = Color.FromArgb(240, 248, 255),
                    SelectionForeColor = Color.FromArgb(33, 37, 41),
                    Padding = new Padding(8, 4, 8, 4)
                },
                ColumnHeadersHeight = 38,
                RowTemplate = { Height = 36 },
                EnableHeadersVisualStyles = false,
                CellBorderStyle = DataGridViewCellBorderStyle.None
            };

            return dgv;
        }

        private void CreateStatCard(string title, string value, Color color, int x, out Label valueLabel)
        {
            Panel card = new Panel
            {
                Location = new Point(x, 10),
                Size = new Size(270, 90),
                BackColor = color
            };

            // Add subtle shadow effect
            card.Paint += (s, e) =>
            {
                using (var shadowBrush = new SolidBrush(Color.FromArgb(20, 0, 0, 0)))
                {
                    e.Graphics.FillRectangle(shadowBrush, 2, 2, 268, 88);
                }
            };

            Label lblTitle = new Label
            {
                Text = title,
                Location = new Point(20, 20),
                Size = new Size(230, 22),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            valueLabel = new Label
            {
                Text = value,
                Location = new Point(20, 45),
                Size = new Size(230, 35),
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            card.Controls.AddRange(new Control[] { lblTitle, valueLabel });
            pnlStats.Controls.Add(card);
        }


        private void CreateSalesChart()
        {
            pnlSalesChart = new Panel
            {
                Location = new Point(0, 10),
                Size = new Size(550, 240),
                BackColor = Color.White
            };

            Label lblTitle = new Label
            {
                Text = "📈 Last 7 Days Sales",
                Location = new Point(20, 15),
                Size = new Size(250, 30),
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            pnlSalesChart.Controls.Add(lblTitle);
            pnlSalesChart.Paint += PnlSalesChart_Paint;
            pnlCharts.Controls.Add(pnlSalesChart);
        }

        private void CreateProfitChart()
        {
            pnlProfitChart = new Panel
            {
                Location = new Point(590, 10),
                Size = new Size(550, 240),
                BackColor = Color.White
            };

            Label lblTitle = new Label
            {
                Text = "💰 Profit Analysis",
                Location = new Point(20, 15),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            pnlProfitChart.Controls.Add(lblTitle);
            pnlProfitChart.Paint += PnlProfitChart_Paint;
            pnlCharts.Controls.Add(pnlProfitChart);
        }

        private void PnlSalesChart_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Get last 7 days sales data - quantity of perfumes sold
            var last7Days = new List<(string Day, int Quantity, decimal Revenue)>();
            for (int i = 6; i >= 0; i--)
            {
                var date = DateTime.Today.AddDays(-i);
                var sales = DataStore.Sales.Where(s => s.SaleDate.Date == date).ToList();
                int quantity = DataStore.SaleItems
                    .Where(si => sales.Any(s => s.Id == si.SaleId))
                    .Sum(si => si.Quantity);
                last7Days.Add((date.ToString("ddd"), quantity, sales.Sum(s => s.Total)));
            }

            // Draw bar chart
            int chartX = 40;
            int chartY = 60;
            int chartWidth = 480;
            int chartHeight = 140;
            int barWidth = chartWidth / 7 - 15;

            // Draw axes with modern style
            using (var pen = new Pen(Color.FromArgb(220, 220, 220), 2))
            {
                g.DrawLine(pen, chartX, chartY + chartHeight, chartX + chartWidth, chartY + chartHeight);
            }

            // Find max for scaling
            int maxQuantity = last7Days.Max(d => d.Quantity);
            if (maxQuantity == 0) maxQuantity = 1;

            // Draw bars with gradient
            for (int i = 0; i < last7Days.Count; i++)
            {
                var data = last7Days[i];
                int barHeight = (int)((double)data.Quantity / maxQuantity * chartHeight);
                int x = chartX + i * (barWidth + 15) + 5;
                int y = chartY + chartHeight - barHeight;

                // Draw bar with gradient (only if height > 0)
                if (barHeight > 0)
                {
                    using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                        new Rectangle(x, y, barWidth, Math.Max(barHeight, 1)),
                        Color.FromArgb(13, 110, 253),
                        Color.FromArgb(102, 16, 242),
                        90f))
                    {
                        g.FillRectangle(brush, x, y, barWidth, barHeight);
                    }

                    // Draw quantity on top of bar
                    if (barHeight > 20)
                    {
                        var quantityStr = data.Quantity.ToString();
                        var quantitySize = g.MeasureString(quantityStr, new Font("Segoe UI", 9, FontStyle.Bold));
                        g.DrawString(quantityStr, new Font("Segoe UI", 9, FontStyle.Bold), Brushes.White,
                            x + barWidth / 2 - quantitySize.Width / 2, y + 5);
                    }
                }

                // Draw day label
                var daySize = g.MeasureString(data.Day, new Font("Segoe UI", 9, FontStyle.Bold));
                g.DrawString(data.Day, new Font("Segoe UI", 9, FontStyle.Bold), 
                    new SolidBrush(Color.FromArgb(73, 80, 87)), 
                    x + barWidth / 2 - daySize.Width / 2, chartY + chartHeight + 8);
            }
        }

        private void PnlProfitChart_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Calculate profit metrics
            var last30Days = DataStore.Sales.Where(s => s.SaleDate >= DateTime.Today.AddDays(-30)).ToList();
            decimal totalRevenue = last30Days.Sum(s => s.Total);
            decimal totalCost = 0;

            foreach (var sale in last30Days)
            {
                var items = DataStore.SaleItems.Where(si => si.SaleId == sale.Id);
                foreach (var item in items)
                {
                    var perfume = DataStore.Perfumes.FirstOrDefault(p => p.Id == item.PerfumeId);
                    if (perfume != null)
                    {
                        totalCost += perfume.CostPrice * item.Quantity;
                    }
                }
            }

            decimal totalProfit = totalRevenue - totalCost;
            decimal profitMargin = totalRevenue > 0 ? (totalProfit / totalRevenue * 100) : 0;

            // Draw profit info with modern styling
            int startY = 60;
            int lineHeight = 32;
            
            g.DrawString("Revenue", new Font("Segoe UI", 10), 
                new SolidBrush(Color.FromArgb(108, 117, 125)), 40, startY);
            g.DrawString($"₱{totalRevenue:N2}", new Font("Segoe UI", 12, FontStyle.Bold), 
                new SolidBrush(Color.FromArgb(33, 37, 41)), 40, startY + 18);
            
            g.DrawString("Cost", new Font("Segoe UI", 10), 
                new SolidBrush(Color.FromArgb(108, 117, 125)), 40, startY + lineHeight + 18);
            g.DrawString($"₱{totalCost:N2}", new Font("Segoe UI", 12, FontStyle.Bold), 
                new SolidBrush(Color.FromArgb(220, 53, 69)), 40, startY + lineHeight + 36);
            
            g.DrawString("Profit", new Font("Segoe UI", 10), 
                new SolidBrush(Color.FromArgb(108, 117, 125)), 40, startY + (lineHeight * 2) + 36);
            g.DrawString($"₱{totalProfit:N2}", new Font("Segoe UI", 12, FontStyle.Bold), 
                new SolidBrush(Color.FromArgb(25, 135, 84)), 40, startY + (lineHeight * 2) + 54);
            
            g.DrawString("Margin", new Font("Segoe UI", 10), 
                new SolidBrush(Color.FromArgb(108, 117, 125)), 40, startY + (lineHeight * 3) + 54);
            g.DrawString($"{profitMargin:N1}%", new Font("Segoe UI", 12, FontStyle.Bold), 
                new SolidBrush(Color.FromArgb(255, 193, 7)), 40, startY + (lineHeight * 3) + 72);

            // Draw modern donut chart
            int pieX = 320;
            int pieY = 80;
            int pieSize = 120;
            int holeSize = 70;

            if (totalRevenue > 0)
            {
                float profitAngle = (float)(totalProfit / totalRevenue * 360);
                float costAngle = 360 - profitAngle;

                // Draw cost slice
                using (var brush = new SolidBrush(Color.FromArgb(220, 53, 69)))
                {
                    g.FillPie(brush, pieX, pieY, pieSize, pieSize, 0, costAngle);
                }

                // Draw profit slice
                using (var brush = new SolidBrush(Color.FromArgb(25, 135, 84)))
                {
                    g.FillPie(brush, pieX, pieY, pieSize, pieSize, costAngle, profitAngle);
                }

                // Draw center hole for donut effect
                using (var brush = new SolidBrush(Color.White))
                {
                    int holeX = pieX + (pieSize - holeSize) / 2;
                    int holeY = pieY + (pieSize - holeSize) / 2;
                    g.FillEllipse(brush, holeX, holeY, holeSize, holeSize);
                }

                // Draw percentage in center
                string percentText = $"{profitMargin:N0}%";
                var textSize = g.MeasureString(percentText, new Font("Segoe UI", 14, FontStyle.Bold));
                g.DrawString(percentText, new Font("Segoe UI", 14, FontStyle.Bold), 
                    new SolidBrush(Color.FromArgb(33, 37, 41)),
                    pieX + pieSize / 2 - textSize.Width / 2, 
                    pieY + pieSize / 2 - textSize.Height / 2);

                // Modern legend
                int legendX = pieX + 140;
                int legendY = pieY + 30;
                
                g.FillRectangle(new SolidBrush(Color.FromArgb(25, 135, 84)), legendX, legendY, 12, 12);
                g.DrawString("Profit", new Font("Segoe UI", 9), 
                    new SolidBrush(Color.FromArgb(73, 80, 87)), legendX + 18, legendY - 2);
                
                g.FillRectangle(new SolidBrush(Color.FromArgb(220, 53, 69)), legendX, legendY + 25, 12, 12);
                g.DrawString("Cost", new Font("Segoe UI", 9), 
                    new SolidBrush(Color.FromArgb(73, 80, 87)), legendX + 18, legendY + 23);
            }
        }

        private void LoadDashboardData()
                {
                    // Total Products
                    lblTotalProductsValue.Text = DataStore.Perfumes.Count.ToString();

                    // Total Bottles Sold - All time quantity of perfumes sold
                    int totalBottlesSold = DataStore.SaleItems.Sum(si => si.Quantity);
                    lblTotalSalesValue.Text = totalBottlesSold.ToString();

                    // Total Sales (All Time)
                    decimal totalRevenue = DataStore.Sales.Sum(s => s.Total);
                    lblTotalRevenueValue.Text = "₱" + totalRevenue.ToString("N2");

                    // Low Stock
                    var lowStock = DataStore.Perfumes.Where(p => p.StockLevel <= p.LowStockThreshold)
                        .OrderBy(p => p.StockLevel)
                        .Take(10)
                        .Select(p => new { 
                            Product = p.Name, 
                            Stock = p.StockLevel, 
                            Threshold = p.LowStockThreshold 
                        })
                        .ToList();
                    lblLowStockValue.Text = DataStore.Perfumes.Count(p => p.StockLevel <= p.LowStockThreshold).ToString();
                    dgvLowStock.DataSource = lowStock;

                    // Top Selling - Calculate from actual sales
                    var topSelling = DataStore.SaleItems
                        .GroupBy(si => si.PerfumeId)
                        .Select(g => new
                        {
                            PerfumeId = g.Key,
                            TotalSold = g.Sum(si => si.Quantity),
                            Revenue = g.Sum(si => si.Subtotal)
                        })
                        .OrderByDescending(x => x.TotalSold)
                        .Take(10)
                        .Select(x => new
                        {
                            Product = DataStore.Perfumes.FirstOrDefault(p => p.Id == x.PerfumeId)?.Name ?? "Unknown",
                            Sold = x.TotalSold,
                            Revenue = "₱" + x.Revenue.ToString("N2")
                        })
                        .ToList();
                    dgvTopSelling.DataSource = topSelling;

                    // Reseller Stats - Show total items purchased
                    var resellerStats = DataStore.Resellers
                        .Where(r => r.IsActive)
                        .Select(r => new
                        {
                            Reseller = r.Name,
                            Items = DataStore.SaleItems
                                .Where(si => DataStore.Sales.Any(s => s.Id == si.SaleId && s.ResellerId == r.Id))
                                .Sum(si => si.Quantity),
                            Total = "₱" + DataStore.Sales.Where(s => s.ResellerId == r.Id).Sum(s => s.Total).ToString("N2")
                        })
                        .OrderByDescending(r => r.Items)
                        .ToList();
                    dgvResellerStats.DataSource = resellerStats;

                    // Monthly Trends (last 6 months)
                    var monthlyTrends = new List<object>();
                    for (int i = 5; i >= 0; i--)
                    {
                        var month = DateTime.Today.AddMonths(-i);
                        var sales = DataStore.Sales.Where(s => s.SaleDate.Year == month.Year && s.SaleDate.Month == month.Month).ToList();
                        monthlyTrends.Add(new
                        {
                            Month = month.ToString("MMM yyyy"),
                            Sales = sales.Count,
                            Revenue = "₱" + sales.Sum(s => s.Total).ToString("N2"),
                            AvgSale = sales.Count > 0 ? "₱" + (sales.Sum(s => s.Total) / sales.Count).ToString("N2") : "₱0.00"
                        });
                    }
                    dgvMonthlyTrends.DataSource = monthlyTrends;

                    // Recent Sales
                    var recentSales = DataStore.Sales.OrderByDescending(s => s.SaleDate)
                        .Take(10)
                        .Select(s => new
                        {
                            Date = s.SaleDate.ToString("MM/dd HH:mm"),
                            Customer = s.CustomerId.HasValue 
                                ? DataStore.Customers.FirstOrDefault(c => c.Id == s.CustomerId)?.Name ?? "Walk-in"
                                : (s.ResellerId.HasValue 
                                    ? DataStore.Resellers.FirstOrDefault(r => r.Id == s.ResellerId)?.Name ?? "Walk-in"
                                    : "Walk-in"),
                            Total = "₱" + s.Total.ToString("N2"),
                            Payment = s.PaymentMethod
                        })
                        .ToList();
                    dgvRecentSales.DataSource = recentSales;

                    // Refresh charts
                    pnlSalesChart?.Invalidate();
                    pnlProfitChart?.Invalidate();
                }

    }
}
