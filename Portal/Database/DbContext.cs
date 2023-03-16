using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Database
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Category> categories{ get; set; }
        public DbSet<News> news { get; set; }
        public DbSet<NewsConfig> newsConfigs { get; set; }
        public DbSet<RefershToken> refreshTokens { get; set; }
        public DbSet<Watched> watcheds { get; set; }
        public DbSet<SavedNews> saved { get; set; }
        public DbSet<Reaction> reaction { get; set; }


    }
}
