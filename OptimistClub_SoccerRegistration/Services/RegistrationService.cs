using Microsoft.EntityFrameworkCore; // Aaron - this code is used to define how the methods from IRegistration work
using OptimistClub_SoccerRegistration.Data;
using OptimistClub_SoccerRegistration.Data.Models;

namespace OptimistClub_SoccerRegistration.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly ApplicationDbContext _context;

        public RegistrationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Player> CreatePlayerAsync(Player player)
        {
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            return player;
        }

        public async Task<Guardian> CreateGuardianAsync(Guardian guardian)
        {
            _context.Guardians.Add(guardian);
            await _context.SaveChangesAsync();
            return guardian;
        }

        public async Task<Volunteer> CreateVolunteerAsync(Volunteer volunteer)
        {
            _context.Volunteers.Add(volunteer);
            await _context.SaveChangesAsync();
            return volunteer;
        }

        public async Task<Registration> CreateRegistrationAsync(Registration registration)
        {
            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();
            return registration;
        }

        public async Task<RegistrationPeriod?> GetActiveRegistrationPeriodAsync()
        {
            return await _context.RegistrationPeriods
                .FirstOrDefaultAsync(p => p.IsActive);
        }

        public async Task<List<Registration>> GetAllRegistrationsAsync()
        {
            return await _context.Registrations
                .Include(r => r.Player)
                .Include(r => r.Period)
                .ToListAsync();
        }
    }
}