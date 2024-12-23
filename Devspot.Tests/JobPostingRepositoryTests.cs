using DevSpot.Data;
using DevSpot.Models;
using DevSpot.Repositories;
using DevSpot.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class JobPostingRepositoryTests : IDisposable
{
    private readonly DbContextOptions<ApplicationDbContext> _options;
    private readonly ApplicationDbContext _context;
    private readonly JobPostingService _service;

    public JobPostingRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"JobPostingDb_{Guid.NewGuid()}")
            .Options;

        _context = new ApplicationDbContext(_options);
        _service = new JobPostingService(new JobPostingRepository(_context));
    }

    private JobPosting CreateJobPosting(string title, string description, string company, string location, string userId) =>
        new JobPosting
        {
            Title = title,
            Description = description,
            Company = company,
            Location = location,
            UserId = userId
        };

    [Fact]
    public async Task AddJobPostingAsync_ShouldAddJobPosting()
    {
        var jobPosting = CreateJobPosting("Software Developer", "C# Developer position", "Tech Corp", "Remote", "test-user-id");

        await _service.AddJobPostingAsync(jobPosting);
        var result = await _context.JobPostings.FirstOrDefaultAsync(j => j.Title == "Software Developer");

        Assert.NotNull(result);
        Assert.Equal("Software Developer", result.Title);
        Assert.Equal("C# Developer position", result.Description);
        Assert.Equal("Tech Corp", result.Company);
        Assert.Equal("Remote", result.Location);
        Assert.Equal("test-user-id", result.UserId);
    }

    [Fact]
    public async Task GetAllJobPostingsAsync_ShouldReturnAllJobPostings()
    {
        var jobPostings = new List<JobPosting>
        {
            CreateJobPosting("Software Developer", "C# Developer position", "Tech Corp", "Remote", "test-user-id"),
            CreateJobPosting("Frontend Developer", "React Developer position", "Web Corp", "Remote", "test-user-id")
        };

        foreach (var jobPosting in jobPostings)
        {
            await _service.AddJobPostingAsync(jobPosting);
        }

        var result = await _service.GetAllJobPostingsAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetJobPostingByIdAsync_ShouldReturnJobPosting()
    {
        var jobPosting = CreateJobPosting("Software Developer", "C# Developer position", "Tech Corp", "Remote", "test-user-id");

        await _service.AddJobPostingAsync(jobPosting);
        var result = await _service.GetJobPostingByIdAsync(jobPosting.Id);

        Assert.NotNull(result);
        Assert.Equal("Software Developer", result.Title);
        Assert.Equal("C# Developer position", result.Description);
        Assert.Equal("Tech Corp", result.Company);
        Assert.Equal("Remote", result.Location);
        Assert.Equal("test-user-id", result.UserId);
    }

    [Fact]
    public async Task UpdateJobPostingAsync_ShouldUpdateJobPosting()
    {
        var jobPosting = CreateJobPosting("Software Developer", "C# Developer position", "Tech Corp", "Remote", "test-user-id");

        await _service.AddJobPostingAsync(jobPosting);

        jobPosting.Title = "Frontend Developer";
        jobPosting.Description = "React Developer position";
        jobPosting.Company = "Web Corp";
        jobPosting.Location = "Remote";
        await _service.UpdateJobPostingAsync(jobPosting);

        var result = await _context.JobPostings.FirstOrDefaultAsync(j => j.Title == "Frontend Developer");

        Assert.NotNull(result);
        Assert.Equal("Frontend Developer", result.Title);
        Assert.Equal("React Developer position", result.Description);
        Assert.Equal("Web Corp", result.Company);
        Assert.Equal("Remote", result.Location);
        Assert.Equal("test-user-id", result.UserId);
    }

    [Fact]
    public async Task DeleteJobPostingAsync_ShouldDeleteJobPosting()
    {
        var jobPosting = CreateJobPosting("Software Developer", "C# Developer position", "Tech Corp", "Remote", "test-user-id");

        await _service.AddJobPostingAsync(jobPosting);
        await _service.DeleteJobPostingAsync(jobPosting.Id);

        var result = await _context.JobPostings.FirstOrDefaultAsync(j => j.Title == "Software Developer");

        Assert.Null(result);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
