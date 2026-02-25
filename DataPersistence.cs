using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PerfumeInventory
{
    public static class DataPersistence
    {
        private static readonly string DataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        private static readonly string PerfumesFile = Path.Combine(DataFolder, "perfumes.json");
        private static readonly string SuppliersFile = Path.Combine(DataFolder, "suppliers.json");
        private static readonly string CustomersFile = Path.Combine(DataFolder, "customers.json");
        private static readonly string ResellersFile = Path.Combine(DataFolder, "resellers.json");
        private static readonly string SalesFile = Path.Combine(DataFolder, "sales.json");
        private static readonly string SaleItemsFile = Path.Combine(DataFolder, "saleitems.json");
        private static readonly string PurchaseOrdersFile = Path.Combine(DataFolder, "purchaseorders.json");
        private static readonly string PurchaseOrderItemsFile = Path.Combine(DataFolder, "purchaseorderitems.json");
        private static readonly string UsersFile = Path.Combine(DataFolder, "users.json");
        private static readonly string TestersFile = Path.Combine(DataFolder, "testers.json");

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public static void EnsureDataFolderExists()
        {
            if (!Directory.Exists(DataFolder))
            {
                Directory.CreateDirectory(DataFolder);
            }
        }

        public static void SaveAllData()
        {
            try
            {
                EnsureDataFolderExists();

                File.WriteAllText(PerfumesFile, JsonSerializer.Serialize(DataStore.Perfumes, JsonOptions));
                File.WriteAllText(SuppliersFile, JsonSerializer.Serialize(DataStore.Suppliers, JsonOptions));
                File.WriteAllText(CustomersFile, JsonSerializer.Serialize(DataStore.Customers, JsonOptions));
                File.WriteAllText(ResellersFile, JsonSerializer.Serialize(DataStore.Resellers, JsonOptions));
                File.WriteAllText(SalesFile, JsonSerializer.Serialize(DataStore.Sales, JsonOptions));
                File.WriteAllText(SaleItemsFile, JsonSerializer.Serialize(DataStore.SaleItems, JsonOptions));
                File.WriteAllText(PurchaseOrdersFile, JsonSerializer.Serialize(DataStore.PurchaseOrders, JsonOptions));
                File.WriteAllText(PurchaseOrderItemsFile, JsonSerializer.Serialize(DataStore.PurchaseOrderItems, JsonOptions));
                File.WriteAllText(UsersFile, JsonSerializer.Serialize(DataStore.Users, JsonOptions));
                File.WriteAllText(TestersFile, JsonSerializer.Serialize(DataStore.Testers, JsonOptions));
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving data: {ex.Message}");
            }
        }

        public static void LoadAllData()
        {
            try
            {
                EnsureDataFolderExists();

                if (File.Exists(PerfumesFile))
                    DataStore.Perfumes = JsonSerializer.Deserialize<List<Perfume>>(File.ReadAllText(PerfumesFile)) ?? new List<Perfume>();

                if (File.Exists(SuppliersFile))
                    DataStore.Suppliers = JsonSerializer.Deserialize<List<Supplier>>(File.ReadAllText(SuppliersFile)) ?? new List<Supplier>();

                if (File.Exists(CustomersFile))
                    DataStore.Customers = JsonSerializer.Deserialize<List<Customer>>(File.ReadAllText(CustomersFile)) ?? new List<Customer>();

                if (File.Exists(ResellersFile))
                    DataStore.Resellers = JsonSerializer.Deserialize<List<Reseller>>(File.ReadAllText(ResellersFile)) ?? new List<Reseller>();

                if (File.Exists(SalesFile))
                    DataStore.Sales = JsonSerializer.Deserialize<List<Sale>>(File.ReadAllText(SalesFile)) ?? new List<Sale>();

                if (File.Exists(SaleItemsFile))
                    DataStore.SaleItems = JsonSerializer.Deserialize<List<SaleItem>>(File.ReadAllText(SaleItemsFile)) ?? new List<SaleItem>();

                if (File.Exists(PurchaseOrdersFile))
                    DataStore.PurchaseOrders = JsonSerializer.Deserialize<List<PurchaseOrder>>(File.ReadAllText(PurchaseOrdersFile)) ?? new List<PurchaseOrder>();

                if (File.Exists(PurchaseOrderItemsFile))
                    DataStore.PurchaseOrderItems = JsonSerializer.Deserialize<List<PurchaseOrderItem>>(File.ReadAllText(PurchaseOrderItemsFile)) ?? new List<PurchaseOrderItem>();

                if (File.Exists(UsersFile))
                    DataStore.Users = JsonSerializer.Deserialize<List<User>>(File.ReadAllText(UsersFile)) ?? new List<User>();

                if (File.Exists(TestersFile))
                    DataStore.Testers = JsonSerializer.Deserialize<List<Tester>>(File.ReadAllText(TestersFile)) ?? new List<Tester>();

                // If no data exists, initialize with sample data
                if (DataStore.Perfumes.Count == 0 && DataStore.Suppliers.Count == 0)
                {
                    DataStore.InitializeSampleData();
                    SaveAllData();
                }
                
                // Initialize testers if they don't exist
                if (DataStore.Testers.Count == 0)
                {
                    DataStore.InitializeTesters();
                    SaveAllData();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading data: {ex.Message}");
            }
        }
    }
}
