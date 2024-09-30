using System.Collections.Generic;
using System.Linq;
using WebsiteBuilderApi.Data;
using WebsiteBuilderApi.Models;

namespace WebsiteBuilderApi.Services
{
    public class TemplateService
    {
        private readonly ApplicationDbContext _context;

        public TemplateService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Template> GetTemplates()
        {
            return _context.Templates.ToList();
        }
    }
}
