using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExamWeb.AlumniService;
using ExamWeb.Interfaces;
using ExamWeb.JobHistoryService;
using ExamWeb.Models;
using ExamWeb.Services;

namespace ExamWeb.Controllers
{
    [Authorize]
    public class JobHistoryController : Controller
    {
        private IJobHistoryRepository _jhRepository;
        private IAlumniRepository _alumniRepository;
        public JobHistoryController()
        {
            _jhRepository = new JobHistoryRepository();
            _alumniRepository = new AlumniRepository();
        }
        // GET: JobHistory
        public JsonResult GetJobHistories(int id)
        {
            var jhData = _jhRepository.GetJobHistories(id);
            var json = Json(new { data = jhData }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult Index(int alumniID)
        {
            ViewBag.AlumniID = alumniID;
            var fullName = _alumniRepository.GetAlumniByID(alumniID).FullNames;
            ViewBag.FullName = fullName;
            return View();
        }

        // GET: JobHistory/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: JobHistory/Create
        public ActionResult Create(int alumniID)
        {
            ViewBag.AlumniID = alumniID;
            return View();
        }


        // POST: JobHistory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JobHistoryModel jobHistory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _jhRepository.InsertJobHistory(jobHistory);
                    //TempData["SuccessMessage"] = "Job added successfully";
                }
                return Json(new
                {
                    success = true
                });
                //return RedirectToAction("Index", new { alumniID = jobHistory.AlumniID });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error adding job: " + ex.Message);
                //return View(jobHistory);
                return Json(new
                {
                    error = true,
                    errorMsg = "Error adding alumni " + ex.Message
                });
            }
        }

        // GET: JobHistory/Edit/5
        public JsonResult GetJobHistoryID(int id)
        {
            var item = _jhRepository.GetJobHistoryByID(id);
            var result = Mapping.Mapper.Map<JobHistoryModel>(item);
            if (item == null)
            {
                return Json(new { error = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult Edit(int id)
        //{
        //    var jh = _jhRepository.GetJobHistoryByID(id);
        //    //int alumniID = (int)jh.AlumniID;
        //    //var alumniID = _alumniRepository.GetAlumniByID((int)jh.AlumniID);
        //    if (jh == null)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.AlumniID = jh.AlumniID;
        //    return View(jh);
        //}

        // POST: JobHistory/Edit/5
        [HttpPost]
        public ActionResult Edit(JobHistoryModel jobHistory)
        {
            try
            {
                var existingData = _jhRepository.GetJobHistories(jobHistory.JobHistoryID);
                if (existingData == null)
                {
                    return HttpNotFound();
                }

                if (ModelState.IsValid)
                {
                    _jhRepository.UpdateJobHistory(jobHistory);
                    TempData["SuccessMessage"] = "Job Updated succesfully";
                }

                //return RedirectToAction("Index", new { alumniID = jobHistory.AlumniID });
                return Json(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to update job due to " + ex.Message);
                return View(jobHistory);
            }
        }

        // GET: JobHistory/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: JobHistory/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var existingData = _jhRepository.GetJobHistoryByID(id);
                if (existingData == null)
                {
                    return HttpNotFound();
                }
                _jhRepository.DeleteJobHistory(id);
                return Json(new
                {
                    success = true
                }
                );
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to delete due to " + ex.Message);
                return Json(new
                {
                    success = false
                }
                );
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
                        _jhRepository.DeleteJobHistory(itemID);  //CHANGE THIS
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
