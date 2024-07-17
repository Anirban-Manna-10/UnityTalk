using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnityTalk.Models;
using UnityTalk.viewModel;

namespace UnityTalk.Controllers
{
    public class GroupViewController : Controller
    {
        UnityTalkEntities8 dbGv = new UnityTalkEntities8();
        // GET: GrouupView

        private bool isMem(int grpid)
        {
            if (Session["Id"] == null || !int.TryParse(Session["Id"].ToString(), out int userId))
            {
                return false;
            }
            var mem = dbGv.GroupMemberTables.SingleOrDefault(item => item.UserId == userId && item.GrpId == grpid);
            return mem != null;
        }

        public ActionResult grupview(int? grpid)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }

            if (grpid == null)
            {
                return new HttpStatusCodeResult(400, "Group Id is required");   
            }
            
            bool isuser = isMem((int)grpid);
            if (!isuser)
            {
                return RedirectToAction("PageNotFound", "Home", new { msg = "Oops, It looks like you are not a valid user of this group" });
            }
            else
            {
                GroupTable grp = dbGv.GroupTables.Find((int)grpid);

                List<AnnouncementTable> announcementTables = grp.AnnouncementTables.ToList();
                announcementTables.Reverse();

                List<GroupMemberTable> grpMemberTables = grp.GroupMemberTables.ToList();
                //add more to sent mode List and bind it bellow


                GroupView grpview = new GroupView()
                {
                    Group = grp,
                    AnnouncementTabl = announcementTables,
                    GroupMemberTabl = grpMemberTables
                };
                return View(grpview);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult createAnnouncement(GroupView text,int? grpid)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }

            if (grpid == null)
            {
                TempData["Error"] =  "Oops, Can not Find the Page!";
            }

