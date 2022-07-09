using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgoUserApp.Data
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ApplicationUser()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
