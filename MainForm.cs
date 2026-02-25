using System;
using System.Drawing;
using System.Windows.Forms;

namespace PerfumeInventory
{
    public class MainForm : Form
    {
        private Panel pnlSidebar, pnlContent;
        private Button btnDashboard, btnInventory, btnSales, btnSuppliers, btnCustomers, btnResellers, btnPurchaseOrders, btnReports, btnTesters, btnLogout, btnToggleSidebar, btnCheckout, btnResellerDelivery;
        private Label lblUser, lblRole;
        private Form? currentForm;
        private bool sidebarExpanded = true;
        private const int SIDEBAR_WIDTH_EXPANDED = 250;
        private const int SIDEBAR_WIDTH_COLLAPSED = 60;

        public MainForm()
        {
            InitializeComponent();
            LoadForm(new DashboardForm());
        }

        private void InitializeComponent()
        {
            this.Text = "SCNT Vault - Inventory Management";
            this.Size = new Size(1400, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 245);
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = new Size(1200, 700);

            // Sidebar
            pnlSidebar = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(SIDEBAR_WIDTH_EXPANDED, this.ClientSize.Height),
                BackColor = Color.FromArgb(45, 52, 54),
                Dock = DockStyle.Left
            };

            // Toggle Button (Hamburger Menu)
            btnToggleSidebar = new Button
            {
                Text = "☰",
                Location = new Point(10, 10),
                Size = new Size(40, 40),
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 18),
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
            btnToggleSidebar.Click += BtnToggleSidebar_Click;

            Label lblLogo = new Label
            {
                Text = "SCNT\nVAULT",
                Location = new Point(20, 60),
                Size = new Size(210, 60),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft
            };

            lblUser = new Label
            {
                Text = DataStore.CurrentUser?.FullName ?? "User",
                Location = new Point(20, 130),
                Size = new Size(210, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White
            };

            lblRole = new Label
            {
                Text = DataStore.CurrentUser?.Role ?? "Role",
                Location = new Point(20, 150),
                Size = new Size(210, 15),
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.FromArgb(200, 200, 200)
            };

            int yPos = 190;
            btnDashboard = CreateMenuButton("📊 Dashboard", yPos);
            btnInventory = CreateMenuButton("🧴 Inventory", yPos += 50);
            btnSales = CreateMenuButton("📈 Sales History", yPos += 50);
            btnSuppliers = CreateMenuButton("📦 Suppliers", yPos += 50);
            btnCustomers = CreateMenuButton("👤 Customers", yPos += 50);
            btnResellers = CreateMenuButton("🤝 Resellers", yPos += 50);
            btnResellerDelivery = CreateMenuButton("🚚 Reseller Delivery", yPos += 50);
            btnPurchaseOrders = CreateMenuButton("📋 Purchase Orders", yPos += 50);
            btnTesters = CreateMenuButton("🧪 Testers", yPos += 50);
            btnReports = CreateMenuButton("📊 Reports", yPos += 50);
            
            btnLogout = CreateMenuButton("🚪 Logout", 0);
            btnLogout.Dock = DockStyle.Bottom;

            btnDashboard.Click += (s, e) => LoadForm(new DashboardForm());
            btnInventory.Click += (s, e) => LoadForm(new InventoryForm());
            btnSales.Click += (s, e) => LoadForm(new SalesHistoryForm());
            btnSuppliers.Click += (s, e) => LoadForm(new SuppliersForm());
            btnCustomers.Click += (s, e) => LoadForm(new CustomersForm());
            btnResellers.Click += (s, e) => LoadForm(new ResellersForm());
            btnResellerDelivery.Click += (s, e) => LoadForm(new ResellerDeliveryForm());
            btnPurchaseOrders.Click += (s, e) => LoadForm(new PurchaseOrdersForm());
            btnTesters.Click += (s, e) => LoadForm(new TestersForm());
            btnReports.Click += (s, e) => LoadForm(new ReportsForm());
            btnLogout.Click += BtnLogout_Click;

            pnlSidebar.Controls.AddRange(new Control[] {
                btnToggleSidebar, lblLogo, lblUser, lblRole, btnDashboard, btnInventory,
                btnSales, btnSuppliers, btnCustomers, btnResellers, btnResellerDelivery, btnPurchaseOrders, btnTesters, btnReports, btnLogout
            });

            // Apply role-based access control
            ApplyRoleBasedAccess();

            // Content Panel
            pnlContent = new Panel
            {
                BackColor = Color.FromArgb(240, 240, 245),
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20, 80, 20, 20) // Extra top padding for checkout button
            };

            // Checkout Button (Top Right) - Added after content panel to appear on top
            btnCheckout = new Button
            {
                Text = "💰 CHECKOUT",
                Size = new Size(180, 50),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                FlatAppearance = { BorderSize = 0 }
            };
            btnCheckout.Location = new Point(this.ClientSize.Width - 200, 20);
            btnCheckout.Click += (s, e) => LoadForm(new POSForm());
            btnCheckout.MouseEnter += (s, e) => btnCheckout.BackColor = Color.FromArgb(56, 142, 60);
            btnCheckout.MouseLeave += (s, e) => btnCheckout.BackColor = Color.FromArgb(76, 175, 80);

            this.Controls.AddRange(new Control[] { pnlContent, pnlSidebar });
            this.Controls.Add(btnCheckout);
            btnCheckout.BringToFront();
            
            this.Resize += MainForm_Resize;
        }

