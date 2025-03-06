using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aspose.Cells;
using ExamWeb.AlumniImageService;
using ExamWeb.Interfaces;
using ExamWeb.Models;
using ExamWeb.Services;

namespace ExamWeb.Controllers
{
    [Authorize]
    public class AlumniController : Controller
    {
        private IAlumniRepository _alumniRepository;
        private IMajorRepository _majorRepository;
        private IFacultyRepository _facultyRepository;
        private IExcelService _excelService;

        public int fileSizeLimit = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AlumniPhotoSizeLimit"]); // 3MB
        public string fileTypes = System.Configuration.ConfigurationManager.AppSettings["FileTypes"]; // "jpeg, jpg, png"
        public string alumniPhotosPath = System.Configuration.ConfigurationManager.AppSettings["AlumniPhotosPath"]; // "~/images/AlumniImages/"
        public AlumniController()
        {
            _alumniRepository = new AlumniRepository();
            _majorRepository = new MajorRepository();
            _facultyRepository = new FacultyRepository();
            _excelService = new ExcelService();
        }
        // GET: Alumni
        public JsonResult GetAlumnis(string faculty = null, string major = null)
        {
            var alumnisData = _alumniRepository.GetAlumnis();

            // Filter berdasarkan faculty jika parameter tidak null atau kosong
            if (!string.IsNullOrEmpty(faculty))
            {
                alumnisData = alumnisData.Where(a => a.FacultyNames == faculty).ToList();
            }

            // Filter berdasarkan major jika parameter tidak null atau kosong
            if (!string.IsNullOrEmpty(major))
            {
                alumnisData = alumnisData.Where(a => a.MajorNames == major).ToList();
            }

            var json = Json(new { data = alumnisData }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }


        public JsonResult GetMajorByFacultyID(int facultyID)
        {
            var room = _majorRepository.GetMajorsByFacultyID(facultyID)
                .Select(sp => new AlumniModel
                {
                    MajorID = sp.MajorID,
                    MajorNames = sp.MajorName
                })
                .ToList();
            var result = Json(room, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
            return result;
        }

        public JsonResult GetDistrictsByStateID(int stateID)
        {
            var districts = _alumniRepository.GetDistrictsByStateID(stateID)
                .Select(d => new
                {
                    DistrictID = d.DistrictID,
                    DistrictNames = d.DistrictName
                })
                .ToList();

            return Json(districts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            ViewBag.FacultiesList = _facultyRepository.GetFaculties()
                .Select(f => new SelectListItem
                {
                    Value = f.FacultyID.ToString(),
                    Text = f.FacultyName,
                })
                .ToList();

            ViewBag.StatesList = _alumniRepository.GetStates()
                .Select(a => new SelectListItem
                {
                    Value = a.StateID.ToString(),
                    Text = a.StateName,
                })
                .ToList();

            ViewBag.HobbiesList = _alumniRepository.GetHobbys()
                .Select(h => new SelectListItem
                {
                    Value = h.HobbyID.ToString(),
                    Text = h.Name,
                }).ToList();

            var alumni = new AlumniModel();
            alumni.Districts = new List<SelectListItem>();
            alumni.Majors = new List<SelectListItem>();
            alumni.Hobbies = new List<SelectListItem>();
            return View(alumni);
        }

        // GET: Alumni/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Alumni/Create
        public ActionResult Create()
        {
            var faculties = _facultyRepository.GetFaculties()
                .Select(f => new SelectListItem
                {
                    Value = f.FacultyID.ToString(),
                    Text = f.FacultyName
                })
                .ToList();

            var states = _alumniRepository.GetStates()
                .Select(a => new SelectListItem
                {
                    Value = a.StateID.ToString(),
                    Text = a.StateName
                })
                .ToList();

            ViewBag.FacultiesList = faculties;
            ViewBag.StatesList = states;
            ViewBag.DistrictsList = new List<SelectListItem>();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpsertAlumni(AlumniModel alumniModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isUpdate = alumniModel.AlumniID > 0; // Jika ID > 0 maka update, jika 0 atau null maka insert
                    _alumniRepository.UpsertAlumni(alumniModel);

                    if (isUpdate)
                    {
                        TempData["SuccessMessage"] = "Alumni updated successfully";
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Alumni added successfully";
                    }

                    return RedirectToAction("Index");
                }

                return View(alumniModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save alumni: " + ex.Message);
                return View(alumniModel);
            }
        }



        // POST: Alumni/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AlumniModel alumni, HttpPostedFileBase file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //if file exist 
                    if (file != null)
                    {
                        //validate if file is more than filesizelimit
                        if (file.ContentLength > fileSizeLimit)
                        {
                            TempData["ErrorMessage"] = "File size must be less than 3MB";
                            return RedirectToAction("Index");
                        }
                        var extension = System.IO.Path.GetExtension(file.FileName).ToLower();
                        if (extension != ".jpeg" && extension != ".png" && extension != ".jpg")
                        {
                            TempData["ErrorMessage"] = "Only .jpeg, .jpg and .png files are allowed.";
                            return RedirectToAction("Index");
                        }
                        var photoName = Guid.NewGuid().ToString() + extension;
                        var photoPath = Path.Combine(Server.MapPath(alumniPhotosPath), photoName);

                        file.SaveAs(photoPath);
                        alumni.PhotoName = photoName;
                        alumni.PhotoPath = alumniPhotosPath;

                    }
                    _alumniRepository.UpsertAlumni(alumni);
                    //TempData["SuccessMessage"] = "Alumni added successfully";
                }

                var faculties = _facultyRepository.GetFaculties()
                .Select(f => new SelectListItem
                {
                    Value = f.FacultyID.ToString(),
                    Text = f.FacultyName
                })
                .ToList();
                var states = _alumniRepository.GetStates()
                    .Select(a => new SelectListItem
                    {
                        Value = a.StateID.ToString(),
                        Text = a.StateName
                    })
                    .ToList();
                ViewBag.MajorsList = _majorRepository.GetMajorsByFacultyID(alumni.FacultyID)
                    .Select(m => new SelectListItem
                    {
                        Value = m.MajorID.ToString(),
                        Text = m.MajorName,
                        Selected = m.FacultyID == alumni.FacultyID
                    })
                    .ToList();
                ViewBag.DistrictsList = _alumniRepository.GetDistrictsByStateID(alumni.StateID)
                    .Select(d => new SelectListItem
                    {
                        Value = d.DistrictID.ToString(),
                        Text = d.DistrictName,
                        Selected = d.DistrictID == alumni.DistrictID
                    })
                    .ToList();
                ViewBag.SelectedMajor = alumni.MajorID;
                ViewBag.SelectedDistrict = alumni.DistrictID;
                return Json(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                var faculties = _facultyRepository.GetFaculties()
                .Select(f => new SelectListItem
                {
                    Value = f.FacultyID.ToString(),
                    Text = f.FacultyName
                })
                .ToList();
                var states = _alumniRepository.GetStates()
                    .Select(a => new SelectListItem
                    {
                        Value = a.StateID.ToString(),
                        Text = a.StateName
                    })
                    .ToList();
                ViewBag.MajorsList = _majorRepository.GetMajorsByFacultyID(alumni.FacultyID)
                    .Select(m => new SelectListItem
                    {
                        Value = m.MajorID.ToString(),
                        Text = m.MajorName,
                        Selected = m.FacultyID == alumni.FacultyID
                    })
                    .ToList();
                ViewBag.DistrictsList = _alumniRepository.GetDistrictsByStateID(alumni.StateID)
                    .Select(d => new SelectListItem
                    {
                        Value = d.DistrictID.ToString(),
                        Text = d.DistrictName,
                        Selected = d.StateID == alumni.StateID
                    })
                    .ToList();
                ViewBag.SelectedMajor = alumni.MajorID;
                ViewBag.SelectedDistrict = alumni.DistrictID;
                ModelState.AddModelError("", "Unable to save alumni " + ex.Message);
                return Json(new
                {
                    error = true,
                    errorMsg = "Error adding alumni " + ex.Message
                });
            }
        }

        // GET: Alumni/Edit/5
        public JsonResult GetAlumniID(int id)
        {
            var item = _alumniRepository.GetAlumniByID(id);
            var result = Mapping.Mapper.Map<AlumniModel>(item);

            if (item == null)
            {
                return Json(new { error = true }, JsonRequestBehavior.AllowGet);
            }

            ViewBag.FacultiesList = _facultyRepository.GetFaculties()
                .Select(f => new SelectListItem
                {
                    Value = f.FacultyID.ToString(),
                    Text = f.FacultyName,
                    Selected = f.FacultyID == item.FacultyID
                })
                .ToList();

            ViewBag.StatesList = _alumniRepository.GetStates()
                .Select(a => new SelectListItem
                {
                    Value = a.StateID.ToString(),
                    Text = a.StateName,
                    Selected = a.StateID == item.StateID
                })
                .ToList();
            result.Hobbies = _alumniRepository.GetHobbys()
                .Select(h => new SelectListItem
                {
                    Value = h.HobbyID.ToString(),
                    Text = h.Name,
                    Selected = item.SelectedHobbies != null && item.SelectedHobbies.Contains(h.HobbyID)
                }).ToList();

            result.Majors = _majorRepository.GetMajorsByFacultyID(item.FacultyID)
                .Select(m => new SelectListItem
                {
                    Value = m.MajorID.ToString(),
                    Text = m.MajorName,
                    Selected = m.MajorID == item.MajorID
                })
                .ToList();

            result.Districts = _alumniRepository.GetDistrictsByStateID(item.StateID)
                .Select(d => new SelectListItem
                {
                    Value = d.DistrictID.ToString(),
                    Text = d.DistrictName,
                    Selected = d.DistrictID == item.DistrictID
                })
                .ToList();

            // Pass selected degree to ViewBag
            ViewBag.SelectedDegree = item.Degree;

            //Pass selected gender to ViewBag
            ViewBag.SelectedGender = item.Gender;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // POST: Alumni/Edit/5
        [HttpPost]
        public ActionResult Edit(AlumniModel alumni, HttpPostedFileBase file)
        {
            try
            {
                var existingAlumni = _alumniRepository.GetAlumniByID(alumni.AlumniID);
                if (existingAlumni == null)
                {
                    return HttpNotFound();
                }

                if (ModelState.IsValid)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        if (file.ContentLength > fileSizeLimit)
                        {
                            TempData["ErrorMessage"] = "File size must be less than 3MB";
                            return RedirectToAction("Index");
                        }

                        var extension = Path.GetExtension(file.FileName).ToLower();
                        if (extension != ".jpeg" && extension != ".png" && extension != ".jpg")
                        {
                            TempData["ErrorMessage"] = "Only .jpeg, .jpg, and .png files are allowed.";
                            return RedirectToAction("Index");
                        }

                        var photoName = Guid.NewGuid().ToString() + extension;
                        var photoPath = Path.Combine(Server.MapPath(alumniPhotosPath), photoName);

                        file.SaveAs(photoPath);
                        alumni.PhotoName = photoName;
                        alumni.PhotoPath = alumniPhotosPath;
                    }
                    else
                    {
                        alumni.PhotoName = existingAlumni.PhotoName;
                        alumni.PhotoPath = existingAlumni.PhotoPath ?? string.Empty;
                    }

                    _alumniRepository.UpsertAlumni(alumni);
                    TempData["SuccessMessage"] = "Alumni updated successfully!";

                    return Json(new
                    {
                        success = true
                    });
                }
            }
            catch (Exception ex)
            {
                var faculties = _facultyRepository.GetFaculties()
                .Select(f => new SelectListItem
                {
                    Value = f.FacultyID.ToString(),
                    Text = f.FacultyName
                })
                .ToList();

                var states = _alumniRepository.GetStates()
                    .Select(a => new SelectListItem
                    {
                        Value = a.StateID.ToString(),
                        Text = a.StateName
                    })
                    .ToList();

                ViewBag.MajorsList = _majorRepository.GetMajorsByFacultyID(alumni.FacultyID)
                    .Select(m => new SelectListItem
                    {
                        Value = m.MajorID.ToString(),
                        Text = m.MajorName,
                        Selected = m.FacultyID == alumni.FacultyID
                    })
                    .ToList();

                ViewBag.DistrictsList = _alumniRepository.GetDistrictsByStateID(alumni.StateID)
                    .Select(d => new SelectListItem
                    {
                        Value = d.DistrictID.ToString(),
                        Text = d.DistrictName,
                        Selected = d.StateID == alumni.StateID
                    })
                    .ToList();

                ViewBag.SelectedMajor = alumni.MajorID;
                ViewBag.SelectedDistrict = alumni.DistrictID;

                ModelState.AddModelError("", "Unable to save alumni " + ex.Message);
                return View(alumni);
            }

            return View(alumni);
        }

