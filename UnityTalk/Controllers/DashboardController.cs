using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using UnityTalk.Models;
using UnityTalk.viewModel;
using UnityTalk.vireModel;

namespace UnityTalk.Controllers
{
    public class DashboardController : Controller
    {
        UnityTalkEntities8 dbD = new UnityTalkEntities8();

        private static Random random = new Random();
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789#";

        // GET: Dashboard
        public ActionResult dashboard()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg  = "It seems You haven't sign in yet !"});
            }

            UserTable user = dbD.UserTables.Find(Session["Id"]);

            List<GroupTable> grps = dbD.GroupTables.Where(a => a.GrpMode == true).ToList();

            grps.Reverse();


            List<EbookTable> ebooks = dbD.EbookTables.ToList();
            ebooks.Reverse();
            dashboardViewModel dashboardModel = new dashboardViewModel()
            {
                User = user,
                Groups = grps.Take(5).ToList(),
                Ebooks = ebooks.Take(5).ToList(),
                ToDos = dbD.ToDoTables.Where(a => a.Uid == user.Uid).ToList()
            };

            return View(dashboardModel);
        }

        [HttpGet]
        public ActionResult createGrp()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home");
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult createGrp(GroupTable grp)
        {
            if (ModelState.IsValid)
            {
                grp.GrpAdminId = (int)Session["Id"];    // the Current User will be the Admin Automatically
                string Grpcode = GenerateRandomGrpCode();

                // Check if the generated group code already exists
                while (dbD.GroupTables.Any(a => a.GrpCode == Grpcode))
                {
                    Grpcode = GenerateRandomGrpCode();
                }
                grp.GrpImgId = 1;

                grp.GrpCode = Grpcode;

                if (grp.GrpMode == true)
                {
                    grp.GrpMode = true;
                }
                else
                {
                    grp.GrpMode = false;
                }

                dbD.GroupTables.Add(grp);
                dbD.SaveChanges();

                GroupMemberTable newgrp = new GroupMemberTable()
                {
                    UserId = grp.GrpAdminId,
                    GrpId = grp.GrpId
                };

                dbD.GroupMemberTables.Add(newgrp);
                dbD.SaveChanges();
                TempData["Success"] = "You've successfully created" + grp.GrpName + ". Lead the way and inspire collaboration!";
                TempData["grpId"] = grp.GrpId;
                ModelState.Clear();
                return View();
            }
            return View();
        }

        private string GenerateRandomGrpCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789#";
            Random random = new Random();
            StringBuilder sb = new StringBuilder(5);
            for (int i = 0; i < 5; i++)
            {
                sb.Append(chars[random.Next(chars.Length)]);
            }
            return sb.ToString();
        }


        [HttpGet]
        public ActionResult JoinGrp()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JoinGrp(GroupTable UserGrp)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }

            int id = (int)Session["Id"];

            GroupTable grp = dbD.GroupTables.Where(a => a.GrpCode == UserGrp.GrpCode).FirstOrDefault();
            if (grp == null)
            {
                ViewBag.notify = "Prease Enter Correct Group Code";
                return View();
            }
            else
            {
                if (dbD.GroupMemberTables.Any(a => a.UserId == id && a.GrpId == grp.GrpId))
                {
                    ViewBag.notify = "U are Already A member of this group";
                    return View();
                }
                else
                {
                    GroupMemberTable member = new GroupMemberTable()
                    {
                        UserId = id,
                        GrpId = grp.GrpId
                    };

                    dbD.GroupMemberTables.Add(member);
                    dbD.SaveChanges();
                    TempData["Success"] = "You're now part of "+ grp.GrpName + "'s dynamic community. Let's achieve greatness together!";
                    TempData["grpId"] = grp.GrpId;
                    ModelState.Clear();
                    return View();
                }
            }
        }

        public ActionResult viewProfile(int? UserId)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }
            else
            {
                if (UserId == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                UserTable user = dbD.UserTables.Find(UserId);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
        }

        public ActionResult profileEdit(int? Usrid)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }
            else
            {
                if (Usrid == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                UserTable user = dbD.UserTables.Find(Usrid);
                if (user == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    UserProfileViewModel tempUser = new UserProfileViewModel();
                    tempUser.Uid = user.Uid;
                    tempUser.Email = user.Email;
                    tempUser.Name = user.Name;
                    tempUser.UserName = user.UserName;
                    tempUser.UserStatus = user.UserStatus;
                    tempUser.UserImgPath = user.UserImgPath;
                    return View(tempUser);
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult profileEdit(UserProfileViewModel tempModel)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }
            else
            {
                if (ModelState.IsValid)
                {

                    if (tempModel.UserImg != null) {
                        string fileName = Path.GetFileNameWithoutExtension(tempModel.UserImg.FileName);
                        string extention = Path.GetExtension(tempModel.UserImg.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extention; 
                        tempModel.UserImgPath = "~/Content/assets/Images/" + fileName;

                        fileName = Path.Combine(Server.MapPath("~/Content/assets/Images/"), fileName);

                        tempModel.UserImg.SaveAs(fileName);
                    }

                    UserTable user = dbD.UserTables.Find(tempModel.Uid);
                    user.Email = tempModel.Email;
                    user.Name = tempModel.Name;
                    user.UserName = tempModel.UserName;
                    user.UserStatus = tempModel.UserStatus;
                    user.UserImgPath = tempModel.UserImgPath;
                    Session["Img"] = tempModel.UserImgPath;
                    user.Pwd = user.Pwd;
                    user.rePwd = user.Pwd;
                    dbD.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    dbD.SaveChanges();
                    return RedirectToAction("viewProfile", "Dashboard", new { UserId = user.Uid });
                }
            }
            TempData["Success"] = "Your Account has been Successfully Updated!!";
            return View();
        }


        public ActionResult JoinGrpBtn(int? grpId)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }
            if (grpId == null)
            {
                return RedirectToAction("PageNotFound", "Home", new { msg = "Oops ! Can not Find the Page" });
            }
            else
            {
               GroupTable grp = dbD.GroupTables.Find(grpId);
               if (grp != null && grp.GrpMode)
               {
                    UserTable user = dbD.UserTables.Find((int)Session["Id"]);
                    GroupMemberTable ismem = dbD.GroupMemberTables.Where(a => a.GrpId == grp.GrpId && a.UserId == user.Uid).FirstOrDefault();

                    if (ismem != null)
                    {
                        TempData["Success"] = "You're already a part of " + grp.GrpName + " .";
                        return RedirectToAction("grupview", "GroupView", new {grpid = grp.GrpId});
                    }
                    else
                    {
                        GroupMemberTable mem = new GroupMemberTable()
                        {
                            UserId = user.Uid,
                            GrpId = grp.GrpId
                        };
                        dbD.GroupMemberTables.Add(mem);
                        dbD.SaveChanges();
                        TempData["Success"] = "You're now part of "+ grp.GrpName + "'s dynamic community. Let's achieve greatness together!";
                        TempData["grpId"] = grp.GrpId;
                        return RedirectToAction("joinGrp", "Dashboard");
                    }

               }
                else
                {
                    return RedirectToAction("PageNotFound", "Home", new { msg = "Oops!Something Is wrong, Try again Later" });
                }

            }
        }


        public ActionResult whiteBoard()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }
            return View();
        }
    }
}