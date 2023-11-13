using NovelHub.App_Start;
using NovelHub.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NovelHub.Areas.Admin.Controllers
{
    public class AdminNovelsController : Controller
    {
        private NovelHubEntities db = new NovelHubEntities();

        [AdminAuthorize]
        public ActionResult ManagerNovels(int? page)
        {
            if(page == null) page = 1;
            // truy van list
            var AllNovels = db.Novels.ToList();
            int pageSize = 10;
            
            ViewBag.pageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.CountNovels = AllNovels.Count();

            int pageNumber = (page ?? 1);
            return View(AllNovels.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorize]
        public ActionResult AddBackListNovel([Bind(Include = "BlacklistedNovelID,NovelID,CreatedAt,Note")] BlacklistedNovel blacklistedNovel)
        {
            blacklistedNovel.CreatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.BlacklistedNovels.Add(blacklistedNovel);
                db.SaveChanges();
                Toast(true, "Truyện đã được khóa");
                return RedirectToAction("ManagerNovels");
            }
            return View(blacklistedNovel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorize]
        public ActionResult RemoveBackListNovel(int novelID)
        {
            var blackNovel = db.BlacklistedNovels.SingleOrDefault(n=>n.NovelID == novelID);
            db.BlacklistedNovels.Remove(blackNovel);
            db.SaveChanges();
            Toast(false, "Truyện đã được duyệt");
            return RedirectToAction("ManagerNovels");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorize]
        public ActionResult DeleteNovel(int? id)
        {
            var deleteNovel = db.Novels.Find(id);
            if (deleteNovel.Poster != null)
            {
                var relativeImagePath = "~/Areas/Author/Contents/img/Poster/" + deleteNovel.Poster;
                var serverPath = Server.MapPath(relativeImagePath);
                System.IO.File.Delete(serverPath);
            }
            db.Novels.Remove(deleteNovel);
            db.SaveChanges();
            Toast(false, "Đã xóa thành công");
            return RedirectToAction("ManagerNovels");
        }
        public void Toast(bool isError, string contentToast){
            TempData["ActiveToast"] = true;
            if(isError)
            {
                TempData["isError"] = true;
                TempData["Color"] = "#dc3545";
                TempData["ToastContent"] = contentToast;
            }
            else
            {
                TempData["isError"] = false;
                TempData["Color"] = "#00dc82";
                TempData["ToastContent"] = contentToast;
            }
        }
    }
}