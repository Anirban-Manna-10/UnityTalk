using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnityTalk.Models;

namespace VirtualMeeting.Controllers
{
    public class ImageController : Controller
    {
        UnityTalkEntities8 ut3 = new UnityTalkEntities8();
        
        public ActionResult UploadImage()
        {
            if (Session["fit"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }
            return View();
        }


        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file, GroupImgTable image)
        {

            if (file != null && file.ContentLength > 0)
            {
          
                string fileName = Path.GetFileNameWithoutExtension(file.FileName);

               

                fileName = fileName + DateTime.Now.ToString("YYmmssff") + Path.GetExtension(file.FileName);

                var savePath = Path.Combine(Server.MapPath("~/Content/assets/GroupImages"), fileName);

                file.SaveAs(savePath);

                image.GrpImgPath = $"/Content/assets/GroupImages/{fileName}";


                 
                ut3.GroupImgTables.Add(image);
                ut3.SaveChanges();

                ViewBag.Mssg1 = "Image has been uploaded!";

            }
            else
            {
                ViewBag.Mssg2 = "Image is not uploaded!";

            }

            return View();
        }

        public ActionResult ViewImage()
        {
            return View(ut3.GroupImgTables.ToList());
        }

        public ActionResult Search(string search)
        {
            return View(ut3.GroupImgTables.Where(x => x.ImgName.Contains(search) || search == null).ToList());
        }
        

        public ActionResult Delete(int? id)
        {   
            if (id == null)
            {
                ViewBag.not = "Id cant be null";
            }
            else
            {
                GroupImgTable grpImg = ut3.GroupImgTables.Find(id);
                string fullPath = grpImg.GrpImgPath;
                fullPath = Path.Combine(Server.MapPath(fullPath));
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);

                }
                else
                {
                    ViewBag.Message = "File not found.";
                }
            }

            //string fullPath = $"~{ebookId.Path}";

            //string coverPath = ebookId.CoverPath;

            //coverPath = Path.Combine(Server.MapPath(coverPath));
            GroupImgTable grpImgId = ut3.GroupImgTables.Find(id);

            ut3.GroupImgTables.Remove(grpImgId);
            ut3.SaveChanges();


            return RedirectToAction("ViewImage");

        }



    }
}