using Microsoft.AspNetCore.SignalR;

namespace ChatAppNET8.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            string processedMessage = ProcessMessage(user, message);

            await Clients.All.SendAsync("ReceiveMessage", user, processedMessage);
        }

        private string ProcessMessage(string user, string message)
        {
            if (!int.TryParse(message, out _))
            {
                return $"{message.ToUpper()}";
            }
            else
            {
                return message+" is an integer";
            }
        }
    }
}
