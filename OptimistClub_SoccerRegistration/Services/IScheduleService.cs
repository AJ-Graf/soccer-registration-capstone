using OptimistClub_SoccerRegistration.Data.Models;

namespace OptimistClub_SoccerRegistration.Services
{
    public interface IScheduleService
    {
        Task<List<Schedule>> GetAllSchedulesAsync();
        Task<List<Schedule>> GetSchedulesByTeamAsync(int teamId);
        Task<Schedule?> GetScheduleByIdAsync(int id);
        Task<Schedule> CreateScheduleAsync(Schedule schedule);
        Task<Schedule> UpdateScheduleAsync(Schedule schedule);
        Task DeleteScheduleAsync(int id);
    }
}