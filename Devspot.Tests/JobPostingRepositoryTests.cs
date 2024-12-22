using DevSpot.Data;
using DevSpot.Models;
using DevSpot.Repositories;
using DevSpot.Services;
using Microsoft.EntityFrameworkCore;

public class JobPostingRepositoryTests
{
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public JobPostingRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("JobPostingDb")
            .Options;
    }

    private ApplicationDbContext CreateDbContext() => new ApplicationDbContext(_options);

    [Fact]
    public async Task AddJobPostingAsync_ShouldAddJobPosting()
    {
        // Arrange
        // db context
        using var context = CreateDbContext();

        // job posting repository
        var repository = new JobPostingRepository(context);

        // job posting service
        var service = new JobPostingService(repository);

        // execute 
        var jobPosting = new JobPosting
        {
            Title = "Software Developer",
            Description = "C# Developer position",
            Company = "Tech Corp",
            Location = "Remote",
            UserId = "test-user-id"
        };

        await service.AddJobPostingAsync(jobPosting);

        // result
        var result = await context.JobPostings.FirstOrDefaultAsync(j => j.Title == "Software Developer");

        // assert
        Assert.NotNull(result);
        Assert.Equal("Software Developer", result.Title);
        Assert.Equal("C# Developer position", result.Description);
        Assert.Equal("Tech Corp", result.Company);
        Assert.Equal("Remote", result.Location);
        Assert.Equal("test-user-id", result.UserId);
    }
}