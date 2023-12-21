using Course.Core.Common;
using Course.Core.DTDs;
using Course.Core.DTDs.Account;
using Course.Core.DTDs.AdminPanel;
using Course.Core.Services.Interfaces;
using Course.Core.Utility;
using Course.Core.Utility.Convertors;
using Course.Core.Utility.Generator;
using Course.DataLayer.Context;
using Course.DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Core.Services
{
    public class UserService : IUserService
    {
        private CourseDbContext _context;
        private readonly IWalletService _walletService;

        public UserService(CourseDbContext context, IWalletService walletService)
        {
            _context = context;
            _walletService = walletService;
        }

        public int AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.UserId;
        }

        #region Account

        public bool IsExistUserName(string userName)
        {
            return _context.Users.Any(u => u.UserName == userName);
        }

        public bool IsExistEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public User RegisterUser(RegisterViewModel registerViewModel)
        {
            var user = new User()
            {
                Email = FixedText.FixedEmail(registerViewModel.Email),
                UserName = registerViewModel.UserName,
                Password = PasswordHelper.EncodePasswordMd5(registerViewModel.Password),
                UserAvatar = ConstValues.UserDefaultAvatar,
                RegisterDate = DefaultValues.DateTimeNow(true),
                ActiveCode = CodeGenerator.GenerateActiveCode(),
                IsActive = false,
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;

        }

        public User GetUser(LoginViewModel loginViewModel)
        {
            var passwordEncod = PasswordHelper.EncodePasswordMd5(loginViewModel.Password);
            var email = FixedText.FixedEmail(loginViewModel.Email);

            return _context.Users
                    .SingleOrDefault(p => p.Email == email && p.Password == passwordEncod);
        }

        public bool ActiveAccount(string activeCode)
        {
            var user = _context.Users.SingleOrDefault(p => p.ActiveCode == activeCode);

            if (user == null || user.IsActive)
            {
                return false;
            }

            user.IsActive = true;
            user.ActiveCode = CodeGenerator.GenerateActiveCode();

            _context.SaveChanges();

            return true;
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(p => p.Email == email);
        }

        public User GetUserByActievCode(string activeCode)
        {
            return _context.Users.SingleOrDefault(p => p.ActiveCode == activeCode);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        #endregion

        #region Userpanel

        public User GetUserByUsername(string username)
        {
            return _context.Users.SingleOrDefault(p => p.UserName == username);
        }

        public User GetUserById(int id)
        {
            return _context.Users.SingleOrDefault(p => p.UserId == id);
        }

        public InformationUserViewModel GetuserInformation(int id)
        {

            var user = GetUserById(id);

            InformationUserViewModel information = new InformationUserViewModel();
            information.UserName = user.UserName;
            information.Email = user.Email;
            information.RegisterDate = user.RegisterDate;
            information.Wallet = _walletService.GetWalletAmount(id);

            return information;

        }


        public SideBarUserPanelViewModel GetSideBarUserPanelData(string username)
        {
            return _context.Users.Where(u => u.UserName == username).Select(u => new SideBarUserPanelViewModel()
            {
                UserName = u.UserName,
                ImageName = u.UserAvatar,
                RegisterDate = u.RegisterDate

            }).SingleOrDefault();
        }

        public SideBarUserPanelViewModel GetSideBarUserPanelDataById(int id)
        {
            return _context.Users.Where(u => u.UserId == id).Select(u => new SideBarUserPanelViewModel()
            {
                UserName = u.UserName,
                ImageName = u.UserAvatar,
                RegisterDate = u.RegisterDate

            }).SingleOrDefault();
        }

        public EditProfileViewModel GetUserPofile(string username)
        {
            return _context.Users
                .Where(p => p.UserName == username)
                .Select(p => new EditProfileViewModel()
                {
                    AvatarName = p.UserAvatar,
                    UserName = p.UserName,
                }).Single();
        }

        public void EditUserProfile(string currentUsername, EditProfileViewModel editProfileViewModel)
        {
            if (editProfileViewModel.UserAvatar != null)
            {

                if (editProfileViewModel.AvatarName != editProfileViewModel.UserAvatar.FileName &&
                    editProfileViewModel.AvatarName != ConstValues.UserDefaultAvatar)
                {
                    string oldImage_FullPath = FileOperation.GetFullPath(ConstValues.AvatarFolder, editProfileViewModel.AvatarName);
                    FileOperation.Delete(oldImage_FullPath);

                }

                editProfileViewModel.AvatarName = FileOperation.GenerateAvatarFullName(Path.GetExtension(editProfileViewModel.UserAvatar.FileName));

                string newImage_FullPath = FileOperation.GetFullPath(ConstValues.AvatarFolder, editProfileViewModel.AvatarName);

                using (var stream = new FileStream(newImage_FullPath, FileMode.Create))
                {
                    editProfileViewModel.UserAvatar.CopyTo(stream);
                }

            }

            var user = GetUserByUsername(currentUsername);
            user.UserName = editProfileViewModel.UserName;
            user.UserAvatar = editProfileViewModel.AvatarName;

            UpdateUser(user);
        }

        public bool IsExistUsenameExistEdit(int id, string username)
        {
            return _context.Users
                .Where(p => p.UserId != id && p.UserName == username)
                .Any();
        }

        public bool IsePasswordCorrect(string username, string EncryptedPassword)
        {
            return
                _context.Users
                .Any(p => p.UserName == username && p.Password == EncryptedPassword);
        }

        public void ChangePassword(string username, string EncryptedPassword)
        {
            var user = _context.Users.SingleOrDefault(p => p.UserName == username);

            user.Password = EncryptedPassword;

            _context.SaveChanges();

        }

        #endregion

        #region AdminPanel

        public UserListViweModel GetUsers(int pageId = 1, string filterEmail = "", string filterUserName = "", bool filterIsDeleted = false)
        {
            IQueryable<User> result = _context.Users
                .Where(p => p.IsDeleted == filterIsDeleted);

            if (!string.IsNullOrEmpty(filterEmail))
            {
                result = result.Where(u => u.Email.Contains(filterEmail));
            }

            if (!string.IsNullOrEmpty(filterUserName))
            {
                result = result.Where(u => u.UserName.Contains(filterUserName));
            }

            // Show Item In Page
            int take = 20;
            int skip = (pageId - 1) * take;


            var users = new UserListViweModel
            {
                CurrentPage = pageId,
                PageCount = result.Count() / take,
                Users = result.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList()
            };

            return users;
        }

        public int AddUserFromAdmin(AddUserViewModel userVm, List<int> selectedRoles)
        {
            User user = new User();

            user.Password = PasswordHelper.EncodePasswordMd5(userVm.Password);
            user.ActiveCode = CodeGenerator.GenerateActiveCode();
            user.Email = userVm.Email;
            user.IsActive = true;
            user.RegisterDate = DefaultValues.DateTimeNow();
            user.UserName = userVm.UserName;


            #region Save Avatar

            if (userVm.UserAvatar != null)
            {

                user.UserAvatar = FileOperation.GenerateAvatarFullName(Path.GetExtension(userVm.UserAvatar.FileName));
                string imagePath = FileOperation.GetFullPath(ConstValues.AvatarFolder, user.UserAvatar);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    userVm.UserAvatar.CopyTo(stream);
                }
            }
            else
            {
                user.UserAvatar = ConstValues.UserDefaultAvatar;
            }

            #endregion

            _context.Users.Add(user);

            foreach (int roleId in selectedRoles)
            {
                _context.UserRoles.Add(new UserRole()
                {
                    RoleId = roleId,
                    UserId = user.UserId
                });
            }


            return user.UserId;

        }

        public EditUserViewModel GetUserForEdit(int userId)
        {
            return _context.Users
                .Where(p => p.UserId == userId)
                .Select(p => new EditUserViewModel()
                {
                    AvatarName = p.UserAvatar,
                    Email = p.Email,
                    UserName = p.UserName,
                    UserId = p.UserId,
                    UserRoles = p.UserRoles.Select(p => p.UserId).ToList(),
                })
                .Single();
        }

        public void EditUser(EditUserViewModel editUserViewModel, IList<int> selectedRoles)
        {
            var user = GetUserById(editUserViewModel.UserId);

            user.Email = editUserViewModel.Email;

            if (!string.IsNullOrEmpty(editUserViewModel.Password))
            {
                user.Password = PasswordHelper.EncodePasswordMd5(editUserViewModel.Password);
            }


            if (editUserViewModel.UserAvatar != null)
            {

                if (editUserViewModel.AvatarName != editUserViewModel.UserAvatar.FileName &&
                    editUserViewModel.AvatarName != ConstValues.UserDefaultAvatar)
                {
                    string oldImage_FullPath = FileOperation.GetFullPath(ConstValues.AvatarFolder, editUserViewModel.AvatarName);
                    FileOperation.Delete(oldImage_FullPath);

                }

                user.UserAvatar = FileOperation.GenerateAvatarFullName(Path.GetExtension(editUserViewModel.UserAvatar.FileName));

                string newImage_FullPath = FileOperation.GetFullPath(ConstValues.AvatarFolder, user.UserAvatar);

                using (var stream = new FileStream(newImage_FullPath, FileMode.Create))
                {
                    editUserViewModel.UserAvatar.CopyTo(stream);
                }
            }

            _context.UserRoles
                .Where(r => r.UserId == editUserViewModel.UserId)
                .ToList()
                .ForEach(r => _context.UserRoles.Remove(r));

            foreach (int roleId in selectedRoles)
            {
                _context.UserRoles.Add(new UserRole()
                {
                    RoleId = roleId,
                    UserId = user.UserId
                });
            }

            _context.SaveChanges();


        }

        public void DeleteUser(int userId)
        {
            _context.Users.Find(userId)
                .IsDeleted = true;
            _context.SaveChanges();
        }

        #endregion

    }


}
