# Setup Guide - SCNT System

## Quick Start

### Option 1: Using Visual Studio

1. Install Visual Studio 2022 (Community Edition is free)
2. Open `PerfumeInventory.csproj`
3. Press F5 to build and run

### Option 2: Using .NET CLI

```bash
# Build the project
dotnet build

# Run the application
dotnet run
```

### Option 3: Publish as Executable

```bash
# Publish as self-contained executable
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true

# The executable will be in: bin\Release\net8.0-windows\win-x64\publish\
```

## First Time Setup

1. **Login**
   - Username: `admin`
   - Password: `admin123`

2. **Change Default Password**
   - Go to User Management
   - Edit the admin user
   - Set a secure password

3. **Add Your Data**
   - Start by adding Suppliers
   - Add Customers and Resellers
   - Add your Perfume inventory
   - Configure categories as needed

## Data Storage

- Data is saved automatically to `perfume_inventory_data.json`
- Backup this file regularly
- To reset the system, delete this file (a new one will be created)

## Troubleshooting

### Application won't start
- Ensure .NET 8.0 Runtime is installed
- Check Windows compatibility

### Data not saving
- Ensure the application has write permissions in its directory
- Check if `perfume_inventory_data.json` is not read-only

### Login issues
- If you forgot the password, delete `perfume_inventory_data.json` to reset

## Building for Distribution

To create a standalone executable for distribution:

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

This creates a single .exe file that can run on any Windows machine without requiring .NET installation.

## System Features

- **Dashboard**: Overview of sales, inventory, and key metrics
- **Inventory**: Manage perfume stock levels
- **POS**: Process sales transactions
- **Customers**: Track customer information and loyalty points
- **Suppliers**: Manage supplier contacts and orders
- **Resellers**: Handle reseller accounts and deliveries
- **Purchase Orders**: Create and track purchase orders
- **Sales History**: View and analyze past sales
- **Reports**: Generate various business reports
- **Testers**: Manage tester inventory

## Need Help?

Refer to the main documentation or contact support.
