using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ChatAppNET8.Hubs
{
    public class PrintHub : Hub
    {
        public async Task PrintDocument(string printerIp, string documentPath)
        {
       
            PrinterHelper.Print(printerIp, documentPath);

            await Clients.Caller.SendAsync("PrintCompleted", "Document printed successfully.");
        }
    }
}
