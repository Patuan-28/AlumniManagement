using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExamWeb.Interfaces;
using ExamWeb.Models;
using ExamWeb.Services;

namespace ExamWeb.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private IEventRepository _eventRepository;
        public int photoSizeLimit = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AlumniPhotoSizeLimit"]); // 3MB
        public string fileTypes = System.Configuration.ConfigurationManager.AppSettings["FileTypes"]; // "jpeg, jpg, png"
        public string eventImagesPath = System.Configuration.ConfigurationManager.AppSettings["EventImagesPath"];
        public EventController()
        {
            _eventRepository = new EventRepository();
        }

        // GET: Event
        public JsonResult GetEvents()
        {
            var eventsData = _eventRepository.GetEvents();
            var json = Json(new { data = eventsData }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public JsonResult GetEventByID(int id)
        {
            var data = _eventRepository.GetEventByID(id);
            var result = Mapping.Mapper.Map<EventModel>(data);
            if (data == null)
            {
                return Json(new { error = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return View();
        }

        // GET: Event/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Event/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Event/Create
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

        // GET: Event/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Event/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Event/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Event/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var existingData = _eventRepository.GetEventByID(id);
                if (existingData == null)
                {
                    return HttpNotFound();
                }

                // Delete the profile image if it exists
                if (!string.IsNullOrEmpty(existingData.EventImageName))
                {
                    var filePath = Path.Combine(Server.MapPath(eventImagesPath), existingData.EventImageName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _eventRepository.DeleteEvent(id);
                return Json(new { success = true, message = "Event deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error deleting event: " + ex.Message });
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
                        var existingData = _eventRepository.GetEventByID(itemID);
                        if (existingData != null)
                        {
                            // Delete the profile image if it exists
                            if (!string.IsNullOrEmpty(existingData.EventImageName))
                            {
                                var filePath = Path.Combine(Server.MapPath(eventImagesPath), existingData.EventImageName);
                                if (System.IO.File.Exists(filePath))
                                {
                                    System.IO.File.Delete(filePath);
                                }
                            }

                            _eventRepository.DeleteEvent(itemID);
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

        [HttpPost]
        public ActionResult UpsertEvent(HttpPostedFileBase file, EventModel eventModel)
        {
            try
            {
                // Remove validation for image fields if they are not provided by the user
                ModelState.Remove("EventImagePath");
                ModelState.Remove("EventImageName");

                if (file == null || file.ContentLength == 0) {
                    ModelState.AddModelError("file","Image is Required");
                }

                if (ModelState.IsValid)
                {
                    // If it's a new event, eventModel.EventID should be 0.
                    if (file == null || file.ContentLength == 0)
                    {
                        // For a new event, a photo is required
                        if (eventModel.EventID == 0)
                        {
                            return Json(new { error = true, errorMsg = "A photo is required." });
                        }
                        else
                        {
                            // For an update, keep the existing image
                            var existingEvent = _eventRepository.GetEventByID(eventModel.EventID);
                            if (existingEvent != null)
                            {
                                eventModel.EventImagePath = existingEvent.EventImagePath;
                                eventModel.EventImageName = existingEvent.EventImageName;
                            }
                        }
                    }
                    else
                    {
                        // Validate file type
                        var allowedExtensions = fileTypes.Split(',')
                                                         .Select(e => e.Trim().ToLower())
                                                         .ToList();

                        var extension = Path.GetExtension(file.FileName).ToLower();
                        if (!allowedExtensions.Contains(extension))
                        {
                            return Json(new { error = true, errorMsg = $"Only {string.Join(", ", allowedExtensions)} files are allowed." });
                        }

                        // Validate file size
                        if (file.ContentLength > photoSizeLimit)
                        {
                            return Json(new { error = true, errorMsg = $"File size must be less than {photoSizeLimit / 1024 / 1024} MB." });
                        }

                        // Generate unique file name and save new file
                        var fileName = Guid.NewGuid().ToString() + extension;
                        var filePath = Path.Combine(Server.MapPath(eventImagesPath), fileName);
                        file.SaveAs(filePath);

                        // Update eventModel with new image details
                        eventModel.EventImagePath = eventImagesPath;
                        eventModel.EventImageName = fileName;
                    }

                    eventModel.ModifiedDate = DateTime.Now;

                    _eventRepository.UpsertEvent(eventModel);
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { error = true });
            }
            catch (Exception ex)
            {
                return Json(new { error = true, errorMsg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}