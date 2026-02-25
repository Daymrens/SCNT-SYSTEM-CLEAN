using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace PerfumeInventory
{
    public class ResellerDeliveryForm : Form
    {
        private DataGridView dgvProducts, dgvCart;
        private TextBox txtSearch, txtResellerSearch, txtDeliveryAddress, txtNotes;
        private Label lblReseller, lblSubtotal, lblDiscount, lblTotal;
        private Button btnAddToCart, btnRemoveFromCart, btnConfirmDelivery, btnPrintInvoice, btnClear;
        private DateTimePicker dtpDeliveryDate;
        private List<DeliveryCartItem> cart = new List<DeliveryCartItem>();
        private Reseller? selectedReseller;
        private Sale? completedSale;

        public ResellerDeliveryForm()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void InitializeComponent()
        {
            this.Text = "Reseller Delivery";
            this.Size = new Size(1400, 900);
            this.BackColor = Color.FromArgb(248, 249, 250);

            Label lblTitle = new Label
            {
                Text = "🚚 RESELLER DELIVERY",
                Location = new Point(30, 25),
                Size = new Size(400, 35),
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            // Products Panel
            Panel pnlProducts = new Panel
            {
                Location = new Point(30, 80),
                Size = new Size(550, 750),
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            Label lblProductsTitle = new Label
            {
                Text = "Products",
                Location = new Point(20, 20),
                Size = new Size(200, 30),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            txtSearch = new TextBox
            {
                Location = new Point(20, 60),
                Width = 510,
                Height = 35,
                Font = new Font("Segoe UI", 11),
                PlaceholderText = "🔍 Search products..."
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            dgvProducts = new DataGridView
            {
                Location = new Point(20, 110),
                Size = new Size(510, 550),
                AutoGenerateColumns = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 10),
                ColumnHeadersHeight = 45,
                RowTemplate = { Height = 40 },
                EnableHeadersVisualStyles = false
            };

            dgvProducts.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Product", Width = 180 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Brand", HeaderText = "Brand", Width = 120 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Size", HeaderText = "Size", Width = 80 },
                new DataGridViewTextBoxColumn { DataPropertyName = "SellingPrice", HeaderText = "Price", Width = 90, DefaultCellStyle = new DataGridViewCellStyle { Format = "₱#,##0.00" } },
                new DataGridViewTextBoxColumn { DataPropertyName = "StockLevel", HeaderText = "Stock", Width = 60 }
            });

            btnAddToCart = new Button
            {
                Text = "➕ Add to Cart",
                Location = new Point(20, 675),
                Size = new Size(510, 50),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAddToCart.FlatAppearance.BorderSize = 0;
            btnAddToCart.Click += BtnAddToCart_Click;

            pnlProducts.Controls.AddRange(new Control[] { lblProductsTitle, txtSearch, dgvProducts, btnAddToCart });

            // Cart & Delivery Panel
            Panel pnlCart = new Panel
            {
                Location = new Point(600, 80),
                Size = new Size(770, 750),
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            Label lblCartTitle = new Label
            {
                Text = "Delivery Details",
                Location = new Point(20, 20),
                Size = new Size(200, 30),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            // Reseller Selection
            Panel pnlReseller = new Panel
            {
                Location = new Point(20, 60),
                Size = new Size(730, 100),
                BackColor = Color.FromArgb(248, 249, 250)
            };

            Label lblResellerLabel = new Label
            {
                Text = "Select Reseller:",
                Location = new Point(15, 10),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(73, 80, 87)
            };

            txtResellerSearch = new TextBox
            {
                Location = new Point(15, 35),
                Width = 300,
                Height = 30,
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "Search by name or business..."
            };

            Button btnSearchReseller = new Button
            {
                Text = "🔍 Find",
                Location = new Point(325, 33),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSearchReseller.FlatAppearance.BorderSize = 0;
            btnSearchReseller.Click += BtnSearchReseller_Click;

            lblReseller = new Label
            {
                Text = "No reseller selected",
                Location = new Point(430, 35),
                Size = new Size(285, 30),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(220, 53, 69),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.FromArgb(248, 215, 218),
                Padding = new Padding(10, 5, 5, 5)
            };

            Label lblAddressLabel = new Label
            {
                Text = "Delivery Address:",
                Location = new Point(15, 70),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(73, 80, 87)
            };

            pnlReseller.Controls.AddRange(new Control[] { lblResellerLabel, txtResellerSearch, btnSearchReseller, lblReseller, lblAddressLabel });

            // Delivery Address
            txtDeliveryAddress = new TextBox
            {
                Location = new Point(20, 170),
                Width = 730,
                Height = 60,
                Font = new Font("Segoe UI", 10),
                Multiline = true,
                PlaceholderText = "Enter delivery address..."
            };

            // Cart Grid
            dgvCart = new DataGridView
            {
                Location = new Point(20, 245),
                Size = new Size(730, 220),
                AutoGenerateColumns = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = false,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 10),
                ColumnHeadersHeight = 40,
                RowTemplate = { Height = 38 },
                EnableHeadersVisualStyles = false
            };

            dgvCart.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Item", Width = 280, ReadOnly = true },
                new DataGridViewTextBoxColumn { DataPropertyName = "Quantity", HeaderText = "Qty", Width = 80, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }, ReadOnly = false },
                new DataGridViewTextBoxColumn { DataPropertyName = "UnitPrice", HeaderText = "Price", Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Format = "₱#,##0.00" }, ReadOnly = false },
                new DataGridViewTextBoxColumn { DataPropertyName = "Subtotal", HeaderText = "Total", Width = 130, DefaultCellStyle = new DataGridViewCellStyle { Format = "₱#,##0.00", Font = new Font("Segoe UI", 10, FontStyle.Bold) }, ReadOnly = true }
            });

            // Handle cell value changes to recalculate subtotal
            dgvCart.CellEndEdit += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.RowIndex < cart.Count)
                {
                    var item = cart[e.RowIndex];
                    
                    if (e.ColumnIndex == 1) // Quantity column
                    {
                        if (int.TryParse(dgvCart.Rows[e.RowIndex].Cells[1].Value?.ToString(), out int qty) && qty > 0)
                        {
                            var perfume = DataStore.Perfumes.FirstOrDefault(p => p.Id == item.PerfumeId);
                            if (perfume != null && qty > perfume.StockLevel)
                            {
                                MessageBox.Show($"Quantity exceeds available stock ({perfume.StockLevel}).", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                dgvCart.Rows[e.RowIndex].Cells[1].Value = item.Quantity;
                                return;
                            }
                            item.Quantity = qty;
                            item.Subtotal = item.Quantity * item.UnitPrice;
                        }
                        else
                        {
                            MessageBox.Show("Please enter a valid quantity.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            dgvCart.Rows[e.RowIndex].Cells[1].Value = item.Quantity;
                        }
                    }
                    else if (e.ColumnIndex == 2) // UnitPrice column
                    {
                        if (decimal.TryParse(dgvCart.Rows[e.RowIndex].Cells[2].Value?.ToString(), out decimal price) && price > 0)
                        {
                            item.UnitPrice = price;
                            item.Subtotal = item.Quantity * item.UnitPrice;
                        }
                        else
                        {
                            MessageBox.Show("Please enter a valid price.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            dgvCart.Rows[e.RowIndex].Cells[2].Value = item.UnitPrice;
                        }
                    }
                    
                    RefreshCart();
                }
            };

            btnRemoveFromCart = new Button
            {
                Text = "🗑️ Remove Item",
                Location = new Point(20, 475),
                Size = new Size(730, 35),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRemoveFromCart.FlatAppearance.BorderSize = 0;
            btnRemoveFromCart.Click += BtnRemoveFromCart_Click;

            // Delivery Options
            Panel pnlOptions = new Panel
            {
                Location = new Point(20, 525),
                Size = new Size(730, 80),
                BackColor = Color.FromArgb(248, 249, 250)
            };

            Label lblDateLabel = new Label
            {
                Text = "Delivery Date:",
                Location = new Point(15, 10),
                Size = new Size(120, 20),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(73, 80, 87)
            };

            dtpDeliveryDate = new DateTimePicker
            {
                Location = new Point(15, 35),
                Width = 200,
                Height = 30,
                Font = new Font("Segoe UI", 10),
                Format = DateTimePickerFormat.Short
            };

            Label lblNotesLabel = new Label
            {
                Text = "Notes:",
                Location = new Point(235, 10),
                Size = new Size(80, 20),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(73, 80, 87)
            };

            txtNotes = new TextBox
            {
                Location = new Point(235, 35),
                Width = 480,
                Height = 30,
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "Optional delivery notes..."
            };

            pnlOptions.Controls.AddRange(new Control[] { lblDateLabel, dtpDeliveryDate, lblNotesLabel, txtNotes });

            // Total Panel
            Panel pnlTotal = new Panel
            {
                Location = new Point(20, 620),
                Size = new Size(730, 75),
                BackColor = Color.FromArgb(33, 37, 41)
            };

            lblSubtotal = new Label
            {
                Text = "Subtotal: ₱0.00",
                Location = new Point(15, 10),
                Size = new Size(700, 18),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(200, 200, 200),
                TextAlign = ContentAlignment.MiddleRight
            };

            lblDiscount = new Label
            {
                Text = "Reseller Discount (0%): ₱0.00",
                Location = new Point(15, 28),
                Size = new Size(700, 18),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(255, 193, 7),
                TextAlign = ContentAlignment.MiddleRight
            };

            lblTotal = new Label
            {
                Text = "TOTAL: ₱0.00",
                Location = new Point(15, 48),
                Size = new Size(700, 24),
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleRight
            };

            pnlTotal.Controls.AddRange(new Control[] { lblSubtotal, lblDiscount, lblTotal });

            // Action Buttons
            btnConfirmDelivery = new Button
            {
                Text = "✓ CONFIRM DELIVERY",
                Location = new Point(20, 705),
                Size = new Size(350, 45),
                BackColor = Color.FromArgb(25, 135, 84),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnConfirmDelivery.FlatAppearance.BorderSize = 0;
            btnConfirmDelivery.Click += BtnConfirmDelivery_Click;

            btnPrintInvoice = new Button
            {
                Text = "🖨️ PRINT INVOICE",
                Location = new Point(380, 705),
                Size = new Size(240, 45),
                BackColor = Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnPrintInvoice.FlatAppearance.BorderSize = 0;
            btnPrintInvoice.Click += BtnPrintInvoice_Click;

            btnClear = new Button
            {
                Text = "↺",
                Location = new Point(630, 705),
                Size = new Size(120, 45),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.Click += BtnClear_Click;

            pnlCart.Controls.AddRange(new Control[] {
                lblCartTitle, pnlReseller, txtDeliveryAddress,
                dgvCart, btnRemoveFromCart, pnlOptions, pnlTotal,
                btnConfirmDelivery, btnPrintInvoice, btnClear
            });

            this.Controls.AddRange(new Control[] { lblTitle, pnlProducts, pnlCart });
        }

        private void LoadProducts()
        {
            dgvProducts.DataSource = DataStore.Perfumes.Where(p => p.StockLevel > 0).ToList();
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.ToLower();
            var filtered = DataStore.Perfumes.Where(p =>
                p.StockLevel > 0 &&
                (p.Name.ToLower().Contains(searchTerm) ||
                 p.Brand.ToLower().Contains(searchTerm))
            ).ToList();
            dgvProducts.DataSource = filtered;
        }

        private void BtnSearchReseller_Click(object? sender, EventArgs e)
        {
            string search = txtResellerSearch.Text.ToLower();
            selectedReseller = DataStore.Resellers.FirstOrDefault(r =>
                r.IsActive &&
                (r.Name.ToLower().Contains(search) ||
                 r.BusinessName.ToLower().Contains(search))
            );

            if (selectedReseller != null)
            {
                lblReseller.Text = $"✓ {selectedReseller.BusinessName} ({selectedReseller.Name})";
                lblReseller.ForeColor = Color.FromArgb(25, 135, 84);
                lblReseller.BackColor = Color.FromArgb(212, 237, 218);
                
                // Auto-update delivery address from reseller's address on file
                if (!string.IsNullOrWhiteSpace(selectedReseller.Address))
                {
                    txtDeliveryAddress.Text = selectedReseller.Address;
                }
                else
                {
                    txtDeliveryAddress.Text = "No address on file - please enter delivery address";
                }
                
                CalculateTotal();
            }
            else
            {
                lblReseller.Text = "Reseller not found";
                lblReseller.ForeColor = Color.FromArgb(220, 53, 69);
                lblReseller.BackColor = Color.FromArgb(248, 215, 218);
                txtDeliveryAddress.Clear();
            }
        }

        private void BtnAddToCart_Click(object? sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvProducts.SelectedRows[0].DataBoundItem is not Perfume perfume)
                return;

            if (perfume.StockLevel <= 0)
            {
                MessageBox.Show("Product is out of stock.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var existingItem = cart.FirstOrDefault(c => c.PerfumeId == perfume.Id);
            if (existingItem != null)
            {
                if (existingItem.Quantity >= perfume.StockLevel)
                {
                    MessageBox.Show("Cannot add more. Stock limit reached.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                existingItem.Quantity++;
                existingItem.Subtotal = existingItem.Quantity * existingItem.UnitPrice;
            }
            else
            {
                cart.Add(new DeliveryCartItem
                {
                    PerfumeId = perfume.Id,
                    Name = $"{perfume.Name} - {perfume.Brand} ({perfume.Size})",
                    Quantity = 1,
                    UnitPrice = perfume.SellingPrice,
                    Subtotal = perfume.SellingPrice
                });
            }

            RefreshCart();
        }

        private void BtnRemoveFromCart_Click(object? sender, EventArgs e)
        {
            if (dgvCart.SelectedRows.Count == 0) return;

            int index = dgvCart.SelectedRows[0].Index;
            cart.RemoveAt(index);
            RefreshCart();
        }

        private void BtnConfirmDelivery_Click(object? sender, EventArgs e)
        {
            if (selectedReseller == null)
            {
                MessageBox.Show("Please select a reseller.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cart.Count == 0)
            {
                MessageBox.Show("Cart is empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDeliveryAddress.Text))
            {
                MessageBox.Show("Please enter a delivery address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal subtotal = cart.Sum(c => c.Subtotal);
            decimal discountPercent = selectedReseller.DiscountRate;
            decimal discountAmount = subtotal * (discountPercent / 100);
            decimal total = subtotal - discountAmount;

            var sale = new Sale
            {
                Id = DataStore.GetNextId(DataStore.Sales),
                SaleDate = dtpDeliveryDate.Value,
                ResellerId = selectedReseller.Id,
                Subtotal = subtotal,
                Discount = discountAmount,
                Total = total,
                PaymentMethod = "Reseller Delivery",
                UserId = DataStore.CurrentUser?.Id ?? 0
            };

            DataStore.Sales.Add(sale);

            foreach (var item in cart)
            {
                var saleItem = new SaleItem
                {
                    Id = DataStore.GetNextId(DataStore.SaleItems),
                    SaleId = sale.Id,
                    PerfumeId = item.PerfumeId,
                    PerfumeName = item.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Subtotal = item.Subtotal
                };
                DataStore.SaleItems.Add(saleItem);

                var perfume = DataStore.Perfumes.FirstOrDefault(p => p.Id == item.PerfumeId);
                if (perfume != null)
                {
                    perfume.StockLevel -= item.Quantity;
                }
            }

            selectedReseller.TotalPurchases += total;
            DataPersistence.SaveAllData();

            completedSale = sale;
            btnPrintInvoice.Enabled = true;

            MessageBox.Show($"Delivery confirmed!\nTotal: ₱{total:N2}\nInvoice ID: {sale.Id}\n\nYou can now print the invoice.", 
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnPrintInvoice_Click(object? sender, EventArgs e)
        {
            if (completedSale == null || selectedReseller == null)
            {
                MessageBox.Show("No completed delivery to print.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += (s, ev) => PrintInvoice(ev, completedSale, selectedReseller);

            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDoc;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }

        private void PrintInvoice(PrintPageEventArgs e, Sale sale, Reseller reseller)
        {
            Graphics g = e.Graphics!;
            Font titleFont = new Font("Segoe UI", 20, FontStyle.Bold);
            Font headerFont = new Font("Segoe UI", 12, FontStyle.Bold);
            Font normalFont = new Font("Segoe UI", 10);
            Font smallFont = new Font("Segoe UI", 9);
            Brush blackBrush = Brushes.Black;
            Brush grayBrush = Brushes.Gray;

            float yPos = 50;
            float leftMargin = 50;
            float rightMargin = e.PageBounds.Width - 50;

            // Try to load and draw logo
            try
            {
                string logoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scnt_logo.png");
                if (System.IO.File.Exists(logoPath))
                {
                    using (Image logo = Image.FromFile(logoPath))
                    {
                        // Draw logo at top left, scaled to fit
                        float logoWidth = 120;
                        float logoHeight = (logo.Height * logoWidth) / logo.Width;
                        g.DrawImage(logo, leftMargin, yPos, logoWidth, logoHeight);
                        yPos += logoHeight + 10;
                    }
                }
            }
            catch
            {
                // If logo fails to load, continue without it
            }

            // Company Header
            g.DrawString("SCNT PERFUME SYSTEM", titleFont, blackBrush, leftMargin, yPos);
            yPos += 35;
            g.DrawString("Delivery Invoice", headerFont, grayBrush, leftMargin, yPos);
            yPos += 40;

            // Invoice Details
            g.DrawString($"Invoice #: {sale.Id}", normalFont, blackBrush, leftMargin, yPos);
            g.DrawString($"Date: {sale.SaleDate:yyyy-MM-dd}", normalFont, blackBrush, rightMargin - 200, yPos);
            yPos += 25;

            // Reseller Details
            g.DrawString("DELIVER TO:", headerFont, blackBrush, leftMargin, yPos);
            yPos += 25;
            g.DrawString($"Business: {reseller.BusinessName}", normalFont, blackBrush, leftMargin, yPos);
            yPos += 20;
            g.DrawString($"Contact: {reseller.Name}", normalFont, blackBrush, leftMargin, yPos);
            yPos += 20;
            g.DrawString($"Phone: {reseller.Phone}", normalFont, blackBrush, leftMargin, yPos);
            yPos += 20;
            g.DrawString($"Address: {txtDeliveryAddress.Text}", normalFont, blackBrush, leftMargin, yPos);
            yPos += 35;

            // Line separator
            g.DrawLine(Pens.Black, leftMargin, yPos, rightMargin, yPos);
            yPos += 15;

            // Table Header
            g.DrawString("ITEM", headerFont, blackBrush, leftMargin, yPos);
            g.DrawString("QTY", headerFont, blackBrush, rightMargin - 350, yPos);
            g.DrawString("PRICE", headerFont, blackBrush, rightMargin - 250, yPos);
            g.DrawString("TOTAL", headerFont, blackBrush, rightMargin - 120, yPos);
            yPos += 25;
            g.DrawLine(Pens.Gray, leftMargin, yPos, rightMargin, yPos);
            yPos += 15;

            // Items
            var saleItems = DataStore.SaleItems.Where(si => si.SaleId == sale.Id).ToList();
            foreach (var item in saleItems)
            {
                g.DrawString(item.PerfumeName, normalFont, blackBrush, leftMargin, yPos);
                g.DrawString(item.Quantity.ToString(), normalFont, blackBrush, rightMargin - 350, yPos);
                g.DrawString($"₱{item.UnitPrice:N2}", normalFont, blackBrush, rightMargin - 250, yPos);
                g.DrawString($"₱{item.Subtotal:N2}", normalFont, blackBrush, rightMargin - 120, yPos);
                yPos += 22;
            }

            yPos += 10;
            g.DrawLine(Pens.Black, leftMargin, yPos, rightMargin, yPos);
            yPos += 20;

            // Totals
            g.DrawString("Subtotal:", normalFont, blackBrush, rightMargin - 250, yPos);
            g.DrawString($"₱{sale.Subtotal:N2}", normalFont, blackBrush, rightMargin - 120, yPos);
            yPos += 22;

            g.DrawString($"Discount ({reseller.DiscountRate}%):", normalFont, blackBrush, rightMargin - 250, yPos);
            g.DrawString($"-₱{sale.Discount:N2}", normalFont, blackBrush, rightMargin - 120, yPos);
            yPos += 25;

            g.DrawLine(Pens.Black, rightMargin - 260, yPos, rightMargin, yPos);
            yPos += 20;

            g.DrawString("TOTAL:", headerFont, blackBrush, rightMargin - 250, yPos);
            g.DrawString($"₱{sale.Total:N2}", new Font("Segoe UI", 14, FontStyle.Bold), blackBrush, rightMargin - 120, yPos);
            yPos += 50;

            // Notes
            if (!string.IsNullOrWhiteSpace(txtNotes.Text))
            {
                g.DrawString("Notes:", smallFont, grayBrush, leftMargin, yPos);
                yPos += 18;
                g.DrawString(txtNotes.Text, smallFont, blackBrush, leftMargin, yPos);
                yPos += 30;
            }

            // Footer
            yPos = e.PageBounds.Height - 100;
            g.DrawLine(Pens.Gray, leftMargin, yPos, rightMargin, yPos);
            yPos += 15;
            g.DrawString("Thank you for your business!", smallFont, grayBrush, leftMargin, yPos);
            g.DrawString($"Printed: {DateTime.Now:yyyy-MM-dd HH:mm}", smallFont, grayBrush, rightMargin - 200, yPos);
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            cart.Clear();
            selectedReseller = null;
            completedSale = null;
            txtResellerSearch.Clear();
            txtDeliveryAddress.Clear();
            txtNotes.Clear();
            lblReseller.Text = "No reseller selected";
            lblReseller.ForeColor = Color.FromArgb(220, 53, 69);
            lblReseller.BackColor = Color.FromArgb(248, 215, 218);
            dtpDeliveryDate.Value = DateTime.Now;
            btnPrintInvoice.Enabled = false;
            RefreshCart();
            LoadProducts();
        }

        private void RefreshCart()
        {
            dgvCart.DataSource = null;
            dgvCart.DataSource = cart.ToList();
            CalculateTotal();
        }

        private void CalculateTotal()
        {
            decimal subtotal = cart.Sum(c => c.Subtotal);
            decimal discountPercent = selectedReseller?.DiscountRate ?? 0;
            decimal discountAmount = subtotal * (discountPercent / 100);
            decimal total = subtotal - discountAmount;

            lblSubtotal.Text = $"Subtotal: ₱{subtotal:N2}";
            lblDiscount.Text = $"Reseller Discount ({discountPercent}%): -₱{discountAmount:N2}";
            lblTotal.Text = $"TOTAL: ₱{total:N2}";
        }
    }

    public class DeliveryCartItem
    {
        public int PerfumeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }
}
