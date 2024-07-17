using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using UnityTalk.Models;
using UnityTalk.viewModel;


namespace UnityTalk.Controllers
{
    public class FileController : Controller
    {
        UnityTalkEntities8 dbF = new UnityTalkEntities8();
        public ActionResult fileView(int? grpId)
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

            GroupTable grp = dbF.GroupTables.Find(grpId);

            if (grp == null)
            {
                return RedirectToAction("PageNotFound", "Home", new { msg = "Oops,Cant find this Group !!, Report us." });
            }

            UserTable user = dbF.UserTables.Find(UserId);

            GroupMemberTable mem = dbF.GroupMemberTables.SingleOrDefault(item => item.UserId == user.Uid && item.GrpId == grp.GrpId);
            //checking wheather the user Is Member of the group or not 
            if (mem == null)
            {
                return RedirectToAction("PageNotFound", "Home", new { msg = "Oops, It looks like you are not a valid user of this group" });
            }

            List<FileTable> files = dbF.FileTables.Where(a => a.GrpId == grp.GrpId).ToList();
            files.Reverse();
            GroupView groupView = new GroupView(); 
            groupView.FileTbl = files;
            groupView.Group = grp;

            return View(groupView);
        }

        [HttpPost]
        public ActionResult fileView(HttpPostedFileBase file, int? grpId,string filename)
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

            GroupTable grp = dbF.GroupTables.Find(grpId);
            if (grp == null)
            {
                return RedirectToAction("PageNotFound", "Home", new { msg = "Oops,Cant find this Group !!, Report us." });
            }
            UserTable user = dbF.UserTables.Find(UserId);

            GroupMemberTable mem = dbF.GroupMemberTables.SingleOrDefault(item => item.UserId == user.Uid && item.GrpId == grp.GrpId);
            
            if (mem == null)
            {
                return RedirectToAction("MemberNotFound", "Home", new { msg = "Oops, It looks like you are not a valid user of this group" });
            }
            //File Upload Method Main PArt

            if (file != null && file.ContentLength > 0)
            {
                if(string.IsNullOrWhiteSpace(filename))
                {
                   filename = Path.GetFileNameWithoutExtension(file.FileName);
                }

                var fileName = Path.GetFileNameWithoutExtension(file.FileName);

                var FullfileName = fileName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/assets/GroupFiles"), FullfileName);

                file.SaveAs(path);

                // Save file information to the database
                FileTable newFile = new FileTable
                {
                    FileName = filename,
                    SenderId = mem.MemberId,
                    FileType = file.ContentType,/* File type */
                    Date = DateTime.Now,
                    Time =  DateTime.Now.TimeOfDay,
                    GrpId = grp.GrpId,
                    FilePath = "~/Content/assets/GroupFiles/" + FullfileName,
                };

                dbF.FileTables.Add(newFile);
                dbF.SaveChanges();
                TempData["Success"] = "File Uploaded Successfully";
                ModelState.Clear();
            }
            else
            {
                TempData["Error"] = "Please Give a valid file to Upload!!";
            }
            List<FileTable> files = dbF.FileTables.Where(a => a.GrpId == grp.GrpId).ToList();
            files.Reverse();
            GroupView groupView = new GroupView();
            groupView.FileTbl = files;
            groupView.Group = grp;
            return RedirectToAction("fileView", "File", new { grpId = grp.GrpId });
        }

        public ActionResult deleteFile(int? fileId,int? grpId)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }
            if (fileId == null)
            {
                return new HttpStatusCodeResult(400, "File Id is required");
            }
            int UserId = (int)Session["Id"];
            UserTable user = dbF.UserTables.Find(UserId);

            GroupTable grp = dbF.GroupTables.Find(grpId);

            if (grp == null)
            {
                return RedirectToAction("MemberNotFound", "Home", new { msg = "Oops, It looks like you are not a valid user of this group" });
            }

            FileTable file = dbF.FileTables.Find(fileId);
            if (file == null)
            {
                List<FileTable> Grpfiles = dbF.FileTables.Where(a => a.GrpId == grp.GrpId).ToList();

                GroupView groupViews = new GroupView();
                groupViews.FileTbl = Grpfiles;
                groupViews.Group = grp;

                return View("fileView", groupViews);
            }

            GroupMemberTable mem = dbF.GroupMemberTables.SingleOrDefault(item => item.UserId == user.Uid && item.GrpId == grp.GrpId);

            if (mem == null)
            {
                return RedirectToAction("MemberNotFound", "Home", new { msg = "Oops, It looks like you are not a valid user of this group" });
            }

            if(user.Uid == grp.UserTable.Uid || file.GroupMemberTable.UserTable.Uid == user.Uid)
            {
                try
                {
                    var FullfilePath = Path.Combine(Server.MapPath(file.FilePath));
                    if (System.IO.File.Exists(FullfilePath))
                    {
                        System.IO.File.Delete(FullfilePath);
                    }

                    dbF.FileTables.Remove(file);
                    dbF.SaveChanges();
                    TempData["Success"] = "File removed Successfully";
                }
                catch (Exception ex)
                {
                    // Handle any errors
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError, "An error occurred while deleting the file.");
                }
            }
            else
            {
                TempData["Error"] = "Only Admin and Sender Can remove files!!";
            }

            List<FileTable> files = dbF.FileTables.Where(a => a.GrpId == grp.GrpId).ToList();
            files.Reverse();
            GroupView groupView = new GroupView();
            groupView.FileTbl = files;
            groupView.Group = grp;

            return View("fileView", groupView);
        }

    }
}