using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExamWeb.Interfaces;
using ExamWeb.Models;
using ExamWeb.Services;

namespace ExamWeb.Controllers
{
    [Authorize]
    public class CandidateController : Controller
    {
        private ICandidateRepository _candidateRepository;
        private IJobPostingRepository _jobPostingRepository;

        public CandidateController()
        {
            _candidateRepository = new CandidateRepository();
            _jobPostingRepository = new JobPostingRepository();
        }

        // GET: Candidate
        public ActionResult Index(Guid jobId)
        {
            var jobData = _jobPostingRepository.GetJobPostingByID(jobId);
            var allSkills = _jobPostingRepository.GetSkills();

            // Mengambil nama skill berdasarkan SelectedSkills dari jobData
            var skillNames = allSkills
                .Where(s => jobData.SelectedSkills.Contains(s.SkillID))
                .Select(s => s.Name)
                .ToList();

            ViewBag.JobTitle = jobData.Title;
            ViewBag.JobDescription = jobData.JobDescription;
            ViewBag.MinExperience = jobData.MinimumExperience;
            ViewBag.SkillNames = skillNames; // Simpan skill yang sudah difilter dalam ViewBag

            // Mengambil daftar semua skill untuk dropdown list, menghindari pemanggilan repository dua kali
            ViewBag.SkillsList = allSkills
                .Select(s => new { Value = s.SkillID, Text = s.Name })
                .ToList();

            var candidates = _jobPostingRepository.GetAllCandidateByJobID(jobId);
            ViewBag.JobID = jobId;

            return View(candidates);
        }


        public JsonResult GetCandidates(Guid jobId)
        {
            var candidates = _jobPostingRepository.GetAllCandidateByJobID(jobId);
            var json = Json(new { data = candidates }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        // GET: Candidate/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Candidate/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Candidate/Create
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

        // GET: Candidate/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Candidate/Edit/5
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

        // GET: Candidate/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Candidate/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
