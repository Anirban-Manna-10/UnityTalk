using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnityTalk.Models;

namespace UnityTalk.Controllers
{
    public class HomeController : Controller
    {
        UnityTalkEntities8 db = new UnityTalkEntities8();       
        public ActionResult Index()
        {

            if (Session["Id"] != null)
            {
                int userId = (int)Session["Id"];
                UserTable user = db.UserTables.Find(userId);
                return View(user);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult signUp() 
        { 
            return View();
        }

        [HttpPost]
        public ActionResult signUp(UserTable user) 
        {
            if(ModelState.IsValid)
            {
                if (db.UserTables.Any(a => a.UserName == user.UserName))
                {

                    ModelState.Clear();
                    TempData["Error"] = "User name already taken,Plese choose another.";
                    return View();
                }
                else
                {
                    user.UserStatus = true;
                    user.UserImgPath = "/Content/assets/IndexImages/userPrimaryImg.png";

                    db.UserTables.Add(user);
                    db.SaveChanges();
                    TempData["Success"] = "Welcome! Explore and enjoy. We're here to assist you anytime.";

                    Session["Id"] = (int)user.Uid;
                    Session["UserName"] = user.UserName.ToString();
                    Session["Name"] = user.Name.ToString();
                    Session["Img"] = user.UserImgPath.ToString();
                    return RedirectToAction("dashboard", "Dashboard");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult login(UserTable user)
        {
            ModelState.Remove("Name");
            ModelState.Remove("rePwd");
            ModelState.Remove("Email");

            if (ModelState.IsValid)
            {
                var checkLogin = db.UserTables.Where(a => a.UserName.Equals(user.UserName) && a.Pwd.Equals(user.Pwd)).FirstOrDefault();
                if (checkLogin != null)
                {
                    checkLogin.UserStatus = true;
                    checkLogin.Pwd = checkLogin.Pwd;
                    checkLogin.rePwd = checkLogin.Pwd;
                    db.Entry(checkLogin).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    Session["UserName"] = checkLogin.UserName.ToString();
                    Session["Name"] = checkLogin.Name.ToString();
                    Session["Id"] = checkLogin.Uid;
                    Session["Img"] = checkLogin.UserImgPath.ToString();
                    TempData["Success"] = "Welcome,"+ checkLogin.Name+ " Explore and enjoy. We're here to assist you anytime.\".";
                    return RedirectToAction("dashboard", "dashboard");
                }
                else
                {
                    TempData["Error"] = "Invalid User Name or Password";
                    ModelState.Clear();
                    return View();
                }
            }
            return View();
        }

        public ActionResult logout()
        {
            int userId = (int)Session["Id"];
            UserTable user = db.UserTables.Find(userId);
            user.UserStatus = false;
            user.Pwd = user.Pwd;
            user.rePwd = user.Pwd;
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            Session.Clear();
            
            return RedirectToAction("Index", "Home");
        }

        public ActionResult UserNotFound(string msg)
        {
            ViewBag.ErrorMsg = msg;
            return View();
        }
        public ActionResult PageNotFound(string msg)
        {
            ViewBag.ErrorMsg = msg;
            return View();
        }
    }
}