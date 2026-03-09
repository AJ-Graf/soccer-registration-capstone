using Microsoft.EntityFrameworkCore;
using OptimistClub_SoccerRegistration.Data;
using OptimistClub_SoccerRegistration.Data.Models;

namespace OptimistClub_SoccerRegistration.Services
{
    public class VolunteerService : IVolunteerService
    {
        private readonly ApplicationDbContext _context;

        public VolunteerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Volunteer>> GetAllVolunteersAsync()
        {
            return await _context.Volunteers
                .Include(v => v.Guardian)
                .ToListAsync();
        }

        public async Task<Volunteer?> GetVolunteerByIdAsync(int id)
        {
            return await _context.Volunteers
                .Include(v => v.Guardian)
                .FirstOrDefaultAsync(v => v.VolunteerId == id);
        }

        public async Task<List<Volunteer>> GetVolunteersByRoleAsync(string role)
        {
            return await _context.Volunteers
                .Include(v => v.Guardian)
                .Where(v => v.Role == role)
                .ToListAsync();
        }

        public async Task<Volunteer> UpdateVolunteerAsync(Volunteer volunteer)
        {
            _context.Volunteers.Update(volunteer);
            await _context.SaveChangesAsync();
            return volunteer;
        }

        public async Task DeleteVolunteerAsync(int id)
        {
            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer != null)
            {
                _context.Volunteers.Remove(volunteer);
                await _context.SaveChangesAsync();
            }
        }
    }
}