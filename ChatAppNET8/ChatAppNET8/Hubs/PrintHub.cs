using ChatAppNET8.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ChatAppNET8.Hubs
{
    public class PrintHub : Hub
    {
        public async Task PrintDocument(string printerIp, string documentPath)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", documentPath.TrimStart('/'));
            Console.WriteLine($"Server file path: {filePath}"); // Log the file path for debugging

            string errorMsg = "";

            if (!File.Exists(documentPath))
            {
                errorMsg = $"Error: The document {documentPath} does not exist.";
                await Clients.Caller.SendAsync("Bad Path:", errorMsg);
                return;
            }

            try
            {
                PrinterHelper.Print(printerIp, documentPath);
                await Clients.Caller.SendAsync("PrintCompleted", "Document printed successfully.");
            }
            catch (Exception ex)
            {
                errorMsg = $"Error printing document: {ex.Message}";
                await Clients.Caller.SendAsync("PrintCompleted", errorMsg);
            }
        }
    }
}
