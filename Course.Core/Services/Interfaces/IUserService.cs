using Course.Core.DTDs;
using Course.Core.DTDs.Account;
using Course.Core.DTDs.AdminPanel;
using Course.DataLayer.Entities.User;
using System.Collections.Generic;

namespace Course.Core.Services.Interfaces
{
    public interface IUserService
    {

        User GetUserByUsername(string username);

        User GetUserById(int id);

        int AddUser(User user);

        #region Account

        bool IsExistUserName(string userName);
        bool IsExistEmail(string email);
        User RegisterUser(RegisterViewModel registerViewModel);
        User GetUser(LoginViewModel loginViewModel);
        bool ActiveAccount(string activeCode);
        User GetUserByEmail(string email);
        User GetUserByActievCode(string activeCode);
        void UpdateUser(User user);

        #endregion

        #region Userpanel

        InformationUserViewModel GetuserInformation(int id);

        SideBarUserPanelViewModel GetSideBarUserPanelData(string username);

        SideBarUserPanelViewModel GetSideBarUserPanelDataById(int Id);

        EditProfileViewModel GetUserPofile(string username);

        void EditUserProfile(string currentUsername, EditProfileViewModel editProfileViewModel);

        bool IsExistUsenameExistEdit(int id, string username);

        bool IsePasswordCorrect(string username, string EncryptedPassword);

        void ChangePassword(string username, string EncryptedPassword);



        #endregion

        #region Admin Panel

        public UserListViweModel GetUsers(int pageId = 1, string filterEmail = "", string filterUserName = "", bool filterIsDeleted=false);

        public int AddUserFromAdmin(AddUserViewModel userVm, List<int> selectedRoles);

        public EditUserViewModel GetUserForEdit(int userId);

        public void EditUser(EditUserViewModel editUserViewModel, IList<int> selectedRoles);

        public void DeleteUser(int userId);

        #endregion


    }
}
