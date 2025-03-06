using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExamWeb.Interfaces;
using ExamWeb.MajorService;
using ExamWeb.Models;
using ExamWeb.Services;

namespace ExamWeb.Controllers
{
    [Authorize]
    public class MajorController : Controller
    {
        private IMajorRepository _majorRepository;
        private IFacultyRepository _facultyRepository;
        public MajorController()
        {
            _majorRepository = new MajorRepository();
            _facultyRepository = new FacultyRepository();
        }
        // GET: Major
        public JsonResult GetMajors()
        {
            var majorsData = _majorRepository.GetMajors();
            var json = Json(new { data = majorsData }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public JsonResult GetMajorsByFacultyID(int facultyID)
        {
            var majors = _majorRepository.GetMajorsByFacultyID(facultyID)
                .Select(m => new
                {
                    MajorID = m.MajorID,
                    MajorNames = m.MajorName
                })
                .ToList();

            return Json(majors, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            var majors = _facultyRepository.GetFaculties()
                .Select(f => new SelectListItem
                {
                    Value = f.FacultyID.ToString(),
                    Text = f.FacultyName
                })
                .ToList();
            ViewBag.FacultyList = majors;
            return View();
        }

        // GET: Major/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Major/Create
        public ActionResult Create()
        {
            var majors = _facultyRepository.GetFaculties()
                .Select(f => new SelectListItem
                {
                    Value = f.FacultyID.ToString(),
                    Text = f.FacultyName
                })
                .ToList();
            ViewBag.FacultyList = majors;
            return View();
        }

        // POST: Major/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MajorModel major)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _majorRepository.InsertMajor(major);
                    //TempData["SuccessMessage"] = "Major added successfully";
                    //return RedirectToAction("Index");
                }
                ViewBag.FacultyList = _facultyRepository.GetFaculties()
                .Select(f => new SelectListItem
                {
                    Value = f.FacultyID.ToString(),
                    Text = f.FacultyName
                })
                .ToList();

                return Json(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                ViewBag.FacultyList = _facultyRepository.GetFaculties()
                .Select(f => new SelectListItem
                {
                    Value = f.FacultyID.ToString(),
                    Text = f.FacultyName
                })
                .ToList();
                ModelState.AddModelError("", "Unable to add new major due to " + ex.Message);
                //return View(major);
                return Json(new
                {
                    error = true,
                    errorMsg = "Error adding major " + ex.Message
                });
            }
        }

        // GET: Major/Edit/5
        public JsonResult GetMajorID(int id)
        {
            var item = _majorRepository.GetMajorByID(id);
            if (item == null)
            {
                return Json(new { error = true }, JsonRequestBehavior.AllowGet);
            }

            var result = Mapping.Mapper.Map<MajorModel>(item);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult Edit(int id)
        //{
        //    var majors = _majorRepository.GetMajorByID(id);
        //    if (majors == null)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.FacultyList = _facultyRepository.GetFaculties()
        //        .Select(f => new SelectListItem
        //        {
        //            Value = f.FacultyID.ToString(),
        //            Text = f.FacultyName
        //        })
        //        .ToList();
        //    return View(majors);
        //}

        // POST: Major/Edit/5
        [HttpPost]
        public ActionResult Edit(MajorModel major)
        {
            try
            {
                var existingData = _majorRepository.GetMajorByID(major.MajorID);
                if (existingData == null)
                {
                    return HttpNotFound();
                }
                if (ModelState.IsValid)
                {
                    _majorRepository.UpdateMajor(major);
                    TempData["SuccessMessage"] = "Major updated successfully!";
                }
                return Json(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to edit major due to " + ex.Message);
                return View();
            }
        }

        // GET: Major/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Major/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var existingData = _majorRepository.GetMajorByID(id);
                if (existingData == null)
                {
                    return HttpNotFound();
                }
                _majorRepository.DeleteMajor(id);
                return Json(new { success = true, message = "Major deleted successfully" });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting major " + ex.Message;
                ModelState.AddModelError("", "Error deleting major " + ex.Message);
                return Json(new { success = false, message = "Error deleting major " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult DeleteSelected(int[] ids)
        {
            try
            {
                if (ids != null && ids.Length > 0)
                {
                    foreach (var itemID in ids)
                    {
                        _majorRepository.DeleteMajor(itemID);  //CHANGE THIS
                    }
                    return Json(new
                    {
                        success = true,
                        message = "Selected items have been deleted successfully"
                    });
                }
                return Json(new
                {
                    success = false,
                    message = "No items selected for deletion"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Error deleting items: " + ex.Message
                });
            }
        }
    }
}
