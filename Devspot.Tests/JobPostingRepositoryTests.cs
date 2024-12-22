using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSpot.Data;
using DevSpot.Models;
using DevSpot.Repositories;
using DevSpot.Services;
using Microsoft.EntityFrameworkCore;

namespace Devspot.Tests
{
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


        }

    }
}
