using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnityTalk.Models;
using UnityTalk.viewModel;

namespace UnityTalk.Controllers
{
    public class MeetingController : Controller
    {
        UnityTalkEntities8 dbM = new UnityTalkEntities8();
        // GET: Meeting
        public ActionResult meetingView(int? grpId)
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

            GroupTable grp = dbM.GroupTables.Find(grpId);

            UserTable user = dbM.UserTables.Find(UserId);

            GroupMemberTable mem = dbM.GroupMemberTables.SingleOrDefault(item => item.UserId == user.Uid && item.GrpId == grp.GrpId);
            //checking wheather the user Is Member of the group or not 
            if (mem == null)
            {
                return RedirectToAction("MemberNotFound", "Home", new { msg = "Oops, It looks like you are not a valid user of this group" });
            }

            List<MeetingTable> links = dbM.MeetingTables.Where(a => a.GrpId == grp.GrpId).ToList();
            links.Reverse();
            GroupView groupView = new GroupView();
            groupView.MeetingTabl = links;
            groupView.Group = grp;

            return View(groupView);
        }

        [HttpPost]
        public ActionResult meetingView(GroupView meet,int? grpId)
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

            GroupTable grp = dbM.GroupTables.Find(grpId);

            UserTable user = dbM.UserTables.Find(UserId);

            GroupMemberTable mem = dbM.GroupMemberTables.SingleOrDefault(item => item.UserId == user.Uid && item.GrpId == grp.GrpId);
            //checking wheather the user Is Member of the group or not 
            if (mem == null)
            {
                return RedirectToAction("MemberNotFound", "Home", new { msg = "Oops, It looks like you are not a valid user of this group" });
            }
            if (string.IsNullOrWhiteSpace(meet.MeetingModel.MeetingLink))
            {
                TempData["Error"] = "Meeting Link Can not be null";
            }
            else
            {

                MeetingTable newLink = new MeetingTable();
                newLink.MeetingLink = meet.MeetingModel.MeetingLink;
                newLink.GrpId = grp.GrpId;
                newLink.CreatorId = mem.MemberId;
                dbM.MeetingTables.Add(newLink);
                dbM.SaveChanges();
                TempData["Success"] = "Meeting Link shared with the Group";
            }

            return RedirectToAction("meetingView", new {grpId = grp.GrpId});
        }



        public ActionResult makeMeet()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }
            return View();
        }
    }
}