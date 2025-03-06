using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExamWeb.Interfaces;
using ExamWeb.Models;
using ExamWeb.Services;
using Newtonsoft.Json;
using static ExamWeb.Models.AspNetUserModel;

namespace ExamWeb.Controllers
{
    [Authorize(Roles = "Superadmin")]
    public class UserController : Controller
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public UserController()
        {
            _userManagementRepository = new UserManagementRepository();
        }
        // GET: User
        public ActionResult Index()
        {
            var roles = _userManagementRepository.GetRoles()
            .Select(r => new { Value = r.Id, Text = r.Name })
            .ToList();
            ViewBag.RolesList = new MultiSelectList(roles, "Value", "Text"); // Using MultiSelectList
            return View();
        }

        public JsonResult GetUsers()
        {
            var usersData = _userManagementRepository.GetAllUsers();
            var json = Json(new { data = usersData }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public JsonResult GetUserID(string id)
        {
            var item = _userManagementRepository.GetUser(id);
            var result = Mapping.Mapper.Map<AspNetUserModel.UserModel>(item);

            result.SelectedRole = item.UserRoles.Select(r => r.RoleId).ToList();

            return Json(new
            {
                user = result,
                roles = _userManagementRepository.GetRoles() // Ambil semua role dari repository
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRoles()
        {
            var roles = _userManagementRepository.GetRoles();
            return Json(new { data = roles }, JsonRequestBehavior.AllowGet);
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(RoleModel role)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {

                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserManage/Edit/5
        [HttpPost]
        public ActionResult Edit()
        {
            try
            {
                // ✅ Read JSON from the request body
                string json;
                using (var reader = new StreamReader(Request.InputStream))
                {
                    json = reader.ReadToEnd();
                }

                // ✅ Deserialize JSON to UserModel
                var userModel = JsonConvert.DeserializeObject<UserModel>(json);

                if (userModel == null || userModel.Id == null)
                {
                    return HttpNotFound();
                }

                _userManagementRepository.UpdateUserFullName(userModel);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMsg = ex.Message });
            }
        }

        //public ActionResult UpdateFullName(string id, string fullName)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            //check existing user
        //            var user = _userManagementRepository.GetUser(id);
        //            var userPrevFullName = user.FullName;
        //            if (userPrevFullName == fullName)
        //            {
        //                TempData["SuccessMessage"] = "No changes made!";
        //                //return Json(new)
        //            }else
        //            {
        //                _userManagementRepository.UpdateUserFullName(id, fullName);
        //                TempData["SuccessMessage"] = "User updated successfully!";
        //            }
                    
        //        }
        //        return Json(new
        //        {
        //            success = true
        //        });
        //    }
        //    catch(Exception ex)
        //    {
        //        return Json(new { success=false ,message = ex.Message });
        //    }
        //}

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                    _userManagementRepository.DeleteUser(id);
                    return Json(new {success = true});
                    
            }
            catch
            {
                return View();
            }
        }
    }
}
