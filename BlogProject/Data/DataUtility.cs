using BlogProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BlogProject.Data
{
    public static class DataUtility
    {
        private const string _adminEmail = "evandaniel@me.com";
        private const string _modEmail = "moderator@Mailinator.com";
        private const string _adminRole = "Administrator";
        private const string _modRole = "Moderator";

        public static DateTime GetPostGresDate(DateTime datetime)
        {
            return DateTime.SpecifyKind(datetime, DateTimeKind.Utc);
        }

        public static string GetConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection"); //Local Connection string
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL"); //Remote Connection String
            return string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);
        }

        private static string BuildConnectionString(string databaseUrl)
        {
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Require,
                TrustServerCertificate = true

            };
            return builder.ToString();



        }


        public static async Task SeedDataAsync(IServiceProvider svcProvider)
        {
            //service: an instance of RoleManager
            var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>(); 

            var configurationSvc = svcProvider.GetRequiredService<IConfiguration>();

            //Service: An instance of RoleManager
            var roleManagerSvc = svcProvider.GetRequiredService<RoleManager<IdentityRole>>();

            //Service: an instance of the UserManager
            var userManagerSvc = svcProvider.GetRequiredService<UserManager<BlogUser>>();
            //Migration: = t update-database
            await dbContextSvc.Database.MigrateAsync();

            //seed role
            await SeedRolesAsync(roleManagerSvc);

            //seed users
            await SeedUsersAsync(dbContextSvc, configurationSvc, userManagerSvc);

        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(_adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(_adminRole));
            }

            if (!await roleManager.RoleExistsAsync(_modRole))
            {
                await roleManager.CreateAsync(new IdentityRole(_modRole));
            }
        }

        private static async Task SeedUsersAsync(ApplicationDbContext context, IConfiguration configuration, UserManager<BlogUser> userManager)
        {

            if (!context.Users.Any(u=> u.Email == _adminEmail))
            {
                BlogUser adminUser = new()
                {
                Email = _adminEmail,
                UserName = _adminEmail,
                FirstName = "Evan",
                LastName = "LaVack",
                PhoneNumber = "336-831-7101",
                EmailConfirmed = true
                };

                await userManager.CreateAsync(adminUser, configuration["AdminPwd"] ?? Environment.GetEnvironmentVariable("AdminPwd"));
                await userManager.AddToRoleAsync(adminUser, _adminRole);

            }

            if (!context.Users.Any(u => u.Email == _modEmail))
            {
                BlogUser modUser = new()
                {
                    Email = _modEmail,
                    UserName = _modEmail,
                    FirstName = "E",
                    LastName = "Moderator",
                    PhoneNumber = "336-274-8596",
                    EmailConfirmed = true
                };


                try
                {

                await userManager.CreateAsync(modUser, configuration["ModeratorPwd"] ?? Environment.GetEnvironmentVariable("ModeratorPwd"));
                await userManager.AddToRoleAsync(modUser, _modRole);
                }
                catch(Exception ex)
                {
                    var err = ex.Message;
                    throw;
                }
            }

        }

    }
}
