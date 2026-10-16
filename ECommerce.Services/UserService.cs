using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using ECommerce.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return [.. _userManager.Users];
        }

        public ApplicationUser? Get(string id)
        {
            return _userManager.FindByIdAsync(id).Result;
        }

        public IdentityResult Create(ApplicationUser user, string password)
        {
            IdentityResult result = _userManager.CreateAsync(user, password).Result;
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, "Customer").Wait();
            }

            return result;
        }

        public IdentityResult Update(ApplicationUser user)
        {
            var userFromDb = _userManager.FindByIdAsync(user.Id).Result;

            if (userFromDb == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "User not found."
                });
            }

            userFromDb.UserName = user.UserName;
            userFromDb.Email = user.Email;
            userFromDb.FullName = user.FullName;
            userFromDb.Address = user.Address;

            return _userManager.UpdateAsync(userFromDb).Result;
        }

        public IdentityResult Delete(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "User not found."
                });
            }

            return _userManager.DeleteAsync(user).Result;
        }

        public IList<string> GetRoles(ApplicationUser user)
        {
            return _userManager.GetRolesAsync(user).Result;
        }

        public IdentityResult AssignRole(string userId, string roleName)
        {
            var user = _userManager.FindByIdAsync(userId).Result;

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "User not found."
                });
            }

            return _userManager.AddToRoleAsync(user, roleName).Result;
        }

        public IdentityResult RemoveRole(string userId, string roleName)
        {
            var user = _userManager.FindByIdAsync(userId).Result;

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "User not found."
                });
            }

            return _userManager.RemoveFromRoleAsync(user, roleName).Result;
        }

    }
}
