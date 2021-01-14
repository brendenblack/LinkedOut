using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkedOut.Blazor.Data
{
    public class UserSettingDbContext : DbContext
    {
        public DbSet<UserSettings> UserSettings { get; set; }
    }
}
