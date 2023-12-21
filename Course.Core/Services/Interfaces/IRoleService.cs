using Course.DataLayer.Entities.User;
using System.Collections.Generic;

namespace Course.Core.Services.Interfaces
{
    public interface IRoleService
    {
        #region Roles

        List<Role> GetRoles();

        void AddRolesToUser(List<int> roleIds, int userId);

        #endregion
    }
}
