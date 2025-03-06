using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExamWeb.Interfaces;
using ExamWeb.Models;
using ExamWeb.PhotoAlbumService;
using ExamWeb.Services;

namespace ExamWeb.Controllers
{
    [Authorize]
    public class PhotoAlbumController : Controller
    {
        private readonly IPhotoAlbumRepository _photoAlbumRepository;
        private readonly IPhotoRepository _photoRepository;

        public PhotoAlbumController()
        {
            _photoAlbumRepository = new PhotoAlbumRepository();
            _photoRepository = new PhotoRepository();
        }

        // GET: PhotoAlbum
        public ActionResult Index()
        {
            // Ambil data album dalam bentuk DTO
            var photoAlbumDTOs = _photoAlbumRepository.GetPhotoAlbums();

            // Ambil semua foto yang memiliki IsPhotoAlbumThumbnail == true
            var thumbnailPhotos = _photoRepository.GetAllPhotos()
                .Where(photo => photo.IsPhotoAlbumThumbnail) // Hanya thumbnail
                .ToList();

            // Mapping DTO ke Model dan ambil thumbnail jika ada
            var photoAlbumModels = photoAlbumDTOs.Select(dto => new PhotoAlbumModel
            {
                AlbumID = dto.AlbumID,
                AlbumName = dto.AlbumName,
                ModifiedDate = dto.ModifiedDate,
                ThumbnailPhotoPath = thumbnailPhotos.FirstOrDefault(p => p.AlbumID == dto.AlbumID)?.PhotoPath, // Ambil foto thumbnail berdasarkan AlbumID
                ThumbnailPhotoName = thumbnailPhotos.FirstOrDefault(p => p.AlbumID == dto.AlbumID)?.PhotoFileName // Ambil nama foto thumbnail berdasarkan AlbumID

            }).ToList();

            // ViewBag untuk dropdown
            ViewBag.AlbumList = photoAlbumDTOs
                .Select(p => new SelectListItem
                {
                    Value = p.AlbumID.ToString(),
                    Text = p.AlbumName
                }).ToList();

            return View(photoAlbumModels);
        }

        public JsonResult GetPhotoAlbums()
        {
            var photoAlbumsData = _photoAlbumRepository.GetPhotoAlbums();
            //var json = Json(new { data = photoAlbumsData }, JsonRequestBehavior.AllowGet);
            //json.MaxJsonLength = int.MaxValue;
            //return json;
            var photoAlbumModels = photoAlbumsData.Select(dto => new PhotoAlbumModel
            {
                AlbumID = dto.AlbumID,
                AlbumName = dto.AlbumName,
                ModifiedDate = dto.ModifiedDate
            }).ToList();

            return Json(photoAlbumModels);
        }

        public JsonResult GetPhotoAlbumById(int id)
        {
            var data = _photoAlbumRepository.GetPhotoAlbumById(id);
            var result = Mapping.Mapper.Map<PhotoAlbumModel>(data);
            if (data == null)
            {
                return Json(new { error = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpsertPhotoAlbum(PhotoAlbumModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _photoAlbumRepository.UpsertPhotoAlbum(model);
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }

                return View("Index", model); // Jika form tidak valid, kembali ke halaman Index dengan form
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }            
        }


        // GET: PhotoAlbum/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PhotoAlbum/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: PhotoAlbum/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var existingData = _photoAlbumRepository.GetPhotoAlbumById(id);
                if (existingData == null)
                {
                    return HttpNotFound();
                }

                _photoAlbumRepository.DeletePhotoAlbum(id);
                return Json(new { success = true, message = "Album deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error deleting album: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult DeleteSelected(int[] ids)
        {
            try
            {
                if (ids != null && ids.Length > 0)
                {
                    int count = 0;
                    foreach (var itemID in ids)
                    {
                        var existingData = _photoAlbumRepository.GetPhotoAlbumById(itemID);
                        if (existingData != null)
                        {
                            _photoAlbumRepository.DeletePhotoAlbum(itemID);
                            count += 1;
                        }
                    }
                    return Json(new { success = true, message = count + " items have been deleted successfully" });
                }
                return Json(new { success = false, message = "No items selected for deletion" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error deleting items: " + ex.Message });
            }
        }
    }
}
