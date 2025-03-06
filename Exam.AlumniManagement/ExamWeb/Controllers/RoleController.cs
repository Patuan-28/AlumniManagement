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
    [Authorize(Roles = "Superadmin")]
    public class RoleController : Controller
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public RoleController()
        {
            _userManagementRepository = new UserManagementRepository();
        }

        // GET: Role
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetRoles()
        {
            var roles = _userManagementRepository.GetRoles();
            return Json(new { data = roles }, JsonRequestBehavior.AllowGet);
        }

        // POST: Role/Create
        [HttpPost]
        public ActionResult Create(AspNetUserModel.RoleModel role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _userManagementRepository.InsertRole(role);
                    return Json(new
                    {
                        success = true,
                        message = "Role has been added successfully."
                    });
                }
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // GET: Role/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Role/Edit/5
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

        // GET: Role/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Role/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var existingRole = _userManagementRepository.GetRoles().Where(x => x.Id == id).FirstOrDefault();
                if (existingRole == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    _userManagementRepository.DeleteRole(id);
                    return Json(new { success = true, message = "Role deleted successfully" });
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