        // POST: Alumni/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var existingData = _alumniRepository.GetAlumniByID(id);
                if (existingData == null)
                {
                    return HttpNotFound();
                }

                // Delete the profile image if it exists
                if (!string.IsNullOrEmpty(existingData.PhotoName))
                {
                    var filePath = Path.Combine(Server.MapPath(alumniPhotosPath), existingData.PhotoName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _alumniRepository.DeleteAlumni(id);
                return Json(new { success = true, message = "Alumni deleted successfully" });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting alumni " + ex.Message;
                ModelState.AddModelError("", "Error deleting alumni " + ex.Message);
                return Json(new { success = false, message = "Error deleting alumni " + ex.Message });
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
                        var existingData = _alumniRepository.GetAlumniByID(itemID);
                        if (existingData != null)
                        {
                            // Delete the profile image if it exists
                            if (!string.IsNullOrEmpty(existingData.PhotoName))
                            {
                                var filePath = Path.Combine(Server.MapPath(alumniPhotosPath), existingData.PhotoName);
                                if (System.IO.File.Exists(filePath))
                                {
                                    System.IO.File.Delete(filePath);
                                }
                            }

                            _alumniRepository.DeleteAlumni(itemID);
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

        public ActionResult ExportToExcel()
        {
            //get workbook
            var workbook = _excelService.AlumniExportToExcel();

            //save workbook to stream
            var stream = new System.IO.MemoryStream();
            workbook.Save(stream, SaveFormat.Xlsx);

            //return file
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Alumnis.xlsx");
        }
        [HttpPost]

        public ActionResult AlumniImportToExcel(HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    _excelService.ImportAlumniExcelToDatabase(file);
                }
                TempData["SuccessMessage"] = "Data imported successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to import data: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        //Get All Faculty and Major Data
        public JsonResult GetFacultyMajorData()
        {
            var faculties = _facultyRepository.GetFaculties()
                .Select(f => new SelectListItem
                {
                    Value = f.FacultyID.ToString(),
                    Text = f.FacultyName
                })
                .ToList();
            var majors = _majorRepository.GetMajors()
                .Select(m => new SelectListItem
                {
                    Value = m.MajorID.ToString(),
                    Text = m.MajorName
                })
                .ToList();

            return Json(new { faculties, majors }, JsonRequestBehavior.AllowGet);
        }

        //Get Major Based on Faculty
        public JsonResult GetMajorsByFaculty(int faculty)
        {
            //var facultyID = _majorRepository.GetFacultyIDByName(faculty);
            var majors = _majorRepository.GetMajorsByFacultyID(faculty)
                .Select(m => new SelectListItem
                {
                    Value = m.MajorID.ToString(),
                    Text = m.MajorName
                })
                .ToList();
            return Json(majors, JsonRequestBehavior.AllowGet);
        }
    }
}
