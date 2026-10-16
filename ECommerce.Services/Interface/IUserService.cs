using ECommerce.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Services.Interface
{
    public interface IUserService
    {
        IEnumerable<ApplicationUser> GetAll();
        ApplicationUser? Get(string id);
        IdentityResult Create(ApplicationUser user, string password);
        IdentityResult Update(ApplicationUser user);
        IdentityResult Delete(string id);
        IList<string> GetRoles(ApplicationUser user);

        IdentityResult AssignRole(string userId, string roleName);

        IdentityResult RemoveRole(string userId, string roleName);
    }
}
