using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Models.MemberModels;

namespace prjMSIT127_G2_Noteledge.Hubs
{
    public class ChatHub : Hub
    {
        static Dictionary<string, int> userIds = new Dictionary<string, int>();
        public void sendUserId(int userId)
        {
            lock (userIds)
            {
                userIds[Context.ConnectionId] = userId;
            }
            Groups.Add(Context.ConnectionId, userId.ToString());
            //Clients.Group(userId.ToString()).hello("YEEEEEEEEEEE");
        }
        //public override Task OnConnected()
        //{
        //    return base.OnConnected();
        //}
        public override Task OnDisconnected(bool stopCalled)
        {
            lock (userIds)
            {
                if(userIds.TryGetValue(Context.ConnectionId, out _))
                {
                    userIds.Remove(Context.ConnectionId);
                }
            }
            return base.OnDisconnected(stopCalled);
        }
        //public override Task OnReconnected()
        //{
        //    return base.OnReconnected();
        //}
    }
}