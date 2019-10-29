using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Areas.Admin.Model
{
    public class RoleEditModel
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<AppUser> Members { get; set; }
        public IEnumerable<AppUser> NonMembers { get; set; }
    }
}