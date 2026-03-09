using OptimistClub_SoccerRegistration.Data.Models;

namespace OptimistClub_SoccerRegistration.Services
{
    public interface IVolunteerService
    {
        Task<List<Volunteer>> GetAllVolunteersAsync();
        Task<Volunteer?> GetVolunteerByIdAsync(int id);
        Task<List<Volunteer>> GetVolunteersByRoleAsync(string role);
        Task<Volunteer> UpdateVolunteerAsync(Volunteer volunteer);
        Task DeleteVolunteerAsync(int id);
    }
}