using Microsoft.EntityFrameworkCore;
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

        public async Task<Parent> CreateParentAsync(Parent parent)
        {
            _context.Parents.Add(parent);
            await _context.SaveChangesAsync();
            return parent;
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
            var today = DateTime.Today;
            return await _context.RegistrationPeriods
                .Where(rp => rp.IsActive
                    && (!rp.RegistrationOpenDate.HasValue || rp.RegistrationOpenDate.Value.Date <= today)
                    && (!rp.RegistrationCloseDate.HasValue || rp.RegistrationCloseDate.Value.Date >= today))
                .FirstOrDefaultAsync();
        }

        public async Task<RegistrationPeriod?> GetActiveSeasonAsync()
        {
            return await _context.RegistrationPeriods
                .Where(rp => rp.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> HasAnyRegistrationPeriodsAsync()
        {
            return await _context.RegistrationPeriods.AnyAsync();
        }

        public async Task<List<Registration>> GetAllRegistrationsAsync()
        {
            return await _context.Registrations
                .Include(r => r.Player)
                    .ThenInclude(p => p.Parent)
                .Include(r => r.RegistrationPeriod)
                .OrderByDescending(r => r.RegisteredAt)
                .ToListAsync();
        }

        public async Task<List<Player>> GetAllPlayersAsync()
        {
            return await _context.Players
                .Include(p => p.Parent)
                .OrderByDescending(p => p.DateAdded)
                .ToListAsync();
        }

        public async Task<List<Parent>> GetAllParentsAsync()
        {
            return await _context.Parents
                .Include(p => p.Players)
                .OrderByDescending(p => p.DateAdded)
                .ToListAsync();
        }

        public async Task<List<Volunteer>> GetAllVolunteersAsync()
        {
            return await _context.Volunteers
                .OrderByDescending(v => v.DateAdded)
                .ToListAsync();
        }

        public async Task<Player?> GetPlayerByIdAsync(int id)
        {
            return await _context.Players
                .Include(p => p.Parent)
                .FirstOrDefaultAsync(p => p.PlayerId == id);
        }

        public async Task<Parent?> GetParentByIdAsync(int id)
        {
            return await _context.Parents.FindAsync(id);
        }

        public async Task<Volunteer?> GetVolunteerByIdAsync(int id)
        {
            return await _context.Volunteers.FindAsync(id);
        }


        public async Task<Player> UpdatePlayerAsync(Player player)
        {
            _context.Players.Update(player);
            await _context.SaveChangesAsync();
            return player;
        }

        public async Task<Parent> UpdateParentAsync(Parent parent)
        {
            _context.Parents.Update(parent);
            await _context.SaveChangesAsync();
            return parent;
        }

        public async Task<Volunteer> UpdateVolunteerAsync(Volunteer volunteer)
        {
            _context.Volunteers.Update(volunteer);
            await _context.SaveChangesAsync();
            return volunteer;
        }

        public async Task UpdatePaymentStatusAsync(int registrationId, string status)
        {
            var reg = await _context.Registrations.FindAsync(registrationId);
            if (reg != null)
            {
                reg.PaymentStatus = status;
                await _context.SaveChangesAsync();
            }
        }


        public async Task DeletePlayerAsync(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player != null)
            {
                var registrations = await _context.Registrations
                    .Where(r => r.PlayerId == id)
                    .ToListAsync();
                _context.Registrations.RemoveRange(registrations);

                var memberships = await _context.TeamMembers
                    .Where(tm => tm.PlayerId == id)
                    .ToListAsync();
                _context.TeamMembers.RemoveRange(memberships);

                _context.Players.Remove(player);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteParentAsync(int id)
        {
            var parent = await _context.Parents.FindAsync(id);
            if (parent != null)
            {
                var hasPlayers = await _context.Players.AnyAsync(p => p.ParentId == id);
                if (!hasPlayers)
                {
                    _context.Parents.Remove(parent);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteVolunteerAsync(int id)
        {
            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer != null)
            {
                var memberships = await _context.TeamMembers
                    .Where(tm => tm.VolunteerId == id)
                    .ToListAsync();
                _context.TeamMembers.RemoveRange(memberships);

                _context.Volunteers.Remove(volunteer);
                await _context.SaveChangesAsync();
            }
        }
    }
}