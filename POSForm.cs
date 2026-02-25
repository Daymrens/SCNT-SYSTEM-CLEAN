using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PerfumeInventory
{
    public class POSForm : Form
    {
        private DataGridView dgvProducts, dgvCart;
        private TextBox txtSearch, txtDiscount, txtCustomerSearch;
        private Label lblSubtotal, lblDiscount, lblTotal, lblCustomer;
        private Button btnAddToCart, btnRemoveFromCart, btnCheckout, btnClear, btnPrintInvoice;
        private ComboBox cmbPaymentMethod;
        private DateTimePicker dtpSaleDate;
        private List<CartItem> cart = new List<CartItem>();
        private Customer? selectedCustomer;
        private Sale? completedSale;

        public POSForm()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void InitializeComponent()
        {
            this.Text = "Point of Sale";
            this.Size = new Size(1200, 880);
            this.BackColor = Color.FromArgb(248, 249, 250);

            Label lblTitle = new Label
            {
                Text = "💰 CHECKOUT",
                Location = new Point(30, 25),
                Size = new Size(300, 35),
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            // Products Panel
            Panel pnlProducts = new Panel
            {
                Location = new Point(30, 80),
                Size = new Size(580, 730),
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
                Width = 540, 
                Height = 35,
                Font = new Font("Segoe UI", 11),
                PlaceholderText = "🔍 Search products by name or brand...",
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(248, 249, 250)
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;
            
            // Underline for search textbox
            Panel searchUnderline = new Panel
            {
                Location = new Point(20, 95),
                Size = new Size(540, 2),
                BackColor = Color.FromArgb(206, 212, 218)
            };

            dgvProducts = new DataGridView
            {
                Location = new Point(20, 110),
                Size = new Size(540, 520),
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
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(240, 240, 240),
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                AllowUserToResizeRows = false
            };

            dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            dgvProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(73, 80, 87);
            dgvProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvProducts.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            dgvProducts.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgvProducts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 244, 253);
            dgvProducts.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dgvProducts.DefaultCellStyle.Padding = new Padding(10, 5, 5, 5);

            dgvProducts.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Product Name", Width = 180 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Brand", HeaderText = "Brand", Width = 120 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Size", HeaderText = "Size", Width = 80 },
                new DataGridViewTextBoxColumn { DataPropertyName = "SellingPrice", HeaderText = "Price", Width = 90, DefaultCellStyle = new DataGridViewCellStyle { Format = "₱#,##0.00" } },
                new DataGridViewTextBoxColumn { DataPropertyName = "StockLevel", HeaderText = "Stock", Width = 70 }
            });

            btnAddToCart = new Button
            {
                Text = "➕ Add to Cart",
                Location = new Point(20, 645),
                Size = new Size(540, 50),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
            btnAddToCart.Click += BtnAddToCart_Click;
            btnAddToCart.MouseEnter += (s, e) => btnAddToCart.BackColor = Color.FromArgb(56, 142, 60);
            btnAddToCart.MouseLeave += (s, e) => btnAddToCart.BackColor = Color.FromArgb(76, 175, 80);

            pnlProducts.Controls.AddRange(new Control[] { lblProductsTitle, txtSearch, searchUnderline, dgvProducts, btnAddToCart });

            // Cart Panel
            Panel pnlCart = new Panel
            {
                Location = new Point(630, 80),
                Size = new Size(540, 730),
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            Label lblCartTitle = new Label
            {
                Text = "Cart",
                Location = new Point(20, 20),
                Size = new Size(200, 30),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            Panel pnlCustomer = new Panel
            {
                Location = new Point(20, 60),
                Size = new Size(500, 45),
                BackColor = Color.FromArgb(248, 249, 250)
            };

            txtCustomerSearch = new TextBox 
            { 
                Location = new Point(10, 10), 
                Width = 300, 
                Height = 30,
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "Search customer...",
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(248, 249, 250)
            };
            
            // Underline for customer search textbox
            Panel customerSearchUnderline = new Panel
            {
                Location = new Point(10, 40),
                Size = new Size(300, 2),
                BackColor = Color.FromArgb(206, 212, 218)
            };
            
            Button btnSearchCustomer = new Button
            {
                Text = "🔍 Find",
                Location = new Point(320, 8),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
            btnSearchCustomer.Click += BtnSearchCustomer_Click;
            btnSearchCustomer.MouseEnter += (s, e) => btnSearchCustomer.BackColor = Color.FromArgb(10, 88, 202);
            btnSearchCustomer.MouseLeave += (s, e) => btnSearchCustomer.BackColor = Color.FromArgb(13, 110, 253);

            lblCustomer = new Label
            {
                Text = "Walk-in",
                Location = new Point(410, 10),
                Size = new Size(85, 28),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(108, 117, 125),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.White
            };

            pnlCustomer.Controls.AddRange(new Control[] { txtCustomerSearch, customerSearchUnderline, btnSearchCustomer, lblCustomer });

            dgvCart = new DataGridView
            {
                Location = new Point(20, 120),
                Size = new Size(500, 240),
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
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(240, 240, 240),
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                AllowUserToResizeRows = false
            };

            dgvCart.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            dgvCart.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(73, 80, 87);
            dgvCart.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvCart.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);

            dgvCart.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 243, 224);
            dgvCart.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dgvCart.DefaultCellStyle.Padding = new Padding(8, 5, 5, 5);

            dgvCart.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Item", Width = 200, ReadOnly = true },
                new DataGridViewTextBoxColumn { DataPropertyName = "Quantity", HeaderText = "Qty", Width = 60, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }, ReadOnly = false },
                new DataGridViewTextBoxColumn { DataPropertyName = "UnitPrice", HeaderText = "Price", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Format = "₱#,##0.00" }, ReadOnly = false },
                new DataGridViewTextBoxColumn { DataPropertyName = "Subtotal", HeaderText = "Total", Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Format = "₱#,##0.00", Font = new Font("Segoe UI", 10, FontStyle.Bold) }, ReadOnly = true }
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
                Text = "🗑️ Remove",
                Location = new Point(20, 370),
                Size = new Size(500, 35),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
            btnRemoveFromCart.Click += BtnRemoveFromCart_Click;
            btnRemoveFromCart.MouseEnter += (s, e) => btnRemoveFromCart.BackColor = Color.FromArgb(187, 45, 59);
            btnRemoveFromCart.MouseLeave += (s, e) => btnRemoveFromCart.BackColor = Color.FromArgb(220, 53, 69);

            Panel pnlOptions = new Panel
            {
                Location = new Point(20, 420),
                Size = new Size(500, 70),
                BackColor = Color.FromArgb(248, 249, 250)
            };

            Label lblDiscountLabel = new Label { Text = "Discount %", Location = new Point(10, 10), Width = 80, Font = new Font("Segoe UI", 9), ForeColor = Color.FromArgb(73, 80, 87) };
            
            txtDiscount = new TextBox 
            { 
                Location = new Point(10, 30), 
                Width = 100, 
                Height = 30, 
                Font = new Font("Segoe UI", 10), 
                Text = "0", 
                BorderStyle = BorderStyle.None, 
                BackColor = Color.FromArgb(248, 249, 250)
            };
            txtDiscount.TextChanged += (s, e) => CalculateTotal();
            
            // Underline for discount textbox
            Panel discountUnderline = new Panel
            {
                Location = new Point(10, 60),
                Size = new Size(100, 2),
                BackColor = Color.FromArgb(206, 212, 218)
            };

            Label lblPaymentLabel = new Label { Text = "Payment", Location = new Point(130, 10), Width = 80, Font = new Font("Segoe UI", 9), ForeColor = Color.FromArgb(73, 80, 87) };
            cmbPaymentMethod = new ComboBox { Location = new Point(130, 30), Width = 150, Height = 30, Font = new Font("Segoe UI", 10), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbPaymentMethod.Items.AddRange(new object[] { "Cash", "Credit Card", "Debit Card", "E-Wallet" });
            cmbPaymentMethod.SelectedIndex = 0;

            Label lblSaleDateLabel = new Label { Text = "Date", Location = new Point(300, 10), Width = 80, Font = new Font("Segoe UI", 9), ForeColor = Color.FromArgb(73, 80, 87) };
            dtpSaleDate = new DateTimePicker 
            { 
                Location = new Point(300, 30), 
                Width = 190,
                Height = 30,
                Font = new Font("Segoe UI", 9),
                Format = DateTimePickerFormat.Short
            };

            pnlOptions.Controls.AddRange(new Control[] { lblDiscountLabel, txtDiscount, discountUnderline, lblPaymentLabel, cmbPaymentMethod, lblSaleDateLabel, dtpSaleDate });

            Panel pnlTotal = new Panel
            {
                Location = new Point(20, 505),
                Size = new Size(500, 75),
                BackColor = Color.FromArgb(33, 37, 41)
            };

            lblSubtotal = new Label { Text = "Subtotal: ₱0.00", Location = new Point(15, 10), Size = new Size(470, 18), Font = new Font("Segoe UI", 9), ForeColor = Color.FromArgb(200, 200, 200), TextAlign = ContentAlignment.MiddleRight };
            lblDiscount = new Label { Text = "Discount: ₱0.00", Location = new Point(15, 28), Size = new Size(470, 18), Font = new Font("Segoe UI", 9), ForeColor = Color.FromArgb(255, 193, 7), TextAlign = ContentAlignment.MiddleRight };
            lblTotal = new Label { Text = "TOTAL: ₱0.00", Location = new Point(15, 48), Size = new Size(470, 24), Font = new Font("Segoe UI", 13, FontStyle.Bold), ForeColor = Color.White, TextAlign = ContentAlignment.MiddleRight };

            pnlTotal.Controls.AddRange(new Control[] { lblSubtotal, lblDiscount, lblTotal });

            btnCheckout = new Button
            {
                Text = "✓ COMPLETE SALE",
                Location = new Point(20, 595),
                Size = new Size(380, 45),
                BackColor = Color.FromArgb(25, 135, 84),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
            btnCheckout.Click += BtnCheckout_Click;
            btnCheckout.MouseEnter += (s, e) => btnCheckout.BackColor = Color.FromArgb(20, 108, 67);
            btnCheckout.MouseLeave += (s, e) => btnCheckout.BackColor = Color.FromArgb(25, 135, 84);

            btnPrintInvoice = new Button
            {
                Text = "🖨️ PRINT",
                Location = new Point(20, 645),
                Size = new Size(380, 45),
                BackColor = Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 },
                Enabled = false
            };
            btnPrintInvoice.Click += BtnPrintInvoice_Click;
            btnPrintInvoice.MouseEnter += (s, e) => { if (btnPrintInvoice.Enabled) btnPrintInvoice.BackColor = Color.FromArgb(10, 88, 202); };
            btnPrintInvoice.MouseLeave += (s, e) => { if (btnPrintInvoice.Enabled) btnPrintInvoice.BackColor = Color.FromArgb(13, 110, 253); };

            btnClear = new Button
            {
                Text = "↺",
                Location = new Point(410, 595),
                Size = new Size(110, 95),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
            btnClear.Click += BtnClear_Click;
            btnClear.MouseEnter += (s, e) => btnClear.BackColor = Color.FromArgb(86, 94, 100);
            btnClear.MouseLeave += (s, e) => btnClear.BackColor = Color.FromArgb(108, 117, 125);

            pnlCart.Controls.AddRange(new Control[] {
                lblCartTitle, pnlCustomer,
                dgvCart, btnRemoveFromCart, pnlOptions, pnlTotal, btnCheckout, btnPrintInvoice, btnClear
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

        private void BtnAddToCart_Click(object? sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvProducts.SelectedRows[0].DataBoundItem is not Perfume perfume)
                return;

            if (perfume == null || perfume.StockLevel <= 0)
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
                cart.Add(new CartItem
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

        private void BtnSearchCustomer_Click(object? sender, EventArgs e)
        {
            string search = txtCustomerSearch.Text.ToLower();
            selectedCustomer = DataStore.Customers.FirstOrDefault(c =>
                c.Name.ToLower().Contains(search) ||
                c.Phone.Contains(search)
            );

            if (selectedCustomer != null)
            {
                lblCustomer.Text = $"✓ {selectedCustomer.Name}";
                lblCustomer.ForeColor = Color.FromArgb(25, 135, 84);
                lblCustomer.BackColor = Color.FromArgb(212, 237, 218);
            }
            else
            {
                lblCustomer.Text = "Not found";
                lblCustomer.ForeColor = Color.FromArgb(220, 53, 69);
                lblCustomer.BackColor = Color.FromArgb(248, 215, 218);
            }
        }

        private void BtnCheckout_Click(object? sender, EventArgs e)
        {
            if (cart.Count == 0)
            {
                MessageBox.Show("Cart is empty.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal subtotal = cart.Sum(c => c.Subtotal);
            decimal discountPercent = decimal.TryParse(txtDiscount.Text, out decimal d) ? d : 0;
            decimal discountAmount = subtotal * (discountPercent / 100);
            decimal total = subtotal - discountAmount;

            var sale = new Sale
            {
                Id = DataStore.GetNextId(DataStore.Sales),
                SaleDate = dtpSaleDate.Value,
                CustomerId = selectedCustomer?.Id,
                Subtotal = subtotal,
                Discount = discountAmount,
                Total = total,
                PaymentMethod = cmbPaymentMethod.Text,
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

                // Deduct stock
                var perfume = DataStore.Perfumes.FirstOrDefault(p => p.Id == item.PerfumeId);
                if (perfume != null)
                {
                    perfume.StockLevel -= item.Quantity;
                }
            }

            // Add loyalty points - 1 point per transaction
            if (selectedCustomer != null)
            {
                // Find the customer in DataStore and update
                var customer = DataStore.Customers.FirstOrDefault(c => c.Id == selectedCustomer.Id);
                if (customer != null)
                {
                    customer.LoyaltyPoints += 1;
                }
            }

            DataPersistence.SaveAllData();

            completedSale = sale;
            btnPrintInvoice.Enabled = true;

            MessageBox.Show($"Sale completed!\nTotal: ₱{total:N2}\nReceipt ID: {sale.Id}\n\nYou can now print the invoice.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadProducts();
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            cart.Clear();
            selectedCustomer = null;
            completedSale = null;
            txtCustomerSearch.Clear();
            lblCustomer.Text = "Walk-in";
            lblCustomer.ForeColor = Color.FromArgb(108, 117, 125);
            lblCustomer.BackColor = Color.White;
            txtDiscount.Text = "0";
            dtpSaleDate.Value = DateTime.Now;
            btnPrintInvoice.Enabled = false;
            RefreshCart();
        }

        private void BtnPrintInvoice_Click(object? sender, EventArgs e)
        {
            if (completedSale == null)
            {
                MessageBox.Show("No completed sale to print.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            System.Drawing.Printing.PrintDocument printDoc = new System.Drawing.Printing.PrintDocument();
            printDoc.PrintPage += (s, ev) => PrintInvoice(ev, completedSale);

            System.Windows.Forms.PrintDialog printDialog = new System.Windows.Forms.PrintDialog();
            printDialog.Document = printDoc;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }

        private void PrintInvoice(System.Drawing.Printing.PrintPageEventArgs e, Sale sale)
        {
            var g = e.Graphics;
            if (g == null) return;

            int y = 50;
            Font titleFont = new Font("Segoe UI", 18, FontStyle.Bold);
            Font headerFont = new Font("Segoe UI", 12, FontStyle.Bold);
            Font normalFont = new Font("Segoe UI", 10);
            Font smallFont = new Font("Segoe UI", 9);

            // Header
            g.DrawString("SCNT VAULT", titleFont, Brushes.Black, 250, y);
            y += 40;
            g.DrawString("Sales Invoice", headerFont, Brushes.Black, 320, y);
            y += 40;

            // Invoice Details
            g.DrawString($"Invoice #: {sale.Id}", normalFont, Brushes.Black, 50, y);
            g.DrawString($"Date: {sale.SaleDate:yyyy-MM-dd HH:mm}", normalFont, Brushes.Black, 550, y);
            y += 25;

            string customerName = "Walk-in";
            if (sale.CustomerId.HasValue)
            {
                var customer = DataStore.Customers.FirstOrDefault(c => c.Id == sale.CustomerId);
                if (customer != null)
                    customerName = customer.Name;
            }
            g.DrawString($"Customer: {customerName}", normalFont, Brushes.Black, 50, y);
            g.DrawString($"Payment: {sale.PaymentMethod}", normalFont, Brushes.Black, 550, y);
            y += 40;

            // Table Header
            g.DrawLine(Pens.Black, 50, y, 750, y);
            y += 10;
            g.DrawString("ITEM", headerFont, Brushes.Black, 50, y);
            g.DrawString("QTY", headerFont, Brushes.Black, 450, y);
            g.DrawString("PRICE", headerFont, Brushes.Black, 530, y);
            g.DrawString("TOTAL", headerFont, Brushes.Black, 650, y);
            y += 25;
            g.DrawLine(Pens.Black, 50, y, 750, y);
            y += 15;

            // Items
            var items = DataStore.SaleItems.Where(si => si.SaleId == sale.Id).ToList();
            foreach (var item in items)
            {
                g.DrawString(item.PerfumeName, normalFont, Brushes.Black, 50, y);
                g.DrawString(item.Quantity.ToString(), normalFont, Brushes.Black, 450, y);
                g.DrawString($"₱{item.UnitPrice:N2}", normalFont, Brushes.Black, 530, y);
                g.DrawString($"₱{item.Subtotal:N2}", normalFont, Brushes.Black, 650, y);
                y += 25;
            }

            y += 10;
            g.DrawLine(Pens.Black, 50, y, 750, y);
            y += 20;

            // Totals
            g.DrawString($"Subtotal:", normalFont, Brushes.Black, 550, y);
            g.DrawString($"₱{sale.Subtotal:N2}", normalFont, Brushes.Black, 650, y);
            y += 25;

            if (sale.Discount > 0)
            {
                g.DrawString($"Discount:", normalFont, Brushes.Black, 550, y);
                g.DrawString($"-₱{sale.Discount:N2}", normalFont, Brushes.Black, 650, y);
                y += 25;
            }

            g.DrawLine(Pens.Black, 530, y, 750, y);
            y += 10;
            g.DrawString($"TOTAL:", headerFont, Brushes.Black, 550, y);
            g.DrawString($"₱{sale.Total:N2}", headerFont, Brushes.Black, 650, y);
            y += 50;

            // Footer
            g.DrawString("Thank you for your purchase!", smallFont, Brushes.Gray, 300, y);
            y += 20;
            g.DrawString($"Cashier: {DataStore.CurrentUser?.FullName ?? "Unknown"}", smallFont, Brushes.Gray, 320, y);
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
            decimal discountPercent = decimal.TryParse(txtDiscount.Text, out decimal d) ? d : 0;
            decimal discountAmount = subtotal * (discountPercent / 100);
            decimal total = subtotal - discountAmount;

            lblSubtotal.Text = $"Subtotal: ₱{subtotal:N2}";
            lblDiscount.Text = $"Discount: -₱{discountAmount:N2}";
            lblTotal.Text = $"TOTAL: ₱{total:N2}";
        }
    }

    public class CartItem
    {
        public int PerfumeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }
}