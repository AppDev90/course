using Course.Core.Services.Interfaces;
using Course.DataLayer.Context;
using Course.DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Core.Services
{
    public class RoleService : IRoleService
    {
        private CourseDbContext _context;

        public RoleService(CourseDbContext courseDbContext)
        {
            _context = courseDbContext;
        }

        public void AddRolesToUser(List<int> roleIds, int userId)
        {
            foreach (int roleId in roleIds)
            {
                _context.UserRoles.Add(new UserRole()
                {
                    RoleId = roleId,
                    UserId = userId
                });
            }

            _context.SaveChanges();
        }

        public List<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }
    }
}
