using Pustok_project.Contexts;
using Pustok_project.Models;

namespace Pustok_project.Helpers
{
    public class FooterServices
    {
        PustokDbContext _context { get; }

        public FooterServices(PustokDbContext context)
        {
            _context = context;
        }

        public async Task<Setting> GetSettingsAsync()
            => await _context.Settings.FindAsync(1);
    }
}
