using ExamWeb.Models;
using ExamWeb.Interfaces;
using ExamWeb.UserManagementService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using static ExamWeb.Models.AspNetUserModel;

namespace ExamWeb.Services
{
    public class UserManagementRepository : IUserManagementRepository
    {
        private UserManagementService.UserManagementServiceClient _userManagementServices;

        public UserManagementRepository()
        {
            _userManagementServices = new UserManagementServiceClient();
        }

        public void DeleteUser(string id)
        {
            _userManagementServices.DeleteUser(id);
        }

        public void DeleteRole(string id)
        {
            _userManagementServices.DeleteRole(id);
        }

        public IEnumerable<UserModel> GetAllUsers()
        {
            var data = _userManagementServices.GetAllUsers();
            var mapped = Mapping.Mapper.Map<IEnumerable<AspNetUserDTOUserDTO>, IEnumerable<UserModel>>(data);
            return mapped;
        }

        public AspNetUserModel.UserModel GetUser(string id)
        {
            var user = Mapping.Mapper.Map<AspNetUserModel.UserModel>(_userManagementServices.GetUser(id));
            return user;
        }

        public IEnumerable<AspNetUserModel.UserModel> GetUsersByRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public bool IsSuperadminExists()
        {
            bool isSuperadminExists = _userManagementServices.IsSuperadminExists();
            return isSuperadminExists;
        }

        public void RegisterUser(string fullName, string email, string password, bool superadmin)
        {
            throw new NotImplementedException();
        }

        public void AssignSuperadmin(string id)
        {
            var user = _userManagementServices.GetUser(id);
            if (user != null)
            {
                _userManagementServices.AssignSuperadmin(id);
            }
        }

        public void UpdatePassword(string id, string password)
        {
            throw new NotImplementedException();
        }

        public void UpdateUserFullName(UserModel model)
        {
            var mapped = Mapping.Mapper.Map<AspNetUserDTOUserDTO>(model);
            _userManagementServices.UpdateUserFullName(mapped);
        }

        public IEnumerable<RoleModel> GetRoles()
        {
            var data = _userManagementServices.GetRoles();
            var mapped = Mapping.Mapper.Map<IEnumerable<AspNetUserDTORoleDTO>, IEnumerable<RoleModel>>(data);
            return mapped;
        }

        public void InsertRole(RoleModel role)
        {
            var data = Mapping.Mapper.Map<AspNetUserDTORoleDTO>(role);
            _userManagementServices.InsertRole(data);
        }
    }
}
