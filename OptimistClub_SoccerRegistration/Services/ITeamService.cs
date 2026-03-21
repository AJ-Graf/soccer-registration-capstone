using OptimistClub_SoccerRegistration.Data.Models;

namespace OptimistClub_SoccerRegistration.Services
{
    public interface ITeamService
    {
        Task<List<Team>> GetAllTeamsAsync();
        Task<Team?> GetTeamByIdAsync(int id);
        Task<Team?> GetTeamWithMembersAsync(int id);
        Task<Team> CreateTeamAsync(Team team);
        Task<Team> UpdateTeamAsync(Team team);
        Task DeleteTeamAsync(int id);
        Task AddPlayerToTeamAsync(int teamId, int playerId);
        Task AddVolunteerToTeamAsync(int teamId, int volunteerId, string role);
        Task RemoveTeamMemberAsync(int teamMemberId);
        Task<List<Player>> GetUnassignedPlayersAsync();
        Task<List<Volunteer>> GetUnassignedVolunteersAsync();
    }
}