using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace ChatAppNET8.Services
{
    public static class PrinterHelper
    {
        public static void Print(string printerIp, string documentPath)
        {
            // Ensure the file exists
            if (!File.Exists(documentPath))
            {
                throw new FileNotFoundException($"The document {documentPath} does not exist.");
            }

            // Read the file content
            byte[] fileBytes = File.ReadAllBytes(documentPath);

            // Connect to the printer
            using (TcpClient client = new TcpClient(printerIp, 9100))
            {
                using (NetworkStream stream = client.GetStream())
                {
                    // Send the file content to the printer
                    stream.Write(fileBytes, 0, fileBytes.Length);
                    stream.Flush();
                }
            }
        }
    }
}
