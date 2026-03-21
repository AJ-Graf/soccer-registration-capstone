using Microsoft.EntityFrameworkCore;
using OptimistClub_SoccerRegistration.Data;
using OptimistClub_SoccerRegistration.Data.Models;

namespace OptimistClub_SoccerRegistration.Services
{
    public class TeamService : ITeamService
    {
        private readonly ApplicationDbContext _context;

        public TeamService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Team>> GetAllTeamsAsync()
        {
            return await _context.Teams
                .Include(t => t.TeamMembers)
                    .ThenInclude(tm => tm.Player)
                .Include(t => t.TeamMembers)
                    .ThenInclude(tm => tm.Volunteer)
                .OrderBy(t => t.TeamName)
                .ToListAsync();
        }

        public async Task<Team?> GetTeamByIdAsync(int id)
        {
            return await _context.Teams.FindAsync(id);
        }

        public async Task<Team?> GetTeamWithMembersAsync(int id)
        {
            return await _context.Teams
                .Include(t => t.TeamMembers)
                    .ThenInclude(tm => tm.Player)
                .Include(t => t.TeamMembers)
                    .ThenInclude(tm => tm.Volunteer)
                .FirstOrDefaultAsync(t => t.TeamId == id);
        }

        public async Task<Team> CreateTeamAsync(Team team)
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            return team;
        }

        public async Task<Team> UpdateTeamAsync(Team team)
        {
            _context.Teams.Update(team);
            await _context.SaveChangesAsync();
            return team;
        }

        public async Task DeleteTeamAsync(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team != null)
            {
                _context.Teams.Remove(team);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddPlayerToTeamAsync(int teamId, int playerId)
        {
            var exists = await _context.TeamMembers
                .AnyAsync(tm => tm.TeamId == teamId && tm.PlayerId == playerId);

            if (!exists)
            {
                var member = new TeamMember
                {
                    TeamId = teamId,
                    PlayerId = playerId,
                    Role = "Player",
                    AssignedDate = DateTime.UtcNow
                };
                _context.TeamMembers.Add(member);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddVolunteerToTeamAsync(int teamId, int volunteerId, string role)
        {
            var exists = await _context.TeamMembers
                .AnyAsync(tm => tm.TeamId == teamId && tm.VolunteerId == volunteerId);

            if (!exists)
            {
                var member = new TeamMember
                {
                    TeamId = teamId,
                    VolunteerId = volunteerId,
                    Role = role,
                    AssignedDate = DateTime.UtcNow
                };
                _context.TeamMembers.Add(member);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveTeamMemberAsync(int teamMemberId)
        {
            var member = await _context.TeamMembers.FindAsync(teamMemberId);
            if (member != null)
            {
                _context.TeamMembers.Remove(member);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Player>> GetUnassignedPlayersAsync()
        {
            var assignedPlayerIds = await _context.TeamMembers
                .Where(tm => tm.PlayerId != null)
                .Select(tm => tm.PlayerId)
                .ToListAsync();

            return await _context.Players
                .Where(p => !assignedPlayerIds.Contains(p.PlayerId))
                .OrderBy(p => p.LastName)
                .ToListAsync();
        }

        public async Task<List<Volunteer>> GetUnassignedVolunteersAsync()
        {
            var assignedVolunteerIds = await _context.TeamMembers
                .Where(tm => tm.VolunteerId != null)
                .Select(tm => tm.VolunteerId)
                .ToListAsync();

            return await _context.Volunteers
                .Where(v => !assignedVolunteerIds.Contains(v.VolunteerId))
                .OrderBy(v => v.LastName)
                .ToListAsync();
        }
    }
}