using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Services.Interface
{
    public interface IRoleService
    {
        IEnumerable<IdentityRole> GetAll();

        IdentityRole? Get(string id);

        IdentityResult Create(IdentityRole role);

        IdentityResult Update(IdentityRole role);

        IdentityResult Delete(string id);
    }
}