        private void BtnToggleSidebar_Click(object? sender, EventArgs e)
        {
            sidebarExpanded = !sidebarExpanded;
            
            // Create smooth animation
            Timer animationTimer = new Timer { Interval = 10 };
            int targetWidth = sidebarExpanded ? SIDEBAR_WIDTH_EXPANDED : SIDEBAR_WIDTH_COLLAPSED;
            int step = sidebarExpanded ? 10 : -10;
            
            animationTimer.Tick += (s, ev) =>
            {
                if ((sidebarExpanded && pnlSidebar.Width < targetWidth) || 
                    (!sidebarExpanded && pnlSidebar.Width > targetWidth))
                {
                    pnlSidebar.Width += step;
                }
                else
                {
                    pnlSidebar.Width = targetWidth;
                    animationTimer.Stop();
                    UpdateSidebarContent();
                }
            };
            
            animationTimer.Start();
        }

        private void UpdateSidebarContent()
        {
            if (sidebarExpanded)
            {
                // Show full text
                btnToggleSidebar.Text = "☰";
                btnToggleSidebar.Location = new Point(10, 10);
                btnToggleSidebar.Size = new Size(40, 40);
                
                foreach (Control ctrl in pnlSidebar.Controls)
                {
                    if (ctrl is Label label)
                    {
                        label.Visible = true;
                    }
                    else if (ctrl is Button button && button != btnToggleSidebar && button != btnLogout)
                    {
                        // Only update visible buttons
                        if (!button.Visible) continue;
                        
                        button.Location = new Point(10, button.Location.Y);
                        button.Size = new Size(230, 40);
                        button.TextAlign = ContentAlignment.MiddleLeft;
                        // Restore original text
                        if (button == btnDashboard) button.Text = "📊 Dashboard";
                        else if (button == btnInventory) button.Text = "🧴 Inventory";
                        else if (button == btnSales) button.Text = "📈 Sales History";
                        else if (button == btnSuppliers) button.Text = "📦 Suppliers";
                        else if (button == btnCustomers) button.Text = "👤 Customers";
                        else if (button == btnResellers) button.Text = "🤝 Resellers";
                        else if (button == btnResellerDelivery) button.Text = "🚚 Reseller Delivery";
                        else if (button == btnPurchaseOrders) button.Text = "📋 Purchase Orders";
                        else if (button == btnTesters) button.Text = "🧪 Testers";
                        else if (button == btnReports) button.Text = "📊 Reports";
                    }
                    else if (ctrl is Button logoutBtn && logoutBtn == btnLogout)
                    {
                        logoutBtn.Size = new Size(250, 40);
                        logoutBtn.TextAlign = ContentAlignment.MiddleLeft;
                        logoutBtn.Text = "🚪 Logout";
                    }
                }
            }
            else
            {
                // Show only icons - centered in collapsed sidebar
                btnToggleSidebar.Text = "☰";
                btnToggleSidebar.Location = new Point(10, 10);
                btnToggleSidebar.Size = new Size(40, 40);
                
                foreach (Control ctrl in pnlSidebar.Controls)
                {
                    if (ctrl is Label label)
                    {
                        label.Visible = false;
                    }
                    else if (ctrl is Button button && button != btnToggleSidebar && button != btnLogout)
                    {
                        // Only update visible buttons
                        if (!button.Visible) continue;
                        
                        button.Location = new Point(5, button.Location.Y);
                        button.Size = new Size(50, 40);
                        button.TextAlign = ContentAlignment.MiddleCenter;
                        // Show only emoji
                        if (button == btnDashboard) button.Text = "📊";
                        else if (button == btnInventory) button.Text = "🧴";
                        else if (button == btnSales) button.Text = "📈";
                        else if (button == btnSuppliers) button.Text = "📦";
                        else if (button == btnCustomers) button.Text = "👤";
                        else if (button == btnResellers) button.Text = "🤝";
                        else if (button == btnResellerDelivery) button.Text = "🚚";
                        else if (button == btnPurchaseOrders) button.Text = "📋";
                        else if (button == btnTesters) button.Text = "🧪";
                        else if (button == btnReports) button.Text = "📊";
                    }
                    else if (ctrl is Button logoutBtn && logoutBtn == btnLogout)
                    {
                        logoutBtn.Size = new Size(60, 40);
                        logoutBtn.TextAlign = ContentAlignment.MiddleCenter;
                        logoutBtn.Text = "🚪";
                    }
                }
            }
        }

        private void MainForm_Resize(object? sender, EventArgs e)
        {
            if (currentForm != null && !currentForm.IsDisposed)
            {
                currentForm.Refresh();
            }
            
            // Reposition checkout button on resize
            if (btnCheckout != null)
            {
                btnCheckout.Location = new Point(this.ClientSize.Width - 200, 20);
            }
        }

        private Button CreateMenuButton(string text, int yPos)
        {
            return new Button
            {
                Text = text,
                Location = new Point(10, yPos),
                Size = new Size(230, 40),
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
        }

        public void LoadForm(Form form)
        {
            if (currentForm != null)
            {
                currentForm.Close();
            }

            currentForm = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(form);
            form.Show();
        }

        private void ApplyRoleBasedAccess()
        {
            // Check if current user is a Cashier
            if (DataStore.CurrentUser?.Role == "Cashier")
            {
                // Hide buttons that cashiers shouldn't access
                btnSales.Visible = false;
                btnSuppliers.Visible = false;
                btnCustomers.Visible = false;
                btnResellers.Visible = false;
                btnResellerDelivery.Visible = false;
                btnPurchaseOrders.Visible = false;
                btnTesters.Visible = false;
                btnReports.Visible = false;

                // Keep visible: Dashboard, Inventory (view only), Checkout
            }
        }

        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                DataStore.CurrentUser = null;
                this.Hide();
                LoginForm loginForm = new LoginForm();
                loginForm.FormClosed += (s, args) => this.Close();
                loginForm.Show();
            }
        }
    }
}