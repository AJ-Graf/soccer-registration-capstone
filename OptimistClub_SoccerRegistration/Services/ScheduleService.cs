using Microsoft.EntityFrameworkCore;
using OptimistClub_SoccerRegistration.Data;
using OptimistClub_SoccerRegistration.Data.Models;

namespace OptimistClub_SoccerRegistration.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly ApplicationDbContext _context;

        public ScheduleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Schedule>> GetAllSchedulesAsync()
        {
            return await _context.Schedules
                .Include(s => s.Team)
                .OrderBy(s => s.GameDate)
                .ThenBy(s => s.GameTime)
                .ToListAsync();
        }

        public async Task<List<Schedule>> GetSchedulesByTeamAsync(int teamId)
        {
            return await _context.Schedules
                .Include(s => s.Team)
                .Where(s => s.TeamId == teamId)
                .OrderBy(s => s.GameDate)
                .ThenBy(s => s.GameTime)
                .ToListAsync();
        }

        public async Task<Schedule?> GetScheduleByIdAsync(int id)
        {
            return await _context.Schedules.FindAsync(id);
        }

        public async Task<Schedule> CreateScheduleAsync(Schedule schedule)
        {
            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();
            return schedule;
        }

        public async Task<Schedule> UpdateScheduleAsync(Schedule schedule)
        {
            var existing = await _context.Schedules.FindAsync(schedule.ScheduleId);
            if (existing == null) throw new Exception("Schedule not found");

            existing.TeamId = schedule.TeamId;
            existing.Location = schedule.Location;
            existing.GameDate = schedule.GameDate;
            existing.GameTime = schedule.GameTime;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task DeleteScheduleAsync(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedules.Remove(schedule);
                await _context.SaveChangesAsync();
            }
        }
    }
}