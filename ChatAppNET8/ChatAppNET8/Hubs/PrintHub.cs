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
        private readonly ILogger<PrintHub> _logger;

        public PrintHub(ILogger<PrintHub> logger)
        {
            _logger = logger;
        }

        public async Task PrintDocument(string printerIp, string documentPath)
        {
            _logger.LogInformation($"Printing document: Printer IP = {printerIp}, Document Path = {documentPath}");
            string errorMsg = "";

            if (!File.Exists(documentPath))
            {
                errorMsg = $"Error: The document {documentPath} does not exist.";
                _logger.LogError(errorMsg);
                await Clients.Caller.SendAsync("PrintCompleted", errorMsg);
                return;
            }

            try
            {
                PrinterHelper.Print(printerIp, documentPath);
                await Clients.Caller.SendAsync("PrintCompleted", "Document printed successfully.");
                _logger.LogInformation("Print operation completed.");
            }
            catch (Exception ex)
            {
                errorMsg = $"Error printing document: {ex.Message}";
                _logger.LogError(errorMsg);
                await Clients.Caller.SendAsync("PrintCompleted", errorMsg);
            }
        }
    }
}
