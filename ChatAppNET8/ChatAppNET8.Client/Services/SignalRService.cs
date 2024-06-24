using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace ChatAppNET8.Client.Services
{
    public class SignalRService
    {
        private HubConnection _chatHubConnection;
        private HubConnection _printHubConnection;

        public event Action<string, string> OnReceiveMessage;
        public event Action<string> OnPrintCompleted;
        public string? SocketConnectionId { get; private set; }
        public string? PrintConnectionId { get; private set; }


        public async Task StartChatHubConnectionAsync(string chatHubUrl)
        {
            _chatHubConnection = new HubConnectionBuilder()
                .WithUrl(chatHubUrl)
                .Build();

            _chatHubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                OnReceiveMessage?.Invoke(user, message);
            });

            await _chatHubConnection.StartAsync();
            SocketConnectionId = _chatHubConnection.ConnectionId; // grab connectionid after process is started
            Console.WriteLine($"Connected to SignalR {SocketConnectionId}"); // check id
        }

        public async Task SendMessageAsync(string user, string message)
        {
            if (_chatHubConnection.State == HubConnectionState.Connected)
            {
                await _chatHubConnection.SendAsync("SendMessage", user, message);
            }
            else
            {
                throw new InvalidOperationException("Chat SignalR connection is not established.");
            }
        }

        public async Task StartPrintHubConnectionAsync(string printHubUrl)
        {
            _printHubConnection = new HubConnectionBuilder()
                .WithUrl(printHubUrl)
                .Build();

            _printHubConnection.On<string>("PrintCompleted", (message) =>
            {
                OnPrintCompleted?.Invoke(message);
                Console.WriteLine($"Received PrintCompleted message: {message}");
            });

            await _printHubConnection.StartAsync();
            PrintConnectionId = _printHubConnection.ConnectionId;
            Console.WriteLine($"Connected to PrintHub {PrintConnectionId}");
        }

        public async Task PrintDocumentAsync(string printerIp, string documentPath)
        {
            if (_printHubConnection == null || _printHubConnection.State != HubConnectionState.Connected)
            {
                throw new InvalidOperationException("Hub connection is not started.");
            }

            await _printHubConnection.InvokeAsync("PrintDocument", printerIp, documentPath);
            Console.WriteLine($"PrintDocument invoked with Printer IP: {printerIp}, Document Path: {documentPath}");
        }

        public async Task StopConnectionAsync()
        {
            if (_printHubConnection != null)
            {
                await _printHubConnection.StopAsync();
                await _printHubConnection.DisposeAsync();
                Console.WriteLine("Disconnected from PrintHub");
            }
        }
    }
}
