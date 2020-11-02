using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace prjMSIT127_G2_Noteledge.Hubs
{
    public class ProductHub: Hub
    {
        public static string getGroupIdString(int productId) => "productId-" + productId;
        public void SendProductId(int productId)
        {
            Groups.Add(Context.ConnectionId, getGroupIdString(productId));
            //Clients.Group(getGroupIdString(productId)).newMessage("安安");
        }
    }
}