using DevSpot.Models;
using DevSpot.Repositories;

namespace DevSpot.Services
{
    public class JobPostingService
    {
        private readonly IRepository<JobPosting> _repository;

        public JobPostingService(IRepository<JobPosting> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<JobPosting>> GetAllJobPostingsAsync() => await _repository.GetAllAsync();

        public async Task<JobPosting> GetJobPostingByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task AddJobPostingAsync(JobPosting jobPosting) => await _repository.AddAsync(jobPosting);

        public async Task UpdateJobPostingAsync(JobPosting jobPosting) => await _repository.UpdateAsync(jobPosting);

        public async Task DeleteJobPostingAsync(int id) => await _repository.DeleteAsync(id);

        public async Task<object?> GetJobPostingAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<object?> GetJobPostingAsync(Guid invalidId)
        {
            throw new NotImplementedException();
        }
    }

}
