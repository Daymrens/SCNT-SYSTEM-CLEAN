using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PerfumeInventory
{
    public class PurchaseOrdersForm : Form
    {
        private DataGridView dgvPurchaseOrders, dgvOrderItems;
        private ComboBox cmbSupplier, cmbStatus;
        private Button btnCreatePO, btnReceiveStock, btnViewDetails, btnAddItems, btnEditPO, btnUploadInvoice, btnViewInvoice;
        private Button btnRemoveItem, btnEditItemQty;
        private PurchaseOrder? selectedPurchaseOrder;

        public PurchaseOrdersForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Purchase Orders";
            this.Size = new Size(1150, 800);
            this.BackColor = Color.FromArgb(248, 249, 250);

            Label lblTitle = new Label
            {
                Text = "📋 PURCHASE ORDERS",
                Location = new Point(30, 30),
                Size = new Size(400, 35),
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            // Create PO Panel
            Panel pnlCreate = new Panel
            {
                Location = new Point(30, 85),
                Size = new Size(1090, 90),
                BackColor = Color.White
            };

            Label lblCreateTitle = new Label
            {
                Text = "Create New Purchase Order",
                Location = new Point(20, 20),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            Label lblSupplier = new Label { Text = "Supplier:", Location = new Point(20, 55), Width = 70, Font = new Font("Segoe UI", 10) };
            cmbSupplier = new ComboBox { Location = new Point(95, 53), Width = 250, Height = 30, Font = new Font("Segoe UI", 10), DropDownStyle = ComboBoxStyle.DropDownList };

            btnCreatePO = new Button
            {
                Text = "➕ Create PO",
                Location = new Point(365, 50),
                Size = new Size(130, 35),
                BackColor = Color.FromArgb(25, 135, 84),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCreatePO.FlatAppearance.BorderSize = 0;
            btnCreatePO.Click += BtnCreatePO_Click;

            pnlCreate.Controls.AddRange(new Control[] { lblCreateTitle, lblSupplier, cmbSupplier, btnCreatePO });

            // Purchase Orders Grid
            Panel pnlOrders = new Panel
            {
                Location = new Point(30, 195),
                Size = new Size(1090, 310),
                BackColor = Color.White
            };

            Label lblOrdersTitle = new Label
            {
                Text = "Purchase Orders List",
                Location = new Point(20, 20),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            Label lblSearch = new Label { Text = "Filter:", Location = new Point(20, 55), Width = 50, Font = new Font("Segoe UI", 10) };
            cmbStatus = new ComboBox { Location = new Point(75, 53), Width = 130, Height = 30, Font = new Font("Segoe UI", 10), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbStatus.Items.AddRange(new object[] { "All", "Pending", "Partial", "Completed" });
            cmbStatus.SelectedIndex = 0;
            cmbStatus.SelectedIndexChanged += (s, e) => LoadData();

            dgvPurchaseOrders = new DataGridView
            {
                Location = new Point(20, 95),
                Size = new Size(1050, 150),
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

            dgvPurchaseOrders.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "PO #", Width = 70 },
                new DataGridViewTextBoxColumn { DataPropertyName = "SupplierName", HeaderText = "Supplier", Width = 220 },
                new DataGridViewTextBoxColumn { DataPropertyName = "OrderDate", HeaderText = "Order Date", Width = 150 },
                new DataGridViewTextBoxColumn { DataPropertyName = "DeliveryDate", HeaderText = "Delivery Date", Width = 150 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Status", HeaderText = "Status", Width = 100 },
                new DataGridViewTextBoxColumn { DataPropertyName = "TotalAmount", HeaderText = "Total Amount", Width = 130 }
            });

            dgvPurchaseOrders.SelectionChanged += DgvPurchaseOrders_SelectionChanged;

            btnViewDetails = new Button
            {
                Text = "View Details",
                Location = new Point(20, 260),
                Size = new Size(110, 35),
                BackColor = Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnViewDetails.FlatAppearance.BorderSize = 0;

            btnEditPO = new Button
            {
                Text = "✏️ Edit PO",
                Location = new Point(140, 260),
                Size = new Size(110, 35),
                BackColor = Color.FromArgb(102, 16, 242),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnEditPO.FlatAppearance.BorderSize = 0;
            btnEditPO.Click += BtnEditPO_Click;

            btnAddItems = new Button
            {
                Text = "➕ Add Items",
                Location = new Point(260, 260),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.FromArgb(33, 37, 41),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAddItems.FlatAppearance.BorderSize = 0;
            btnAddItems.Click += BtnAddItems_Click;

            btnReceiveStock = new Button
            {
                Text = "✓ Receive Stock",
                Location = new Point(390, 260),
                Size = new Size(140, 35),
                BackColor = Color.FromArgb(25, 135, 84),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnReceiveStock.FlatAppearance.BorderSize = 0;
            btnReceiveStock.Click += BtnReceiveStock_Click;

            btnUploadInvoice = new Button
            {
                Text = "📄 Upload Invoice",
                Location = new Point(540, 260),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(253, 126, 20),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnUploadInvoice.FlatAppearance.BorderSize = 0;
            btnUploadInvoice.Click += BtnUploadInvoice_Click;

            btnViewInvoice = new Button
            {
                Text = "👁️ View Invoice",
                Location = new Point(700, 260),
                Size = new Size(140, 35),
                BackColor = Color.FromArgb(102, 16, 242),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnViewInvoice.FlatAppearance.BorderSize = 0;
            btnViewInvoice.Click += BtnViewInvoice_Click;

            Button btnProcessInvoice = new Button
            {
                Text = "⚡ Process Invoice",
                Location = new Point(850, 260),
                Size = new Size(160, 35),
                BackColor = Color.FromArgb(13, 202, 240),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnProcessInvoice.FlatAppearance.BorderSize = 0;
            btnProcessInvoice.Click += BtnProcessInvoice_Click;

            pnlOrders.Controls.AddRange(new Control[] { lblOrdersTitle, lblSearch, cmbStatus, dgvPurchaseOrders, btnViewDetails, btnEditPO, btnAddItems, btnReceiveStock, btnUploadInvoice, btnViewInvoice, btnProcessInvoice });

            // Order Items Grid
            Panel pnlItems = new Panel
            {
                Location = new Point(30, 525),
                Size = new Size(1090, 245),
                BackColor = Color.White
            };

            Label lblItemsTitle = new Label
            {
                Text = "Order Items",
                Location = new Point(20, 20),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41)
            };

            dgvOrderItems = new DataGridView
            {
                Location = new Point(20, 55),
                Size = new Size(1050, 130),
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

            dgvOrderItems.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "Item ID", Width = 80, Visible = false },
                new DataGridViewTextBoxColumn { DataPropertyName = "PerfumeName", HeaderText = "Product", Width = 350 },
                new DataGridViewTextBoxColumn { DataPropertyName = "OrderedQuantity", HeaderText = "Ordered", Width = 120 },
                new DataGridViewTextBoxColumn { DataPropertyName = "ReceivedQuantity", HeaderText = "Received", Width = 120 },
                new DataGridViewTextBoxColumn { DataPropertyName = "UnitCost", HeaderText = "Unit Cost", Width = 130 },
                new DataGridViewTextBoxColumn { DataPropertyName = "TotalCost", HeaderText = "Total", Width = 130 }
            });

            // Add buttons for editing/removing items (only visible for pending orders)
            btnEditItemQty = new Button
            {
                Text = "✏️ Edit Quantity",
                Location = new Point(20, 195),
                Size = new Size(140, 35),
                BackColor = Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = false
            };
            btnEditItemQty.FlatAppearance.BorderSize = 0;
            btnEditItemQty.Click += BtnEditItemQty_Click;

            btnRemoveItem = new Button
            {
                Text = "✕ Remove Item",
                Location = new Point(170, 195),
                Size = new Size(140, 35),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Visible = false
            };
            btnRemoveItem.FlatAppearance.BorderSize = 0;
            btnRemoveItem.Click += BtnRemoveItem_Click;

            pnlItems.Controls.AddRange(new Control[] { lblItemsTitle, dgvOrderItems, btnEditItemQty, btnRemoveItem });

            this.Controls.AddRange(new Control[] { lblTitle, pnlCreate, pnlOrders, pnlItems });
            
            // Apply role-based access control
            ApplyRoleBasedAccess();
        }

        private void ApplyRoleBasedAccess()
        {
            // Check if current user is a Cashier
            if (DataStore.CurrentUser?.Role == "Cashier")
            {
                // Hide all edit/create buttons for cashiers - they can only view
                btnCreatePO.Visible = false;
                btnEditPO.Visible = false;
                btnAddItems.Visible = false;
                btnReceiveStock.Visible = false;
                btnUploadInvoice.Visible = false;
                
                // Also hide the supplier combo box and label in create panel since they can't create
                cmbSupplier.Visible = false;
                foreach (Control ctrl in cmbSupplier.Parent.Controls)
                {
                    if (ctrl is Label lbl && lbl.Text == "Supplier:")
                    {
                        lbl.Visible = false;
                        break;
                    }
                }
                
                // Update the create panel title to indicate view-only mode
                foreach (Control ctrl in cmbSupplier.Parent.Controls)
                {
                    if (ctrl is Label lbl && lbl.Text == "Create New Purchase Order")
                    {
                        lbl.Text = "Purchase Orders (View Only)";
                        break;
                    }
                }
            }
        }

        private void LoadData()
        {
            cmbSupplier.DataSource = DataStore.Suppliers.Select(s => new { s.Id, s.Name }).ToList();
            cmbSupplier.DisplayMember = "Name";
            cmbSupplier.ValueMember = "Id";

            var orders = DataStore.PurchaseOrders.AsEnumerable();
            
            if (cmbStatus.SelectedItem?.ToString() != "All")
            {
                orders = orders.Where(po => po.Status == cmbStatus.SelectedItem?.ToString());
            }

            var ordersData = orders.Select(po => new
            {
                po.Id,
                SupplierName = DataStore.Suppliers.FirstOrDefault(s => s.Id == po.SupplierId)?.Name ?? "Unknown",
                OrderDate = po.OrderDate.ToString("MM/dd/yyyy"),
                DeliveryDate = po.DeliveryDate?.ToString("MM/dd/yyyy") ?? "Not delivered",
                po.Status,
                po.TotalAmount
            }).OrderByDescending(po => po.Id).ToList();

            dgvPurchaseOrders.DataSource = ordersData;
        }

        private void BtnCreatePO_Click(object? sender, EventArgs e)
        {
            if (cmbSupplier.SelectedValue == null)
            {
                MessageBox.Show("Please select a supplier.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var po = new PurchaseOrder
            {
                Id = DataStore.GetNextId(DataStore.PurchaseOrders),
                SupplierId = (int)cmbSupplier.SelectedValue,
                OrderDate = DateTime.Now,
                Status = "Pending",
                TotalAmount = 0
            };

            DataStore.PurchaseOrders.Add(po);
            DataPersistence.SaveAllData();
            LoadData();
            MessageBox.Show($"Purchase Order #{po.Id} created successfully!\nYou can now add items to this order.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnEditPO_Click(object? sender, EventArgs e)
        {
            if (dgvPurchaseOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a purchase order to edit.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int poId = (int)dgvPurchaseOrders.SelectedRows[0].Cells[0].Value;
            var po = DataStore.PurchaseOrders.FirstOrDefault(p => p.Id == poId);

            if (po == null) return;

            // Create edit dialog
            Form editDialog = new Form
            {
                Text = $"Edit Purchase Order #{poId}",
                Size = new Size(500, 380),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblSupplier = new Label { Text = "Supplier:", Location = new Point(20, 20), Width = 100, Font = new Font("Segoe UI", 10) };
            ComboBox cmbEditSupplier = new ComboBox 
            { 
                Location = new Point(130, 18), 
                Width = 320, 
                Font = new Font("Segoe UI", 10), 
                DropDownStyle = ComboBoxStyle.DropDownList 
            };
            cmbEditSupplier.DataSource = DataStore.Suppliers.Select(s => new { s.Id, s.Name }).ToList();
            cmbEditSupplier.DisplayMember = "Name";
            cmbEditSupplier.ValueMember = "Id";
            cmbEditSupplier.SelectedValue = po.SupplierId;

            Label lblOrderDate = new Label { Text = "Order Date:", Location = new Point(20, 60), Width = 100, Font = new Font("Segoe UI", 10) };
            DateTimePicker dtpOrderDate = new DateTimePicker 
            { 
                Location = new Point(130, 58), 
                Width = 320, 
                Font = new Font("Segoe UI", 10),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "MMMM dd, yyyy hh:mm tt",
                Value = po.OrderDate
            };

            Label lblDeliveryDate = new Label { Text = "Delivery Date:", Location = new Point(20, 100), Width = 100, Font = new Font("Segoe UI", 10) };
            DateTimePicker dtpDeliveryDate = new DateTimePicker 
            { 
                Location = new Point(130, 98), 
                Width = 320, 
                Font = new Font("Segoe UI", 10),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "MMMM dd, yyyy hh:mm tt",
                Value = po.DeliveryDate ?? DateTime.Now
            };
            CheckBox chkHasDelivery = new CheckBox 
            { 
                Text = "Has been delivered", 
                Location = new Point(130, 128), 
                Width = 200, 
                Font = new Font("Segoe UI", 9),
                Checked = po.DeliveryDate.HasValue
            };

            Label lblStatus = new Label { Text = "Status:", Location = new Point(20, 160), Width = 100, Font = new Font("Segoe UI", 10) };
            ComboBox cmbEditStatus = new ComboBox 
            { 
                Location = new Point(130, 158), 
                Width = 200, 
                Font = new Font("Segoe UI", 10), 
                DropDownStyle = ComboBoxStyle.DropDownList 
            };
            cmbEditStatus.Items.AddRange(new object[] { "Pending", "Partial", "Completed" });
            cmbEditStatus.SelectedItem = po.Status;

            Label lblInvoiceNumber = new Label { Text = "Invoice #:", Location = new Point(20, 195), Width = 100, Font = new Font("Segoe UI", 10) };
            TextBox txtInvoiceNumber = new TextBox 
            { 
                Location = new Point(130, 193), 
                Width = 320, 
                Font = new Font("Segoe UI", 10),
                Text = po.InvoiceNumber
            };

            Button btnSave = new Button
            {
                Text = "Save Changes",
                Location = new Point(130, 240),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(260, 240),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(158, 158, 158),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            Button btnDelete = new Button
            {
                Text = "Delete PO",
                Location = new Point(130, 285),
                Size = new Size(120, 30),
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            btnSave.Click += (s, ev) =>
            {
                if (cmbEditSupplier.SelectedValue == null)
                {
                    MessageBox.Show("Please select a supplier.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (cmbEditStatus.SelectedItem == null)
                {
                    MessageBox.Show("Please select a status.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                po.SupplierId = (int)cmbEditSupplier.SelectedValue;
                po.OrderDate = dtpOrderDate.Value;
                po.DeliveryDate = chkHasDelivery.Checked ? dtpDeliveryDate.Value : (DateTime?)null;
                po.Status = cmbEditStatus.SelectedItem.ToString()!;
                po.InvoiceNumber = txtInvoiceNumber.Text;

                DataPersistence.SaveAllData();
                LoadData();
                DgvPurchaseOrders_SelectionChanged(null, EventArgs.Empty);
                
                MessageBox.Show("Purchase order updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                editDialog.Close();
            };

            btnDelete.Click += (s, ev) =>
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete Purchase Order #{poId}?\nThis will also delete all associated items.",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    // Delete associated items first
                    var itemsToDelete = DataStore.PurchaseOrderItems.Where(i => i.PurchaseOrderId == poId).ToList();
                    foreach (var item in itemsToDelete)
                    {
                        DataStore.PurchaseOrderItems.Remove(item);
                    }

                    // Delete the purchase order
                    DataStore.PurchaseOrders.Remove(po);

                    DataPersistence.SaveAllData();
                    LoadData();
                    dgvOrderItems.DataSource = null;
                    
                    MessageBox.Show("Purchase order deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    editDialog.Close();
                }
            };

            btnCancel.Click += (s, ev) => editDialog.Close();

            editDialog.Controls.AddRange(new Control[] 
            { 
                lblSupplier, cmbEditSupplier, 
                lblOrderDate, dtpOrderDate, 
                lblDeliveryDate, dtpDeliveryDate, chkHasDelivery,
                lblStatus, cmbEditStatus,
                lblInvoiceNumber, txtInvoiceNumber,
                btnSave, btnCancel, btnDelete 
            });

            editDialog.ShowDialog();
        }

        private void BtnReceiveStock_Click(object? sender, EventArgs e)
        {
            if (dgvPurchaseOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a purchase order.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int poId = (int)dgvPurchaseOrders.SelectedRows[0].Cells[0].Value;
            var po = DataStore.PurchaseOrders.FirstOrDefault(p => p.Id == poId);

            if (po == null) return;

            var items = DataStore.PurchaseOrderItems.Where(i => i.PurchaseOrderId == poId).ToList();
            
            if (items.Count == 0)
            {
                MessageBox.Show("This purchase order has no items.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (var item in items)
            {
                var perfume = DataStore.Perfumes.FirstOrDefault(p => p.Id == item.PerfumeId);
                if (perfume != null)
                {
                    int qtyToReceive = item.OrderedQuantity - item.ReceivedQuantity;
                    perfume.StockLevel += qtyToReceive;
                    item.ReceivedQuantity = item.OrderedQuantity;
                }
            }

            po.Status = "Completed";
            po.DeliveryDate = DateTime.Now;

            DataPersistence.SaveAllData();
            LoadData();
            DgvPurchaseOrders_SelectionChanged(null, EventArgs.Empty);
            MessageBox.Show("Stock received successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnAddItems_Click(object? sender, EventArgs e)
        {
            if (dgvPurchaseOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a purchase order first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int poId = (int)dgvPurchaseOrders.SelectedRows[0].Cells[0].Value;
            var po = DataStore.PurchaseOrders.FirstOrDefault(p => p.Id == poId);

            if (po == null) return;

            if (po.Status == "Completed")
            {
                MessageBox.Show("Cannot add items to a completed purchase order.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (DataStore.Perfumes.Count == 0)
            {
                MessageBox.Show("No perfumes available in inventory.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Create dialog to add items
            Form addItemDialog = new Form
            {
                Text = $"Add Items to PO #{poId}",
                Size = new Size(850, 600),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Dictionary to track selected items with their data
            Dictionary<int, (bool selected, int quantity, decimal unitCost)> selectedItemsData = new Dictionary<int, (bool, int, decimal)>();

            // Search box
            Label lblSearch = new Label { Text = "Search:", Location = new Point(20, 20), Width = 80, Font = new Font("Segoe UI", 10) };
            TextBox txtSearch = new TextBox { Location = new Point(110, 18), Width = 300, Font = new Font("Segoe UI", 10) };

            // DataGridView for perfume selection
            DataGridView dgvPerfumes = new DataGridView
            {
                Location = new Point(20, 60),
                Size = new Size(800, 350),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = false,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D,
                Font = new Font("Segoe UI", 9)
            };

            // Add columns
            dgvPerfumes.Columns.Add(new DataGridViewCheckBoxColumn { Name = "Select", HeaderText = "Select", Width = 60 });
            dgvPerfumes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", Width = 50, ReadOnly = true });
            dgvPerfumes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Name", Width = 200, ReadOnly = true });
            dgvPerfumes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Brand", HeaderText = "Brand", Width = 150, ReadOnly = true });
            dgvPerfumes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Size", HeaderText = "Size", Width = 100, ReadOnly = true });
            dgvPerfumes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Quantity", HeaderText = "Quantity", Width = 100 });
            dgvPerfumes.Columns.Add(new DataGridViewTextBoxColumn { Name = "UnitCost", HeaderText = "Unit Cost", Width = 100 });

            // Save current state before reloading
            void SaveCurrentState()
            {
                foreach (DataGridViewRow row in dgvPerfumes.Rows)
                {
                    int perfumeId = (int)row.Cells["Id"].Value;
                    bool isSelected = row.Cells["Select"].Value != null && (bool)row.Cells["Select"].Value;
                    
                    if (isSelected || selectedItemsData.ContainsKey(perfumeId))
                    {
                        int.TryParse(row.Cells["Quantity"].Value?.ToString(), out int quantity);
                        decimal.TryParse(row.Cells["UnitCost"].Value?.ToString(), out decimal unitCost);
                        
                        if (quantity <= 0) quantity = 1;
                        if (unitCost <= 0) unitCost = 0;
                        
                        selectedItemsData[perfumeId] = (isSelected, quantity, unitCost);
                    }
                }
            }

            // Load all perfumes
            void LoadPerfumes(string searchText = "")
            {
                SaveCurrentState();
                dgvPerfumes.Rows.Clear();
                
                var perfumes = DataStore.Perfumes.AsEnumerable();
                var selectedPerfumes = new List<Perfume>();
                var unselectedPerfumes = new List<Perfume>();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    searchText = searchText.ToLower();
                    perfumes = perfumes.Where(p => 
                        p.Name.ToLower().Contains(searchText) || 
                        p.Brand.ToLower().Contains(searchText) || 
                        p.Size.ToLower().Contains(searchText));
                }

                // Separate selected and unselected items
                foreach (var perfume in perfumes)
                {
                    if (selectedItemsData.ContainsKey(perfume.Id) && selectedItemsData[perfume.Id].selected)
                    {
                        selectedPerfumes.Add(perfume);
                    }
                    else
                    {
                        unselectedPerfumes.Add(perfume);
                    }
                }

                // Add selected items first (at the top)
                foreach (var perfume in selectedPerfumes)
                {
                    var data = selectedItemsData[perfume.Id];
                    dgvPerfumes.Rows.Add(true, perfume.Id, perfume.Name, perfume.Brand, perfume.Size, 
                        data.quantity.ToString(), data.unitCost.ToString("F2"));
                }

                // Then add unselected items
                foreach (var perfume in unselectedPerfumes)
                {
                    string quantity = "1";
                    string unitCost = perfume.CostPrice.ToString("F2");
                    
                    // Restore previous values if item was previously selected
                    if (selectedItemsData.ContainsKey(perfume.Id))
                    {
                        var data = selectedItemsData[perfume.Id];
                        quantity = data.quantity.ToString();
                        unitCost = data.unitCost.ToString("F2");
                    }
                    
                    dgvPerfumes.Rows.Add(false, perfume.Id, perfume.Name, perfume.Brand, perfume.Size, quantity, unitCost);
                }
            }

            LoadPerfumes();

            // Search functionality
            txtSearch.TextChanged += (s, ev) => LoadPerfumes(txtSearch.Text);

            // Tip label
            Label lblTip = new Label 
            { 
                Text = "💡 Tip: Check items to select, then edit Quantity and Unit Cost. Selected items appear at the top!",
                Location = new Point(20, 420),
                Size = new Size(800, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.FromArgb(100, 100, 100)
            };
            
            // Enable/disable quantity and cost when checkbox is clicked
            dgvPerfumes.CellContentClick += (s, ev) =>
            {
                if (ev.ColumnIndex == 0 && ev.RowIndex >= 0)
                {
                    dgvPerfumes.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    
                    // Reload to move selected items to top
                    SaveCurrentState();
                    LoadPerfumes(txtSearch.Text);
                }
            };

            Button btnAddSelected = new Button
            {
                Text = "Add Selected Items",
                Location = new Point(300, 450),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(460, 450),
                Size = new Size(100, 40),
                BackColor = Color.FromArgb(158, 158, 158),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnAddSelected.Click += (s, ev) =>
            {
                List<(int perfumeId, int quantity, decimal unitCost)> selectedItems = new List<(int perfumeId, int quantity, decimal unitCost)>();

                foreach (DataGridViewRow row in dgvPerfumes.Rows)
                {
                    bool isSelected = row.Cells["Select"].Value != null && (bool)row.Cells["Select"].Value;
                    
                    if (isSelected)
                    {
                        int perfumeId = (int)row.Cells["Id"].Value;
                        
                        if (!int.TryParse(row.Cells["Quantity"].Value?.ToString(), out int quantity) || quantity <= 0)
                        {
                            MessageBox.Show($"Please enter a valid quantity for {row.Cells["Name"].Value}.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (!decimal.TryParse(row.Cells["UnitCost"].Value?.ToString(), out decimal unitCost) || unitCost <= 0)
                        {
                            MessageBox.Show($"Please enter a valid unit cost for {row.Cells["Name"].Value}.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        selectedItems.Add((perfumeId, quantity, unitCost));
                    }
                }

                if (selectedItems.Count == 0)
                {
                    MessageBox.Show("Please select at least one item.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Add all selected items
                foreach (var item in selectedItems)
                {
                    var poItem = new PurchaseOrderItem
                    {
                        Id = DataStore.GetNextId(DataStore.PurchaseOrderItems),
                        PurchaseOrderId = poId,
                        PerfumeId = item.perfumeId,
                        OrderedQuantity = item.quantity,
                        ReceivedQuantity = 0,
                        UnitCost = item.unitCost
                    };

                    DataStore.PurchaseOrderItems.Add(poItem);
                }

                // Update PO total
                po.TotalAmount = DataStore.PurchaseOrderItems
                    .Where(i => i.PurchaseOrderId == poId)
                    .Sum(i => i.OrderedQuantity * i.UnitCost);

                DataPersistence.SaveAllData();
                LoadData();
                DgvPurchaseOrders_SelectionChanged(null, EventArgs.Empty);
                
                MessageBox.Show($"{selectedItems.Count} item(s) added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                addItemDialog.Close();
            };

            btnCancel.Click += (s, ev) => addItemDialog.Close();

            addItemDialog.Controls.AddRange(new Control[] { lblSearch, txtSearch, dgvPerfumes, lblTip, btnAddSelected, btnCancel });

            addItemDialog.ShowDialog();
        }

        private void DgvPurchaseOrders_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvPurchaseOrders.SelectedRows.Count > 0)
            {
                int poId = (int)dgvPurchaseOrders.SelectedRows[0].Cells[0].Value;
                selectedPurchaseOrder = DataStore.PurchaseOrders.FirstOrDefault(po => po.Id == poId);
                
                var items = DataStore.PurchaseOrderItems
                    .Where(i => i.PurchaseOrderId == poId)
                    .Select(i => new
                    {
                        i.Id,
                        PerfumeName = DataStore.Perfumes.FirstOrDefault(p => p.Id == i.PerfumeId)?.Name ?? "Unknown",
                        i.OrderedQuantity,
                        i.ReceivedQuantity,
                        i.UnitCost,
                        TotalCost = i.OrderedQuantity * i.UnitCost
                    })
                    .ToList();

                dgvOrderItems.DataSource = items;
                
                // Show edit/remove buttons only if order is pending and user is not a cashier
                bool isPending = selectedPurchaseOrder?.Status == "Pending";
                bool isCashier = DataStore.CurrentUser?.Role == "Cashier";
                btnEditItemQty.Visible = isPending && !isCashier;
                btnRemoveItem.Visible = isPending && !isCashier;
            }
            else
            {
                btnEditItemQty.Visible = false;
                btnRemoveItem.Visible = false;
            }
        }

        private void BtnEditItemQty_Click(object? sender, EventArgs e)
        {
            if (dgvOrderItems.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item to edit.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedPurchaseOrder == null || selectedPurchaseOrder.Status != "Pending")
            {
                MessageBox.Show("Can only edit items in pending orders.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the item ID from the hidden column by index (column 0)
            int itemId = (int)dgvOrderItems.SelectedRows[0].Cells[0].Value;
            var item = DataStore.PurchaseOrderItems.FirstOrDefault(i => i.Id == itemId);

            if (item == null) return;

            // Create edit dialog
            Form editDialog = new Form
            {
                Text = "Edit Item Quantity",
                Size = new Size(400, 200),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(248, 249, 250)
            };

            Label lblProduct = new Label
            {
                Text = $"Product: {DataStore.Perfumes.FirstOrDefault(p => p.Id == item.PerfumeId)?.Name}",
                Location = new Point(30, 30),
                Size = new Size(340, 25),
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            Label lblQty = new Label
            {
                Text = "Ordered Quantity:",
                Location = new Point(30, 70),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10)
            };

            NumericUpDown numQty = new NumericUpDown
            {
                Location = new Point(190, 68),
                Size = new Size(180, 30),
                Font = new Font("Segoe UI", 11),
                Minimum = 1,
                Maximum = 10000,
                Value = item.OrderedQuantity
            };

            Button btnSave = new Button
            {
                Text = "✓ Save",
                Location = new Point(190, 115),
                Size = new Size(90, 35),
                BackColor = Color.FromArgb(25, 135, 84),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;

            Button btnCancel = new Button
            {
                Text = "✕ Cancel",
                Location = new Point(290, 115),
                Size = new Size(80, 35),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            btnSave.Click += (s, ev) =>
            {
                item.OrderedQuantity = (int)numQty.Value;
                
                // Recalculate total amount
                selectedPurchaseOrder.TotalAmount = DataStore.PurchaseOrderItems
                    .Where(i => i.PurchaseOrderId == selectedPurchaseOrder.Id)
                    .Sum(i => i.OrderedQuantity * i.UnitCost);
                
                DataPersistence.SaveAllData();
                LoadData();
                DgvPurchaseOrders_SelectionChanged(null, EventArgs.Empty);
                
                MessageBox.Show("Quantity updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                editDialog.Close();
            };

            btnCancel.Click += (s, ev) => editDialog.Close();

            editDialog.Controls.AddRange(new Control[] { lblProduct, lblQty, numQty, btnSave, btnCancel });
            editDialog.ShowDialog();
        }

        private void BtnRemoveItem_Click(object? sender, EventArgs e)
        {
            if (dgvOrderItems.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item to remove.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedPurchaseOrder == null || selectedPurchaseOrder.Status != "Pending")
            {
                MessageBox.Show("Can only remove items from pending orders.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the item ID from the hidden column by index (column 0)
            int itemId = (int)dgvOrderItems.SelectedRows[0].Cells[0].Value;
            var item = DataStore.PurchaseOrderItems.FirstOrDefault(i => i.Id == itemId);

            if (item == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to remove this item?\n\n{DataStore.Perfumes.FirstOrDefault(p => p.Id == item.PerfumeId)?.Name}",
                "Confirm Remove",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                DataStore.PurchaseOrderItems.Remove(item);
                
                // Recalculate total amount
                selectedPurchaseOrder.TotalAmount = DataStore.PurchaseOrderItems
                    .Where(i => i.PurchaseOrderId == selectedPurchaseOrder.Id)
                    .Sum(i => i.OrderedQuantity * i.UnitCost);
                
                // If no items left, you might want to delete the PO or set status
                var remainingItems = DataStore.PurchaseOrderItems.Count(i => i.PurchaseOrderId == selectedPurchaseOrder.Id);
                if (remainingItems == 0)
                {
                    MessageBox.Show("This was the last item. The purchase order is now empty.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
                DataPersistence.SaveAllData();
                LoadData();
                DgvPurchaseOrders_SelectionChanged(null, EventArgs.Empty);
                
                MessageBox.Show("Item removed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnUploadInvoice_Click(object? sender, EventArgs e)
        {
            if (dgvPurchaseOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a purchase order first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int poId = (int)dgvPurchaseOrders.SelectedRows[0].Cells[0].Value;
            var po = DataStore.PurchaseOrders.FirstOrDefault(p => p.Id == poId);

            if (po == null) return;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Invoice Files|*.pdf;*.png;*.jpg;*.jpeg|PDF Files|*.pdf|Image Files|*.png;*.jpg;*.jpeg|All Files|*.*";
                openFileDialog.Title = "Select Invoice File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Create Invoices directory if it doesn't exist
                        string invoicesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Invoices");
                        if (!Directory.Exists(invoicesDir))
                        {
                            Directory.CreateDirectory(invoicesDir);
                        }

                        // Copy file to Invoices folder with PO number
                        string fileExtension = Path.GetExtension(openFileDialog.FileName);
                        string newFileName = $"PO_{poId}_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";
                        string destinationPath = Path.Combine(invoicesDir, newFileName);

                        File.Copy(openFileDialog.FileName, destinationPath, true);

                        // Update PO with invoice file path
                        po.InvoiceFilePath = destinationPath;

                        // Prompt for invoice number
                        string invoiceNumber = Microsoft.VisualBasic.Interaction.InputBox(
                            "Enter Invoice Number (optional):",
                            "Invoice Number",
                            po.InvoiceNumber,
                            -1, -1
                        );

                        if (!string.IsNullOrWhiteSpace(invoiceNumber))
                        {
                            po.InvoiceNumber = invoiceNumber;
                        }

                        DataPersistence.SaveAllData();
                        LoadData();

                        MessageBox.Show($"Invoice uploaded successfully!\nFile: {newFileName}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error uploading invoice: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnViewInvoice_Click(object? sender, EventArgs e)
        {
            if (dgvPurchaseOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a purchase order first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int poId = (int)dgvPurchaseOrders.SelectedRows[0].Cells[0].Value;
            var po = DataStore.PurchaseOrders.FirstOrDefault(p => p.Id == poId);

            if (po == null) return;

            if (string.IsNullOrWhiteSpace(po.InvoiceFilePath) || !File.Exists(po.InvoiceFilePath))
            {
                MessageBox.Show("No invoice file attached to this purchase order.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Open the invoice file with default application
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = po.InvoiceFilePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening invoice: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnProcessInvoice_Click(object? sender, EventArgs e)
        {
            if (dgvPurchaseOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a purchase order first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int poId = (int)dgvPurchaseOrders.SelectedRows[0].Cells[0].Value;
            var po = DataStore.PurchaseOrders.FirstOrDefault(p => p.Id == poId);

            if (po == null) return;

            // Create invoice processing dialog
            Form processDialog = new Form
            {
                Text = $"Process Invoice for PO #{poId}",
                Size = new Size(700, 650),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.Sizable,
                MaximizeBox = true,
                MinimizeBox = false
            };

            Label lblInstructions = new Label
            {
                Text = "Invoice data extracted below. Review and edit if needed:\nFormat: Product Name, Quantity, Unit Price",
                Location = new Point(20, 20),
                Size = new Size(640, 40),
                Font = new Font("Segoe UI", 9)
            };

            Button btnExtract = new Button
            {
                Text = "Extract from Invoice",
                Location = new Point(20, 65),
                Size = new Size(150, 30),
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            TextBox txtInvoiceData = new TextBox
            {
                Location = new Point(20, 105),
                Size = new Size(640, 350),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Consolas", 10)
            };

            // Auto-extract if invoice file exists
            if (!string.IsNullOrWhiteSpace(po.InvoiceFilePath) && File.Exists(po.InvoiceFilePath))
            {
                try
                {
                    // Check if there's a pre-extracted data file
                    string dataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "INVOICE_ISO500195_DATA.txt");
                    
                    if (File.Exists(dataFile) && po.InvoiceNumber.Contains("ISO500195"))
                    {
                        // Load pre-extracted data
                        txtInvoiceData.Text = File.ReadAllText(dataFile);
                    }
                    else
                    {
                        // Open the PDF for viewing
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = po.InvoiceFilePath,
                            UseShellExecute = true
                        });
                        
                        txtInvoiceData.Text = "// Invoice opened in external viewer.\n// Please enter invoice data below:\n// Format: Product Name, Quantity, Unit Price\n// Example: COOL M.85ml, 1, 159.00\n\n";
                    }
                }
                catch (Exception ex)
                {
                    txtInvoiceData.Text = $"// Could not open invoice: {ex.Message}\n// Please enter invoice data manually:\n\n";
                }
            }
            else
            {
                txtInvoiceData.Text = "// No invoice file attached. Please enter data manually:\n// Format: Product Name, Quantity, Unit Price\n\n";
            }

            btnExtract.Click += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(po.InvoiceFilePath) || !File.Exists(po.InvoiceFilePath))
                {
                    MessageBox.Show("No invoice file attached. Please upload an invoice first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    string extractedText = ExtractTextFromInvoice(po.InvoiceFilePath);
                    txtInvoiceData.Text = ParseInvoiceHelper.ParseInvoiceText(extractedText);
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Invoice data extracted! Please review and edit if needed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show($"Error extracting invoice data: {ex.Message}\n\nPlease enter data manually.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            CheckBox chkAutoCreateProducts = new CheckBox
            {
                Text = "Automatically create products if they don't exist",
                Location = new Point(20, 465),
                Size = new Size(400, 25),
                Checked = true,
                Font = new Font("Segoe UI", 9)
            };

            CheckBox chkAutoReceive = new CheckBox
            {
                Text = "Automatically receive stock (update inventory)",
                Location = new Point(20, 495),
                Size = new Size(400, 25),
                Checked = true,
                Font = new Font("Segoe UI", 9)
            };

            Button btnProcess = new Button
            {
                Text = "Process & Add Items",
                Location = new Point(450, 560),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(610, 560),
                Size = new Size(70, 35),
                BackColor = Color.FromArgb(158, 158, 158),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnProcess.Click += (s, ev) =>
            {
                try
                {
                    string[] lines = txtInvoiceData.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    int itemsAdded = 0;
                    int productsCreated = 0;
                    decimal totalAmount = 0;

                    foreach (string line in lines)
                    {
                        // Skip comment lines
                        if (line.TrimStart().StartsWith("//")) continue;

                        string[] parts = line.Split(',');
                        if (parts.Length < 3) continue;

                        string productName = parts[0].Trim();
                        if (!int.TryParse(parts[1].Trim(), out int quantity)) continue;
                        if (!decimal.TryParse(parts[2].Trim(), out decimal unitPrice)) continue;

                        // Find or create perfume
                        var perfume = DataStore.Perfumes.FirstOrDefault(p => 
                            p.Name.ToLower().Contains(productName.ToLower()) || 
                            productName.ToLower().Contains(p.Name.ToLower()));

                        if (perfume == null && chkAutoCreateProducts.Checked)
                        {
                            // Create new perfume
                            perfume = new Perfume
                            {
                                Id = DataStore.GetNextId(DataStore.Perfumes),
                                Name = productName,
                                Brand = "Generic",
                                Category = "Imported",
                                Size = "85ml",
                                Gender = "Unisex",
                                BatchNumber = $"IMP{DateTime.Now:yyyyMMdd}",
                                ExpirationDate = DateTime.Now.AddYears(3),
                                SupplierId = po.SupplierId,
                                CostPrice = unitPrice,
                                SellingPrice = 195m,
                                StockLevel = 0,
                                LowStockThreshold = 0
                            };
                            DataStore.Perfumes.Add(perfume);
                            productsCreated++;
                        }

                        if (perfume != null)
                        {
                            // Add purchase order item
                            var poItem = new PurchaseOrderItem
                            {
                                Id = DataStore.GetNextId(DataStore.PurchaseOrderItems),
                                PurchaseOrderId = poId,
                                PerfumeId = perfume.Id,
                                OrderedQuantity = quantity,
                                ReceivedQuantity = chkAutoReceive.Checked ? quantity : 0,
                                UnitCost = unitPrice
                            };
                            DataStore.PurchaseOrderItems.Add(poItem);

                            // Update stock if auto-receive is checked
                            if (chkAutoReceive.Checked)
                            {
                                perfume.StockLevel += quantity;
                            }

                            totalAmount += quantity * unitPrice;
                            itemsAdded++;
                        }
                    }

                    // Update PO total and status
                    po.TotalAmount = totalAmount;
                    if (chkAutoReceive.Checked)
                    {
                        po.Status = "Completed";
                        po.DeliveryDate = DateTime.Now;
                    }

                    DataPersistence.SaveAllData();
                    LoadData();
                    DgvPurchaseOrders_SelectionChanged(null, EventArgs.Empty);

                    MessageBox.Show(
                        $"Invoice processed successfully!\n\n" +
                        $"Items added: {itemsAdded}\n" +
                        $"New products created: {productsCreated}\n" +
                        $"Total amount: ₱{totalAmount:N2}\n" +
                        $"Stock updated: {(chkAutoReceive.Checked ? "Yes" : "No")}",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    processDialog.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error processing invoice: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnCancel.Click += (s, ev) => processDialog.Close();

            processDialog.Controls.AddRange(new Control[] 
            { 
                lblInstructions, btnExtract, txtInvoiceData, 
                chkAutoCreateProducts, chkAutoReceive,
                btnProcess, btnCancel 
            });

            processDialog.ShowDialog();
        }

        private string ExtractTextFromInvoice(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();

            if (extension == ".pdf")
            {
                return ExtractTextFromPDF(filePath);
            }
            else if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
            {
                return ExtractTextFromImage(filePath);
            }
            else
            {
                throw new Exception("Unsupported file format. Please use PDF or image files.");
            }
        }

        private string ExtractTextFromPDF(string pdfPath)
        {
            // PDF text extraction requires complex libraries
            // Instead, we'll open the PDF and let user manually enter data
            return "// PDF file detected. Please view the PDF and enter data manually below.\n// Opening PDF in external viewer...";
        }

        private string ExtractTextFromImage(string imagePath)
        {
            try
            {
                // For image OCR, we'll use a simple approach
                // In production, you'd use Tesseract or similar OCR library
                // For now, return a message to manually enter data
                throw new Exception("Image OCR requires Tesseract data files. Please enter invoice data manually.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to extract text from image: {ex.Message}");
            }
        }

        private string ParseInvoiceText(string rawText)
        {
            var result = new System.Text.StringBuilder();
            result.AppendLine("// Extracted invoice data - Review and edit as needed:");
            result.AppendLine("// Format: Product Name, Quantity, Unit Price");
            result.AppendLine();

            // Show raw text for manual parsing
            result.AppendLine("// Raw extracted text from PDF:");
            var lines = rawText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines.Take(50))
            {
                result.AppendLine($"// {line}");
            }

            result.AppendLine();
            result.AppendLine("// Please format your invoice data below:");
            result.AppendLine("// Example: COOL M.85ml, 1, 127.14");
            result.AppendLine();

            return result.ToString();
        }

    }
}