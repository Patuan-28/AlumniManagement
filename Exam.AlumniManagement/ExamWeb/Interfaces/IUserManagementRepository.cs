using ExamWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ExamWeb.Interfaces
{
    public interface IUserManagementRepository
    {
        
        bool IsSuperadminExists();
        
        void RegisterUser(string fullName, string email, string password, bool superadmin);
        
        void UpdatePassword(string id, string password);
        
        void UpdateUserFullName(AspNetUserModel.UserModel model);
        
        void DeleteUser(string id);
        
        AspNetUserModel.UserModel GetUser(string id);
        
        IEnumerable<AspNetUserModel.UserModel> GetAllUsers();
        
        IEnumerable<AspNetUserModel.UserModel> GetUsersByRole(string roleName);

        void AssignSuperadmin(string id);

        IEnumerable<AspNetUserModel.RoleModel> GetRoles();

        void InsertRole(AspNetUserModel.RoleModel role);
    
        void DeleteRole(string id);
    }
}
