using System;
using System.Collections.Generic;
using System.Linq;

namespace PerfumeInventory
{
    public static class DataStore
    {
        public static List<Perfume> Perfumes { get; set; } = new List<Perfume>();
        public static List<Supplier> Suppliers { get; set; } = new List<Supplier>();
        public static List<Customer> Customers { get; set; } = new List<Customer>();
        public static List<Reseller> Resellers { get; set; } = new List<Reseller>();
        public static List<Sale> Sales { get; set; } = new List<Sale>();
        public static List<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
        public static List<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
        public static List<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
        public static List<User> Users { get; set; } = new List<User>();
        public static List<Tester> Testers { get; set; } = new List<Tester>();
        
        public static User? CurrentUser { get; set; }

        public static int GetNextId<T>(List<T> list) where T : class
        {
            if (list.Count == 0) return 1;
            
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty == null) return 1;
            
            var maxId = list.Max(item => (int)(idProperty.GetValue(item) ?? 0));
            return maxId + 1;
        }

        public static void InitializeTesters()
        {
            // Initialize empty testers list if needed
            if (Testers == null)
            {
                Testers = new List<Tester>();
            }
        }

        public static void InitializeSampleData()
        {
            // Default admin user for initial login
            Users.Add(new User { Id = 1, Username = "admin", Password = "admin123", FullName = "Administrator", Role = "Admin" });
            
            // Empty system - ready for your data
            // Add your suppliers, customers, perfumes, and other data here
        }
    }
}
