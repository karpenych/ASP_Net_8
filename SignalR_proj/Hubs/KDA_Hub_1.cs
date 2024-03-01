using Microsoft.AspNetCore.SignalR;

namespace SignalR_proj.Hubs
{
    public class KDA_Hub_1 : Hub
    {
        

        public async Task SendMessage(string STR)
        {
            string[] strs = STR.Split(new char[] { '@' });
            string  user    = strs[0], 
                    message = strs[1],
                    time    = strs[2],
                    prc     = strs[3];
            await Clients.All.SendAsync("UpdateData", user, message, time, prc);
        }

        public override async Task OnConnectedAsync()
        {
            
            await Clients.All.SendAsync("Notify", $"Чел номер {Context.ConnectionId} вошел в чат");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} покинул в чат");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
