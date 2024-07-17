using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnityTalk.Models;

namespace VirtualMeeting.Controllers
{
    public class ProductController : Controller
    {
        UnityTalkEntities8 ut1 = new UnityTalkEntities8();
        // GET: Product

        public ActionResult ViewEbook()
        {
            return View(ut1.EbookTables.ToList());
        }

        

        public ActionResult Search(string searchby,string search)
        {
            if(searchby == "Author")
            {
                return View(ut1.EbookTables.Where(x => x.Author.StartsWith(search) || search == null).ToList());
            }
            else
            {
                return View(ut1.EbookTables.Where(x => x.EbookName.StartsWith(search) || search == null).ToList());
            }
        }

        

        public ActionResult UploadBook()
        {
            if (Session["fit"] == null)
            {
                return RedirectToAction("UserNotFound", "Home", new { msg = "It seems You haven't sign in yet !" });
            }
            return View();
        }

        // POST: Ebook/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadBook(HttpPostedFileBase ebookfile, HttpPostedFileBase coverfile, EbookTable ebook)
        {
            if(ModelState.IsValid)
            {

                if (ebookfile != null && ebookfile.ContentLength > 0)
                {

                    string ebookfileName = Path.GetFileNameWithoutExtension(ebookfile.FileName);



                    ebookfileName = ebookfileName + DateTime.Now.ToString("YYmmssff") + Path.GetExtension(ebookfile.FileName);

                    var savePath1 = Path.Combine(Server.MapPath("~/Content/assets/Ebooks"), ebookfileName);

                    ebookfile.SaveAs(savePath1);

                    ebook.EbookPath = $"/Content/assets/Ebooks/{ebookfileName}";


                    if (coverfile != null && coverfile.ContentLength > 0)
                    {

                        string coverfileName = Path.GetFileNameWithoutExtension(coverfile.FileName);


                        string coverfileext = Path.GetExtension(coverfile.FileName);
                        coverfileName = coverfileName + DateTime.Now.ToString("YYmmssff") + coverfileext;


                        var savePath2 = Path.Combine(Server.MapPath("~/Content/assets/EbookCover"), coverfileName);

                        coverfile.SaveAs(savePath2);

                        ebook.EbookImgPath = $"~/Content/assets/EbookCover/{coverfileName}";



                        ut1.EbookTables.Add(ebook);

                        ut1.SaveChanges();

                        ViewBag.Mssg1 = "Files have been Uploaded Successfully!";

                    }
                    else
                    {
                        ViewBag.Mssg2 = "CoverFile is not uploaded!";

                    }

                }
                else
                {
                    ViewBag.Mssg3 = "No Files have been Uploaded!";
                }
                ModelState.Clear();
                return View();
            }
            //ut1.EbookTables.Add(ebook);
            //ut1.SaveChanges();

            //ViewBag.Message = "File has been Uploaded Successfully!";
            return View();
        }



        public ActionResult Delete(int? id,string path) 
        {
            EbookTable ebookId = ut1.EbookTables.Find(id);
            //string fullPath = $"~{ebookId.Path}";
            string fullPath = ebookId.EbookPath;
            string coverPath = ebookId.EbookImgPath;
            fullPath = Path.Combine(Server.MapPath(fullPath));
            coverPath = Path.Combine(Server.MapPath(coverPath));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
                if (System.IO.File.Exists(coverPath))
                {
                    System.IO.File.Delete(coverPath);
                }
            }
            else
            {
                ViewBag.Message = "File not found.";
            }
           
            ut1.EbookTables.Remove(ebookId);
            ut1.SaveChanges();
            

            return RedirectToAction("ViewEbook");

        }



        
    }
    
}


/*if (coverfile != null && coverfile.ContentLength > 0)
{
    string coverfileName = Path.GetFileNameWithoutExtension(coverfile.FileName);



    coverfileName = coverfileName + DateTime.Now.ToString("YYmmssff") + Path.GetExtension(coverfile.FileName);

    var savePath2 = Path.Combine(Server.MapPath("~/Content/EbookCovers"), coverfileName);

    coverfile.SaveAs(savePath2);

    ebook.EbookImgPath = $"/Content/EbookCovers/{coverfileName}";



    ut1.EbookTables.Add(ebook);
    ut1.SaveChanges();
    ViewBag.Message1 = "File has been Uploaded Successfully!";*/