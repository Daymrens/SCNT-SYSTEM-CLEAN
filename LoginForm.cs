using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace PerfumeInventory
{
    public class LoginForm : Form
    {
        private Label lblTitle, lblSubtitle, lblInfo;
        private Panel pnlLeft, pnlRight;
        private PictureBox picLogo;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "SCNT. COLLECTIONS - Login";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;

            // Left Panel - Beige Background with Branding
            pnlLeft = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(500, 600),
                BackColor = ColorTranslator.FromHtml("#FCF7F5")
                
            };
            pnlLeft.Paint += PnlLeft_Paint;

            // Background Image
            picLogo = new PictureBox
            {
                Location = new Point(0, 0),
                Size = new Size(500, 600),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = ColorTranslator.FromHtml("#FCF7F5"),
                Dock = DockStyle.Fill
            };

            // Try to load background image
            string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "bg_img.png");
            if (File.Exists(logoPath))
            {
                try
                {
                    picLogo.Image = Image.FromFile(logoPath);
                }
                catch
                {
                    picLogo.Visible = false;
                }
            }
            else
            {
                picLogo.Visible = false;
            }

            pnlLeft.Controls.Add(picLogo);

            // Right Panel - Role Selection
            pnlRight = new Panel
            {
                Location = new Point(500, 0),
                Size = new Size(500, 600),
                BackColor = Color.White
            };

            // Close button
            Button btnClose = new Button
            {
                Text = "✕",
                Location = new Point(450, 10),
                Size = new Size(40, 40),
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(100, 100, 100),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 16),
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
            btnClose.Click += (s, e) => Application.Exit();
            btnClose.MouseEnter += (s, e) => btnClose.ForeColor = Color.FromArgb(244, 67, 54);
            btnClose.MouseLeave += (s, e) => btnClose.ForeColor = Color.FromArgb(100, 100, 100);

            lblTitle = new Label
            {
                Text = "Select Your Role",
                Location = new Point(80, 150),
                Size = new Size(340, 40),
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblSubtitle = new Label
            {
                Text = "Choose how you want to access the system",
                Location = new Point(80, 195),
                Size = new Size(340, 25),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(108, 117, 125),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Admin Button
            Button btnAdmin = new Button
            {
                Text = "👤 ADMIN",
                Location = new Point(80, 270),
                Size = new Size(340, 80),
                BackColor = Color.FromArgb(63, 81, 181),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
            btnAdmin.Click += BtnAdmin_Click;
            btnAdmin.MouseEnter += (s, e) => btnAdmin.BackColor = Color.FromArgb(48, 63, 159);
            btnAdmin.MouseLeave += (s, e) => btnAdmin.BackColor = Color.FromArgb(63, 81, 181);

            // Cashier Button
            Button btnCashier = new Button
            {
                Text = "💰 CASHIER",
                Location = new Point(80, 370),
                Size = new Size(340, 80),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
            btnCashier.Click += BtnCashier_Click;
            btnCashier.MouseEnter += (s, e) => btnCashier.BackColor = Color.FromArgb(56, 142, 60);
            btnCashier.MouseLeave += (s, e) => btnCashier.BackColor = Color.FromArgb(76, 175, 80);

            lblInfo = new Label
            {
                Text = "Admin requires password authentication\nCashier provides instant access",
                Location = new Point(80, 480),
                Size = new Size(340, 40),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(108, 117, 125),
                TextAlign = ContentAlignment.TopCenter
            };

            pnlRight.Controls.AddRange(new Control[] { 
                btnClose, lblTitle, lblSubtitle, 
                btnAdmin, btnCashier, lblInfo 
            });

            this.Controls.AddRange(new Control[] { pnlLeft, pnlRight });
        }

        private void BtnAdmin_Click(object? sender, EventArgs e)
        {
            // Prompt for admin password
            string password = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter Admin Password:",
                "Admin Authentication",
                "",
                -1, -1);

            if (password == "admin123")
            {
                // Find admin user
                var adminUser = DataStore.Users.FirstOrDefault(u => u.Username == "admin");
                if (adminUser != null)
                {
                    DataStore.CurrentUser = adminUser;
                    OpenMainForm();
                }
                else
                {
                    MessageBox.Show("Admin user not found in the system!", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (!string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Incorrect password!", "Authentication Failed", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCashier_Click(object? sender, EventArgs e)
        {
            // Auto login as cashier
            var cashierUser = DataStore.Users.FirstOrDefault(u => u.Username == "cashier");
            if (cashierUser != null)
            {
                DataStore.CurrentUser = cashierUser;
                OpenMainForm();
            }
            else
            {
                MessageBox.Show("Cashier user not found in the system!", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenMainForm()
        {
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.FormClosed += (s, args) => this.Close();
            mainForm.Show();
        }

        private void PnlLeft_Paint(object? sender, PaintEventArgs e)
        {
            // Background is now handled by the PictureBox
            // This method can be removed or kept for future customization
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            // This method is no longer used but kept for compatibility
        }
    }
}