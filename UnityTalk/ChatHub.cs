using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using UnityTalk.Models;

namespace UnityTalk
{
    public class ChatHub : Hub
    {
        private UnityTalkEntities8 dbC = new UnityTalkEntities8();
       
        public async Task JoinGroup(string groupId)
        {
            await Groups.Add(Context.ConnectionId, groupId);
        }

        // Leave a group
        public async Task LeaveGroup(string groupId)
        {
            await Groups.Remove(Context.ConnectionId, groupId);
        }

        public void Send(int GrpId, string message, int UserId)
        {

            // Find the group by ID
            GroupTable grp = dbC.GroupTables.Find(GrpId);

            if (grp == null)
            {
                // Handle null group by informing the caller and stopping further execution
                Clients.All.addNewMessageToPage("Error", "Group not found.");
                return;
            }

            var groupMember = dbC.GroupMemberTables.Where(gm => gm.UserId == UserId && gm.GrpId == GrpId).Single();

            if (groupMember == null)
            {
                Clients.All.addNewMessageToPage("Error", "Sender not part of this group.");
                return;
            }

            // Create a new message
            MessageTable msg = new MessageTable
            {
                SenderId = groupMember.MemberId,
                GrpId = GrpId,
                Cont = message,
                Time = DateTime.Now.TimeOfDay,
                Date = DateTime.Now
            };

            // Find the group member associated with the sender (replace with the actual sender's ID)

            // Get the name of the sender for broadcasting

            // Save the message to the database
            dbC.MessageTables.Add(msg);
            dbC.SaveChanges();

            var senderName = groupMember.UserTable.Name;
            var senderId = groupMember.UserTable.Uid;
            var senderImage = groupMember.UserTable.UserImgPath;

            var date = DateTime.Now.ToShortDateString();
            var time = DateTime.Now.ToShortTimeString();

            string fullImageUrl = ConvertToFullUrl(senderImage);

            // Call the addNewMessageToPage method to update clients in the group
            //Clients.Group(grp.GrpName).addNewMessageToPage(senderName, message);
            Clients.Group(msg.GrpId.ToString()).addNewMessageToPage(senderId,senderName, message, date, time, fullImageUrl);
        }


        public string GetBaseUrl()
        {
            var request = HttpContext.Current.Request;
            return $"{request.Url.Scheme}://{request.Url.Authority}{request.ApplicationPath.TrimEnd('/')}/";
        }

        public string ConvertToFullUrl(string relativePath)
        {
            string baseUrl = GetBaseUrl();
            return $"{baseUrl.TrimEnd('/')}/{relativePath.TrimStart('~', '/')}";
        }
    }
}