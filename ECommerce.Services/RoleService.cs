using ECommerce.Services.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IEnumerable<IdentityRole> GetAll()
        {
            return [.. _roleManager.Roles];
        }

        public IdentityRole? Get(string id)
        {
            return _roleManager.FindByIdAsync(id).Result;
        }

        public IdentityResult Create(IdentityRole role)
        {
            return _roleManager.CreateAsync(role).Result;
        }

        public IdentityResult Update(IdentityRole role)
        {
            var roleFromDb = _roleManager.FindByIdAsync(role.Id).Result;

            if (roleFromDb == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Role not found."
                });
            }

            roleFromDb.Name = role.Name;
            roleFromDb.NormalizedName = role.Name!.ToUpper();

            return _roleManager.UpdateAsync(roleFromDb).Result;
        }

        public IdentityResult Delete(string id)
        {
            var role = _roleManager.FindByIdAsync(id).Result;

            if (role == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Role not found."
                });
            }

            return _roleManager.DeleteAsync(role).Result;
        }
    }
}
