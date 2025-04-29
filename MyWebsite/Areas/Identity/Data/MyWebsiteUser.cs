using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MyWebsite.Areas.Identity.Data;

// Add profile data for application users by adding properties to the MyWebsiteUser class
public class MyWebsiteUser : IdentityUser
{
    public string Name { get; set; }
}

