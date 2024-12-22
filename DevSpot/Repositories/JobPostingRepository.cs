using DevSpot.Data;
using DevSpot.Models;
using Microsoft.EntityFrameworkCore;

namespace DevSpot.Repositories
{
    public class JobPostingRepository : Repository<JobPosting>
    {
        private readonly ApplicationDbContext _context;

        public JobPostingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // Add any custom methods for JobPosting-specific operations
        public async Task<IEnumerable<JobPosting>> GetRecentJobPostings(int days)
        {
            var recentDate = DateTime.Now.AddDays(-days);
            return await _context.JobPostings
                .Where(j => j.PostedDate >= recentDate)
                .ToListAsync();
        }
    }
}
