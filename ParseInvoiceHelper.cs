using System;
using System.Linq;
using System.Text;

namespace PerfumeInventory
{
    public static class ParseInvoiceHelper
    {
        public static string ParseInvoiceText(string rawText)
        {
            var result = new StringBuilder();
            result.AppendLine("// Extracted text from PDF - Please format as: Product Name, Quantity, Unit Price");
            result.AppendLine();
            
            var lines = rawText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            
            // Show first 50 lines of extracted text
            result.AppendLine("// Raw extracted text:");
            foreach (var line in lines.Take(50))
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    result.AppendLine($"// {line.Trim()}");
                }
            }
            
            result.AppendLine();
            result.AppendLine("// Format your data below (one product per line):");
            result.AppendLine("// Example: COOL M.85ml, 1, 127.14");
            result.AppendLine();

            return result.ToString();
        }
    }
}
