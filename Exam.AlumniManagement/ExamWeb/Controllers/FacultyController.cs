using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using ExamWeb.Interfaces;
using ExamWeb.Models;
using ExamWeb.Services;

namespace ExamWeb.Controllers
{
    [Authorize]
    public class FacultyController : Controller
    {
        private IFacultyRepository _facultyRepository;
        public FacultyController()
        {
            _facultyRepository = new FacultyRepository();
        }

        // GET: Faculty
        public JsonResult GetFaculties()
        {
            var facultiesData = _facultyRepository.GetFaculties();
            var json = Json(new { data = facultiesData }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult Index()
        {
            return View();
        }

        // GET: Faculty/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Faculty/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Faculty/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FacultyModel faculty)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _facultyRepository.InsertFaculty(faculty);
                }
                return Json(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to add new faculty due to " + ex.Message);
                return Json(new
                {
                    error = true,
                    errorMsg = "Error adding faculty " + ex.Message
                });
            }
        }

        // GET: Faculty/Edit/5
        public JsonResult GetFacultyID(int id)
        {
            var faculty = _facultyRepository.GetFacultyByID(id);
            if (faculty == null)
            {
                return Json(new
                {
                    error = true
                });
            }
            var result = Mapping.Mapper.Map<FacultyModel>(faculty);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult Edit(int id)
        //{
        //    var faculty = _facultyRepository.GetFacultyByID(id);
        //    if (faculty == null)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    return View(faculty);
        //}

        // POST: Faculty/Edit/5
        [HttpPost]
        public ActionResult Edit(FacultyModel faculty)
        {
            try
            {
                var existingData = _facultyRepository.GetFacultyByID(faculty.FacultyID);
                if (existingData == null)
                {
                    return HttpNotFound();
                }
                if (ModelState.IsValid)
                {
                    _facultyRepository.UpdateFaculty(faculty);
                    TempData["SuccessMessage"] = "Faculty updated successfully!";
                }

                return Json(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to edit faculty due to " + ex.Message);
                return Json(new
                {
                    error = true,
                    errorMsg = "Error saving faculty " + ex.Message
                });
            }
        }

        // GET: Faculty/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Faculty/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var existingData = _facultyRepository.GetFacultyByID(id);
                if (existingData == null)
                {
                    return HttpNotFound();
                }
                _facultyRepository.DeleteFaculty(id);
                return Json(new { success = true, message = "Faculty deleted successfully" });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting faculty " + ex.Message;
                ModelState.AddModelError("", "Error deleting faculty " + ex.Message);
                return Json(new { success = false, message = "Error deleting faculty " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult DeleteSelected(int[] ids)
        {
            try
            {
                if (ids != null && ids.Length > 0)
                {
                    foreach (var facultyid in ids)
                    {
                        _facultyRepository.DeleteFaculty(facultyid);
                    }
                    return Json(new
                    {
                        success = true,
                        message = "Selected faculties have been deleted successfully"
                    });
                }
                return Json(new
                {
                    success = false,
                    message = "No faculties selected for deletion"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Error deleting faculties: " + ex.Message
                });
            }
        }
    }
}
