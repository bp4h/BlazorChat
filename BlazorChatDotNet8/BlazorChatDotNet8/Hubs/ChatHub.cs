using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace BlazorChatDotNet8.Hubs
{
    public class ChatHub : Hub
    {
        private readonly MessageService _messageService;

        public ChatHub(MessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task SendMessage(string message)
        {
            var user = Context.User.Identity.Name;
            var chatMessage = new ChatMessage { User = user, Message = message, Timestamp = DateTime.Now };

            _messageService.SaveMessage(chatMessage);

            await Clients.All.SendAsync("ReceiveMessage", Context.User.Identity.Name, message);
        }

        public override async Task OnConnectedAsync()
        {
            var user = Context.User.Identity.Name;

            if (!string.IsNullOrEmpty(user))
            {
                var messages = _messageService.GetMessages();
                await Clients.Caller.SendAsync("LoadMessages", messages);
                await Clients.All.SendAsync("UserConnected", Context.ConnectionId, user);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = Context.User.Identity.Name;
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId, user);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
