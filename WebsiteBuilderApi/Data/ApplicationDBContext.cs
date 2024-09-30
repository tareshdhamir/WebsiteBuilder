using Microsoft.EntityFrameworkCore;
using WebsiteBuilderApi.Models;

namespace WebsiteBuilderApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Template> Templates { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
