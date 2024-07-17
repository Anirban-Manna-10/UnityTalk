using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnityTalk.Models;
using UnityTalk.viewModel;

namespace UnityTalk.Controllers
{
    public class ChatController : Controller
    {
        UnityTalkEntities8 dbC = new UnityTalkEntities8();
        public ActionResult Chat(int? grpId)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }
            if (grpId == null)
            {
                return new HttpStatusCodeResult(400, "Group Id is required");
            }
            int UserId = (int)Session["Id"];

            GroupTable grp = dbC.GroupTables.Find(grpId);

            UserTable user = dbC.UserTables.Find(UserId);

            GroupMemberTable mem = dbC.GroupMemberTables.SingleOrDefault(item => item.UserId == user.Uid && item.GrpId == grp.GrpId);
            //checking wheather the user Is Member of the group or not 
            if (mem == null)
            {
                return RedirectToAction("MemberNotFound", "Home", new { msg = "Oops, It looks like you are not a valid user of this group" });
            }

            List<MessageTable> message = dbC.MessageTables.Where(a => a.GrpId == grp.GrpId).ToList();

            ChatViewModel chat = new ChatViewModel();

            if (user != null && grp != null)
            {
                chat.Group = grp;
                chat.User = user;
                chat.Members = dbC.GroupMemberTables.Where(thisMember => thisMember.GrpId == grp.GrpId).ToList();
                chat.Messages = message;
                return View(chat);
            }
            TempData["Error"] = "Something is Wrong , Report this.";
            return RedirectToAction("grupview", "GroupView", new {grpid = (int)grpId});
        }
    }
}