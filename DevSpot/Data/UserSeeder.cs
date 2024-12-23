using DevSpot.Constants;
using Microsoft.AspNetCore.Identity;

namespace DevSpot.Data
{
    public class UserSeeder
    {
        public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var logger = serviceProvider.GetRequiredService<ILogger<UserSeeder>>();

            try
            {
                await CreateUserWithRole(userManager, logger, "admin@devspot.com", "Admin123!", Roles.Admin);
                await CreateUserWithRole(userManager, logger, "employer@devspot.com", "Employer123!", Roles.Employer);
                await CreateUserWithRole(userManager, logger, "jobseeker@devspot.com", "JobSeeker123!", Roles.JobSeeker);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding users");
                throw;
            }
        }

        private static async Task CreateUserWithRole(
            UserManager<IdentityUser> userManager,
            ILogger logger,
            string email,
            string password,
            string role)
        {
            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                logger.LogInformation($"User {email} already exists");
                return;
            }

            var user = new IdentityUser
            {
                Email = email,
                EmailConfirmed = true,
                UserName = email,
                NormalizedEmail = email.ToUpper(),
                NormalizedUserName = email.ToUpper()
            };

            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                var roleResult = await userManager.AddToRoleAsync(user, role);
                if (roleResult.Succeeded)
                {
                    logger.LogInformation($"Created user {email} with role {role}");
                }
                else
                {
                    logger.LogError($"Failed to add role {role} to user {email}. Errors: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                    throw new Exception($"Failed to add role {role} to user {email}");
                }
            }
            else
            {
                logger.LogError($"Failed to create user {email}. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                throw new Exception($"Failed to create user {email}. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }
}
