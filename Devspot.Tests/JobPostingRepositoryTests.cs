using System;
using System.Threading.Tasks;
using DevSpot.Data;
using DevSpot.Models;
using DevSpot.Repositories;
using DevSpot.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Devspot.Tests
{
    public class JobPostingRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly ApplicationDbContext _context;
        private readonly JobPostingRepository _repository;
        private readonly JobPostingService _service;

        public JobPostingRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(_options);
            _repository = new JobPostingRepository(_context);
            _service = new JobPostingService(_repository);
        }

        [Fact]
        public async Task AddJobPostingAsync_ShouldAddJobPosting()
        {
            // Arrange
            var jobPosting = new JobPosting
            {
                Title = "Software Developer",
                Description = "C# Developer position",
                Company = "Tech Corp",
                Location = "Remote",
                UserId = "test-user-id",
                IsApproved = false
            };

            // Act
            await _service.AddJobPostingAsync(jobPosting);

            // Assert
            var savedJob = await _context.JobPostings.FirstOrDefaultAsync(j => j.Title == "Software Developer");
            Assert.NotNull(savedJob);
            Assert.Equal("Software Developer", savedJob.Title);
            Assert.Equal("Tech Corp", savedJob.Company);
            Assert.Equal("Remote", savedJob.Location);
            Assert.Equal("test-user-id", savedJob.UserId);
            Assert.False(savedJob.IsApproved);
            Assert.True(savedJob.PostedDate <= DateTime.UtcNow);
        }

        [Fact]
        public async Task AddJobPostingAsync_ShouldSetDefaultValues()
        {
            // Arrange
            var jobPosting = new JobPosting
            {
                Title = "Test Job",
                Description = "Test Description",
                Company = "Test Company",
                Location = "Test Location",
                UserId = "test-user-id"
            };

            // Act
            await _service.AddJobPostingAsync(jobPosting);

            // Assert
            var savedJob = await _context.JobPostings.FirstOrDefaultAsync(j => j.Title == "Test Job");
            Assert.NotNull(savedJob);
            Assert.False(savedJob.IsApproved);
            Assert.True((DateTime.UtcNow - savedJob.PostedDate).TotalMinutes < 1);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}