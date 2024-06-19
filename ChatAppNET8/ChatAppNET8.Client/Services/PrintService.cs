namespace ChatAppNET8.Client.Services
{
    // Services/SignalRService.cs
    using Microsoft.AspNetCore.SignalR.Client;
    using System;
    using System.Threading.Tasks;

    public class SignalRService
    {
        private HubConnection _chatHubConnection;
        private HubConnection _printHubConnection;

        public event Action<string, string> OnReceiveMessage;
        public event Action<string> OnPrintCompleted;

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
            });

            await _printHubConnection.StartAsync();
        }

        public async Task PrintDocumentAsync(string printerIp, string documentPath)
        {
            if (_printHubConnection.State == HubConnectionState.Connected)
            {
                await _printHubConnection.InvokeAsync("PrintDocument", printerIp, documentPath);
            }
            else
            {
                throw new InvalidOperationException("Print SignalR connection is not established.");
            }
        }

        public async Task StopConnectionAsync()
        {
            if (_chatHubConnection != null)
            {
                await _chatHubConnection.DisposeAsync();
            }

            if (_printHubConnection != null)
            {
                await _printHubConnection.DisposeAsync();
            }
        }
    }
}