            bool isuser = isMem((int)grpid);
            if (!isuser)
            {
                return RedirectToAction("PageNotFound", "Home", new { msg = "Oops, It looks like you are not a valid user of this group" });
            }
            else
            {

                GroupTable grp = dbGv.GroupTables.Find(grpid);
                UserTable user = dbGv.UserTables.Find((int)Session["Id"]);
                if (ModelState.IsValid)
                {
                    var mem = dbGv.GroupMemberTables.SingleOrDefault(item => item.UserId == user.Uid && item.GrpId == grp.GrpId);
                    if ((int)user.Uid != (int)grp.UserTable.Uid)
                    {
                        TempData["Error"] = "You are not admin! Only Admin can Create Announcement";
                        return RedirectToAction("grupview", new { grpid = grp.GrpId });
                    }

                    bool isEmpty = string.IsNullOrWhiteSpace(text.AnnounceModel.Cont.Trim());
                    if (isEmpty )
                    {
                        TempData["Error"] = "Give a valid Announcement!!";
                        return RedirectToAction("grupview", new { grpid = grp.GrpId });
                    }

                    AnnouncementTable announcement = new AnnouncementTable()
                    {
                        Cont = text.AnnounceModel.Cont,
                        GrpId = grp.GrpId,
                        SenderId = mem.MemberId,
                        Time = DateTime.Now.TimeOfDay,
                        Date = DateTime.Today.Date
                    };

                    dbGv.AnnouncementTables.Add(announcement);
                    dbGv.SaveChanges();
                }

                return RedirectToAction("grupview", new { grpid = grp.GrpId});
            }
        }


        public ActionResult removeAnnouncement(int id)
        {
                AnnouncementTable aun = dbGv.AnnouncementTables.Find(id);
                UserTable user = dbGv.UserTables.Find((int)Session["Id"]);
                GroupTable grp = dbGv.GroupTables.Find(aun.GroupTable.GrpId);

                if ((int)user.Uid != (int)aun.GroupTable.UserTable.Uid)
                {
                    TempData["notify"] = "You are not admin! Only Admin can Delete Announcement";
                    return RedirectToAction("grupview", new { grpid = aun.GroupTable.GrpId });
                }

                dbGv.AnnouncementTables.Remove(aun);
                dbGv.SaveChanges();

                return RedirectToAction("grupview", new { grpid = grp.GrpId });
        }

        public ActionResult grpInfo(int? grpId)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }

            if (grpId == null)
            {
                return new HttpStatusCodeResult(400, "Group Id is required");
            }

            bool isuser = isMem((int)grpId);
            if (!isuser)
            {
                return RedirectToAction("PageNotFound", "Home", new { msg = "Oops, It looks like you are not a valid user of this group" });
            }
            else
            {
                GroupTable grp = dbGv.GroupTables.Find((int)grpId);
                if (grp == null)
                {
                    return RedirectToAction("PageNotFound", "Home", new { msg = "Oops,Cant find this Group !!, Report us." });
                }
                List<GroupMemberTable> grpMemberTables = grp.GroupMemberTables.ToList();
                //add more to sent mode List and bind it bellow


                GroupView grpview = new GroupView()
                {
                    Group = grp,
                    GroupMemberTabl = grpMemberTables
                };
                return View(grpview);
            }
        }


        public ActionResult removeMember(int? MemberId)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }
            //getting the UserTable Information
            GroupMemberTable memInfo = dbGv.GroupMemberTables.Find(MemberId);

            if (memInfo == null)
            {
                return RedirectToAction("PageNotFound", "Home", new { msg = "Oops, It looks like it is not a valid user of this group" });
            }

            var thisGrpId = memInfo.GrpId;
            //Getting the groupTable Info from GrpId take AdminId
            GroupTable grpInfo = dbGv.GroupTables.Find(thisGrpId);
            var GrpAdminUserId = grpInfo.UserTable.Uid;

            //now check Weather the SessionId is the Admin of that group, if yes then remove user 
            if ((int)Session["Id"] == GrpAdminUserId)
            {
                if (GrpAdminUserId != memInfo.UserId)
                {
                    var rowsToDelete = dbGv.MeetingTables.Where(t => t.CreatorId == memInfo.MemberId).ToList();
                    if (rowsToDelete.Count > 0)
                    {
                        dbGv.MeetingTables.RemoveRange(rowsToDelete);
                        dbGv.SaveChanges();
                    }
                    var msgsToDelete = dbGv.MessageTables.Where(t => t.SenderId == memInfo.MemberId).ToList();
                    if (msgsToDelete.Count > 0)
                    {
                        dbGv.MessageTables.RemoveRange(msgsToDelete);
                        dbGv.SaveChanges();
                    }
                    var filesToDelete = dbGv.FileTables.Where(t => t.SenderId == memInfo.MemberId).ToList();
                    if (filesToDelete.Count > 0)
                    {
                        dbGv.FileTables.RemoveRange(filesToDelete);
                        dbGv.SaveChanges();
                    }
                    TempData["Success"] = "The Member" + memInfo.UserTable.Name+", Has successfully Removed From Group";
                    dbGv.GroupMemberTables.Remove(memInfo);
                    dbGv.SaveChanges();

                    return RedirectToAction("grpInfo", "GroupView", new { grpId = grpInfo.GrpId });
                }
                else
                {
                    TempData["Error"] = "Can Not Remove You , As u are the Group Admin";
                    return RedirectToAction("grpInfo", "GroupView", new { grpId = grpInfo.GrpId });
                }
            }
            else
            {
                // Set a one time error message to display in the view
                TempData["Error"] = "Only group admins can remove members.";
                return RedirectToAction("grpInfo", "GroupView", new { grpId = grpInfo.GrpId });
            }
        }


        public ActionResult leaveGroup(int? GrpId)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }

            if (GrpId == null)
            {
                return new HttpStatusCodeResult(400, "Group Id is required");
            }
            //First Get GroupID
            int UserId = (int)Session["Id"];

            //search the MEMBERID from SEssionId and GroupId
            GroupMemberTable memInfo = dbGv.GroupMemberTables.FirstOrDefault(a => a.UserId == UserId && a.GrpId == GrpId);
            var thisMemID = memInfo.MemberId;

            //if the mwmberId is not the sessionID themn give error else confirm leaveeeeeeeeeee
            if (memInfo.UserTable.Uid == UserId)
            {
                if (memInfo.GroupTable.GrpAdminId != memInfo.UserTable.Uid)
                {
                    var rowsToDelete = dbGv.MeetingTables.Where(t => t.CreatorId == memInfo.MemberId).ToList();
                    if (rowsToDelete.Count > 0)
                    {
                        dbGv.MeetingTables.RemoveRange(rowsToDelete);
                        dbGv.SaveChanges();
                    }
                    var msgsToDelete = dbGv.MessageTables.Where(t => t.SenderId == memInfo.MemberId).ToList();
                    if (msgsToDelete.Count > 0)
                    {
                        dbGv.MessageTables.RemoveRange(msgsToDelete);
                        dbGv.SaveChanges();
                    }
                    var filesToDelete = dbGv.FileTables.Where(t => t.SenderId == memInfo.MemberId).ToList();
                    if (filesToDelete.Count > 0)
                    {
                        dbGv.FileTables.RemoveRange(filesToDelete);
                        dbGv.SaveChanges();
                    }
                    var announceToDelete = dbGv.AnnouncementTables.Where(t => t.SenderId == memInfo.MemberId).ToList();
                    if (announceToDelete.Count > 0)
                    {
                        dbGv.AnnouncementTables.RemoveRange(announceToDelete);
                        dbGv.SaveChanges();
                    }
                    TempData["Success"] = "You are No Longer a member of the Group:" + memInfo.GroupTable.GrpName;
                    dbGv.GroupMemberTables.Remove(memInfo);
                    dbGv.SaveChanges();
                    return RedirectToAction("allGroup", "Group");
                }
                else
                {
                    TempData["Error"] = "Make a New Admin to leave the group!";
                    return RedirectToAction("grpInfo", "GroupView", new { grpId = GrpId });
                }
            }
            else
            {
                TempData["Error"] = "You Can Not Remove Others, as You Are not Admin!";
                return RedirectToAction("grpInfo", "GroupView", new { GrpId = GrpId });
            }
        }


        public ActionResult makeAdmin(int? MemberId)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }
            //getting the UserTable Information
            GroupMemberTable memInfo = dbGv.GroupMemberTables.Find(MemberId);

            if (memInfo == null)
            {
                return RedirectToAction("PageNotFound", "Home", new { msg = "Oops, It looks like it is not a valid user of this group" });
            }

            var thisGrpId = memInfo.GrpId;
            //Getting the groupTable Info from GrpId take AdminId
            GroupTable grpInfo = dbGv.GroupTables.Find(thisGrpId);
            var GrpAdminUserId = grpInfo.UserTable.Uid;

            //now check Weather the SessionId is the Admin of that group, if yes then remove user 
            if ((int)Session["Id"] == GrpAdminUserId)
            {
                if (GrpAdminUserId != memInfo.UserId)
                {
                    grpInfo.GrpAdminId = memInfo.UserId;
                    TempData["Success"] = "The Member " + memInfo.UserTable.Name + ", is now This Group Admin";
                    dbGv.SaveChanges();

                    return RedirectToAction("grpInfo", "GroupView", new { grpId = grpInfo.GrpId });
                }
                else
                {
                    TempData["Error"] = "You are already a Group Admin";
                    return RedirectToAction("grpInfo", "GroupView", new { grpId = grpInfo.GrpId });
                }
            }
            else
            {
                // Set a one time error message to display in the view
                TempData["Error"] = "Only group admins can make new Admin.";
                return RedirectToAction("grpInfo", "GroupView", new { grpId = grpInfo.GrpId });
            }
        }

        public ActionResult deleteGroup(int? GrpId)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }

            if (GrpId == null)
            {
                return new HttpStatusCodeResult(400, "Group Id is required");
            }
            //First Get GroupID
            int UserId = (int)Session["Id"];
            GroupTable grp = dbGv.GroupTables.Find(GrpId);
            if (grp == null)
            {
                return RedirectToAction("PageNotFound", "Home", new { msg = "Oops,Cant find this Group !!, Report us." });
            }
            //search the MEMBERID from SEssionId and GroupId
            GroupMemberTable memInfo = dbGv.GroupMemberTables.FirstOrDefault(a => a.UserId == UserId && a.GrpId == grp.GrpId);
            if (memInfo == null)
            {
                return RedirectToAction("PageNotFound", "Home", new { msg = "Oops,Cant find U as a member of this group!" });
            }
            var thisMemID = memInfo.MemberId;

            //if the mwmberId is not the sessionID themn give error else confirm leaveeeeeeeeeee
                if (memInfo.GroupTable.GrpAdminId == memInfo.UserTable.Uid)
                {
                    var rowsToDelete = dbGv.MeetingTables.Where(t => t.GrpId == grp.GrpId).ToList();
                    if (rowsToDelete.Count > 0)
                    {
                        dbGv.MeetingTables.RemoveRange(rowsToDelete);
                        dbGv.SaveChanges();
                    }
                    var msgsToDelete = dbGv.MessageTables.Where(t => t.GrpId == grp.GrpId).ToList();
                    if (msgsToDelete.Count > 0)
                    {
                        dbGv.MessageTables.RemoveRange(msgsToDelete);
                        dbGv.SaveChanges();
                    }

                    var filesToDelete = dbGv.FileTables.Where(t => t.GrpId == grp.GrpId).ToList();
                    
                    foreach(var file in filesToDelete)
                {
                    try
                    {
                        var FullfilePath = Path.Combine(Server.MapPath(file.FilePath));
                        if (System.IO.File.Exists(FullfilePath))
                        {
                            System.IO.File.Delete(FullfilePath);
                        }

                        dbGv.FileTables.Remove(file);
                        dbGv.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        // Handle any errors
                        return new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError, "An error occurred while deleting the file.");
                    }
                }
                    var announceToDelete = dbGv.AnnouncementTables.Where(t => t.GrpId == grp.GrpId).ToList();
                    if (announceToDelete.Count > 0)
                    {
                        dbGv.AnnouncementTables.RemoveRange(announceToDelete);
                        dbGv.SaveChanges();
                    }
                    var MemberToDelete = dbGv.GroupMemberTables.Where(t => t.GrpId == grp.GrpId).ToList();
                    if (MemberToDelete.Count > 0)
                    {
                        dbGv.GroupMemberTables.RemoveRange(MemberToDelete);
                        dbGv.SaveChanges();
                    }

                dbGv.GroupTables.Remove(grp);


                TempData["Success"] = "The Group is no Longer Exists";

                    dbGv.SaveChanges();
                    return RedirectToAction("allGroup", "Group");
                }
                else
                {
                    TempData["Error"] = "Only Admin Can Delete Groups";
                    return RedirectToAction("grpInfo", "GroupView", new { grpId = GrpId });
                }
            }
        

    }
}