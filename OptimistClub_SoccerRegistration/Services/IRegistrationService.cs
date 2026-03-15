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
    }
}
