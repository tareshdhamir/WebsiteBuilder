using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using WebsiteBuilderApi.Models;

namespace WebsiteBuilderApi.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                // Check if the Templates table is empty
                if (context.Templates.Any())
                {
                    return;   // Database has been seeded
                }

                // Seed the Templates table
                context.Templates.AddRange(
                    new Template
                    {
                        TemplateName = "Template 1",
                        TemplatePath = "StaticTemplates/Template1/index.html",
                        ImageUrl = "/assets/template1.png"
                    },
                    new Template
                    {
                        TemplateName = "Template 2",
                        TemplatePath = "StaticTemplates/Template2/index.html",
                        ImageUrl = "/assets/template2.png"
                    }
                );

                context.SaveChanges();
            }
        }
    }
}
