using System;
using System.Linq;
using System.Windows.Forms;

namespace PerfumeInventory
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            try
            {
                DataPersistence.LoadAllData();
                
                // Auto-import sales data if not already imported
                if (DataStore.Sales.Count == 0 || !DataStore.Sales.Any(s => s.SaleDate.Year == 2025 && s.SaleDate.Month >= 6))
                {
                    ImportSalesData_June_Dec2025.Import();
                }
                
                // Auto-import reseller sales if not already imported (check for reseller sales specifically)
                if (!DataStore.Sales.Any(s => s.ResellerId.HasValue && 
                    ((s.SaleDate.Year == 2025 && s.SaleDate.Month >= 10) || 
                     (s.SaleDate.Year == 2026 && s.SaleDate.Month == 1))))
                {
                    ImportResellerSales_Oct2025_Jan2026.Import();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}\nStarting with sample data.", "Data Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DataStore.InitializeSampleData();
            }
            
            Application.Run(new LoginForm());
        }
    }
}
