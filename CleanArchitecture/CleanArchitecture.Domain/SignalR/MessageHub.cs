using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.SignalR
{
    [AllowAnonymous]
    public class MessageHub : Hub
    {
        public async Task SendResponse(string responseMsg)
        {
            await Clients.Caller.SendAsync("ResponseCode", responseMsg);
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public override Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            GenerateOnDisConnectedEvent();
            return base.OnDisconnectedAsync(exception);
        }

        public async Task GenerateOnDisConnectedEvent()
        {
            //UserConnectionRequestDto data = new UserConnectionRequestDto();
            //data.ConnectionId = Context.ConnectionId;
            //data.IsActive = false;
            //var jsonInString = JsonConvert.SerializeObject(data);
            //var client = new HttpClient();
            //var apiURL = ServerConfigurations.APIServerBaseUrl + "ManageUserConnections/updateUserConnection";
            //var result = await client.PutAsync(apiURL, new StringContent(jsonInString, Encoding.UTF8, "application/json"));
        }
    }
}
