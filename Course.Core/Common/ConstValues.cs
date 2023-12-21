
namespace Course.Core.Common
{
    public static class ConstValues
    {
        public readonly static string UserDefaultAvatar = "Default.jpeg";
        public readonly static string SiteName = "سایت آموزشی";
        public readonly static string SiteNameEn = "Learning Site";
        public readonly static string LoginPath = "/Account/Login";
        public readonly static string LogoutPath = "/Account/Logout";
        public readonly static string AvatarFolder = "/UserAvatar";
        public readonly static string DomainPath = "https://localhost:44351/";
        public readonly static string ActivationFullPath = DomainPath + "Account/ActiveAccount";
        public readonly static string ResetPasswordFullPath = DomainPath + "Account/ResetPassword";
        public readonly static string UserDefaultAvatarFullPath = AvatarFolder + "/" + UserDefaultAvatar;

    }
}
