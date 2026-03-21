using OptimistClub_SoccerRegistration.Data.Models;

namespace OptimistClub_SoccerRegistration.Services
{
    public interface IRegistrationService
    {
        Task<Player> CreatePlayerAsync(Player player);
        Task<Parent> CreateParentAsync(Parent parent);
        Task<Volunteer> CreateVolunteerAsync(Volunteer volunteer);
        Task<Registration> CreateRegistrationAsync(Registration registration);

        Task<RegistrationPeriod?> GetActiveRegistrationPeriodAsync();
        Task<List<Registration>> GetAllRegistrationsAsync();
        Task<List<Player>> GetAllPlayersAsync();
        Task<List<Parent>> GetAllParentsAsync();
        Task<List<Volunteer>> GetAllVolunteersAsync();
        Task<Player?> GetPlayerByIdAsync(int id);
        Task<Parent?> GetParentByIdAsync(int id);
        Task<Volunteer?> GetVolunteerByIdAsync(int id);

        Task<Player> UpdatePlayerAsync(Player player);
        Task<Parent> UpdateParentAsync(Parent parent);
        Task<Volunteer> UpdateVolunteerAsync(Volunteer volunteer);
        Task UpdatePaymentStatusAsync(int registrationId, string status);

        Task DeletePlayerAsync(int id);
        Task DeleteParentAsync(int id);
        Task DeleteVolunteerAsync(int id);
    }
}