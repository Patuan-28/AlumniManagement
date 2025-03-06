using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExamWeb.Interfaces;
using ExamWeb.Models;
using ExamWeb.Services;
using Microsoft.Ajax.Utilities;

namespace ExamWeb.Controllers
{
    [Authorize]
    public class PhotoController : Controller
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IPhotoAlbumRepository _photoAlbumRepository;
        public int photoSizeLimit = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AlumniPhotoSizeLimit"]); // 3MB
        public string fileTypes = System.Configuration.ConfigurationManager.AppSettings["FileTypes"]; // "jpeg, jpg, png"
        public string uploadPhotoPath = System.Configuration.ConfigurationManager.AppSettings["PhotoPath"];

        public PhotoController()
        {
            _photoRepository = new PhotoRepository();
            _photoAlbumRepository = new PhotoAlbumRepository();
        }

        // GET: Photo
        [Authorize(Roles = "Superadmin")]
        public ActionResult Index(int albumID)
        {
            ViewBag.AlbumId = albumID;
            var album = _photoAlbumRepository.GetPhotoAlbumById(albumID);
            ViewBag.AlbumName = album.AlbumName;
            var photos = _photoRepository.GetPhotos(albumID);
            return View(photos);
        }

        public JsonResult GetPhotos(int albumID)
        {
            var photos = _photoRepository.GetPhotos(albumID);
            return Json(photos, JsonRequestBehavior.AllowGet);
        }

        //Post set thumbnail
        [HttpPost]
        public JsonResult SetThumbnail(int id, int albumID)
        {
            //throw the new id to become the next thumbnail
            _photoRepository.SetThumbnail(id, albumID);
            return Json(new { success = true, message = "Thumbnail has been set successfully" });
        }

        // POST: Photo/Create
        [HttpPost]
        public ActionResult Create(PhotoModel photo, int AlbumID)
        {
            try
            {
                // Validasi model
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Invalid data." });
                }

                // Cek apakah ada file yang diupload
                if (Request.Files.Count == 0 || Request.Files["PhotoFile"] == null)
                {
                    return Json(new { success = false, message = "Please upload a photo." });
                }

                var file = Request.Files["PhotoFile"];

                // Validasi ukuran file
                if (file.ContentLength > photoSizeLimit)
                {
                    return Json(new
                    {
                        success = false,
                        message = $"File size must be less than {photoSizeLimit / 1024 / 1024} MB."
                    });
                }

                // Validasi ekstensi file
                var allowedExtensions = fileTypes.Split(',')
                                                 .Select(e => e.Trim().ToLower())
                                                 .ToList();
                var extension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    return Json(new
                    {
                        success = false,
                        message = $"Only {string.Join(", ", allowedExtensions)} files are allowed."
                    });
                }

                // Generate nama file unik
                var fileName = $"{Guid.NewGuid()}{extension}";
                var serverPath = Server.MapPath(uploadPhotoPath);

                // Pastikan direktori penyimpanan ada
                if (!Directory.Exists(serverPath))
                {
                    Directory.CreateDirectory(serverPath);
                }

                // Simpan file ke server
                var filePath = Path.Combine(serverPath, fileName);
                file.SaveAs(filePath);

                // Simpan informasi file ke dalam database
                photo.PhotoPath = uploadPhotoPath;
                photo.PhotoFileName = fileName;
                photo.ModifiedDate = DateTime.Now;
                photo.AlbumID = AlbumID;
                photo.IsPhotoAlbumThumbnail = false;

                _photoRepository.InsertPhoto(photo, AlbumID);

                return Json(new { success = true, message = "Photo added successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        // POST: Photo/Delete/5
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
                        
                            _photoRepository.DeletePhoto(itemID);
                            count += 1;
                        
                    }
                    return Json(new { success = true, message = "Photos have been deleted successfully" });
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
