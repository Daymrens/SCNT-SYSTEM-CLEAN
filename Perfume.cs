using System;

namespace PerfumeInventory
{
    public class Perfume
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // EDT, EDP, Parfum, Body Mist
        public string Size { get; set; } = string.Empty; // 30ml, 50ml, 100ml
        public string Gender { get; set; } = string.Empty; // Men, Women, Unisex
        public string BatchNumber { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
        public int SupplierId { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal ResellerPrice { get; set; } = 150m; // Default reseller price
        public decimal Price60ml { get; set; } = 175m; // Selling price for 60ml
        public decimal Cost60ml { get; set; } = 125m; // Order/cost price for 60ml
        public int StockLevel { get; set; }
        public int LowStockThreshold { get; set; } = 10;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string ImagePath { get; set; } = string.Empty;
    }

    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int LoyaltyPoints { get; set; }
        public DateTime RegisteredDate { get; set; } = DateTime.Now;
    }

    public class Reseller
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal DiscountRate { get; set; } = 0; // Percentage discount for wholesale
        public decimal TotalPurchases { get; set; } = 0;
        public DateTime RegisteredDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }

    public class Sale
    {
        public int Id { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.Now;
        public int? CustomerId { get; set; }
        public int? ResellerId { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public int UserId { get; set; }
    }

    public class SaleItem
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public int PerfumeId { get; set; }
        public string PerfumeName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class PurchaseOrder
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime? DeliveryDate { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Partial, Completed
        public decimal TotalAmount { get; set; }
        public string InvoiceFilePath { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;
    }

    public class PurchaseOrderItem
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public int PerfumeId { get; set; }
        public int OrderedQuantity { get; set; }
        public int ReceivedQuantity { get; set; }
        public decimal UnitCost { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // Admin, Cashier, Manager
        public bool IsActive { get; set; } = true;
    }
}

    public class Tester
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Status { get; set; } = "Available"; // Available, In Use, Low, Empty, Damaged
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
