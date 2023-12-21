

using System.ComponentModel.DataAnnotations;

namespace Course.DataLayer.Entities.User
{
    public class UserRole
    {

        [Key]
        public int UserRole_Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }


        #region Relations

        public User User { get; set; }
        public Role Role { get; set; }

        #endregion
    }
}
