using Course.DataLayer.Entities.User;
using System.Collections.Generic;

namespace Course.Core.DTDs.AdminPanel
{
    public class UserListViweModel
    {
        public List<User> Users { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }

        public string FilterUserName { get; set; }
        public string FilterEmail { get; set; }
        public bool FilterIsDeleted { get; set; }

    }
}
