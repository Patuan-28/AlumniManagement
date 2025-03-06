using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ExamWeb.AlumniImageService;
using ExamWeb.Interfaces;
using ExamWeb.Models;
using ExamWeb.Services;

namespace ExamWeb.Controllers
{
    [Authorize]
    public class AlumniImageController : Controller
    {
        public IAlumniImageRepository _alumniImageRepository;
        public IAlumniRepository _alumniRepository;
        public int fileSizeLimit = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["FileSizeLimit"]); // 10MB
        public string fileTypes = System.Configuration.ConfigurationManager.AppSettings["FileTypes"]; // "jpeg, jpg, png"
        public string alumniImagesPath = System.Configuration.ConfigurationManager.AppSettings["AlumniImagesPath"]; // "~/images/AlumniImages/"

        public AlumniImageController() : this(new AlumniImageRepository(), new AlumniRepository())
        {
        }

        public AlumniImageController(AlumniImageRepository alumniImageRepository, AlumniRepository alumniRepository)
        {
            _alumniImageRepository = alumniImageRepository;
            _alumniRepository = alumniRepository;
        }

        // GET: AlumniImage
        public ActionResult Index(int alumniID)
        {
            ViewBag.AlumniID = alumniID;
            var alumniImages = _alumniImageRepository.GetAllImages(alumniID); 
            var fullName = _alumniRepository.GetAlumniByID(alumniID).FullNames;
            ViewBag.FullName = fullName;
            return View(alumniImages);
        }

        // POST: AlumniImage/Create
        [HttpPost]
        public async Task<ActionResult> UploadImages(int alumniID, HttpPostedFileBase[] files)
        {
            try
            {
                if (files == null || files.Length == 0)
                {
                    TempData["ErrorMessage"] = "Please upload at least one valid file.";
                    return RedirectToAction("Index", new { alumniID = alumniID });
                }

                var alumniImage = new List<AlumniImageModel>();

                foreach (var file in files)
                {
                    if (file.ContentLength > fileSizeLimit) // 10 MB
                    {
                        TempData["ErrorMessage"] = "File size must be less than 10 MB.";
                        return RedirectToAction("Index", new { alumniID = alumniID });
                    }

                    var extension = Path.GetExtension(file.FileName).ToLower();
                    if (extension != ".jpeg" && extension != ".png" && extension != ".jpg")
                    {
                        TempData["ErrorMessage"] = "Only .jpeg, .jpg and .png files are allowed.";
                        return RedirectToAction("Index", new { alumniID = alumniID });
                    }

                    var fileName = Guid.NewGuid().ToString() + extension;
                    var filePath = Path.Combine(Server.MapPath(alumniImagesPath), fileName);

                    file.SaveAs(filePath);  // Using SaveAs instead of CopyToAsync

                    var image = new AlumniImageModel
                    {
                        AlumniID = alumniID,
                        ImagePath = alumniImagesPath,
                        FileName = fileName,
                        UploadDate = DateTime.Now
                    };
                    alumniImage.Add(image);
                }

                await _alumniImageRepository.AddImage(alumniImage);
                TempData["SuccessMessage"] = "Images uploaded successfully.";
                return RedirectToAction("Index", new { alumniID = alumniID });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while uploading the images: " + ex.Message;
                return RedirectToAction("Index", new { alumniID = alumniID });
            }
        }

        // POST: AlumniImage/Delete/5
        [HttpPost]
        public async Task<ActionResult> DeleteImage(int id, int alumniID)
        {
            try
            {
                var image = _alumniImageRepository.GetImageByID(id, alumniID);
                if (image == null)
                {
                    ModelState.AddModelError("", "Image not found.");
                    return RedirectToAction("Index", new { alumniID = alumniID });
                }
                else
                {
                    var filePath = Path.Combine(Server.MapPath(alumniImagesPath), image.FileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    await _alumniImageRepository.DeleteImageByID(id, alumniID);
                    return Json(new { success = true, message = "Image deleted successfully." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error deleting image: " + ex.Message });
            }
        }

        public async Task<ActionResult> DeleteSelectedImages(int alumniID, List<int> selectedImages)
        {
            try
            {
                if (selectedImages == null || selectedImages.Count == 0)
                {
                    ModelState.AddModelError("", "Please select at least one image to delete.");
                    return RedirectToAction("Index", new { alumniID = alumniID });
                }
                foreach (var imageId in selectedImages)
                {
                    await _alumniImageRepository.DeleteImageByID(imageId, alumniID);
                }
                return Json(new { success = true, message = "Image deleted successfully." });
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("", "Error deleting image due to " + ex.Message);
                return Json(new { success = false, message = "Error deleting image: " + ex.Message });
            }
            //return RedirectToAction("Index", new { alumniID = alumniID });
        }
    }
}
