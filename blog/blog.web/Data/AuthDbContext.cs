using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace blog.web.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // seed roles ,users , admin , superadmin

            var adminRoleId = "83e0ca5e-32b7-42e6-9ddf-6f23c81b5f24";

            var superAdminRoleId = "f8c879c0-187e-4958-b513-51a67ccb5b0c";

            var userId = "09ee885b-70df-4dab-82df-8e0dd1dd42c3";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {


                Name = "Admin",
                NormalizedName = "Admin",
                Id = adminRoleId,
                ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Name= "SuperAdmin",
                    NormalizedName= "SuperAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                },

                new IdentityRole
                {
                    Name= "User",
                    NormalizedName= "User",

                    Id = userId,
                    ConcurrencyStamp = userId
                 
                },
            };


            // pass the seed to build in method call Has thr builder
            // seed super-admin-users
            builder.Entity<IdentityRole>().HasData(roles);

            var superAdminId = "068d34c2-9ea8-4483-9a2c-e60370dd87b6";
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@gmail.com",
                Email = "superadmin@gmail.com",
                NormalizedEmail = "superadmin@gmail.com".ToUpper(),
                NormalizedUserName = "superadmin@gmail.com".ToUpper(),
                Id = superAdminId,
            };
            //  hardcoding To generate password 
            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser,"musa@1234");

            builder.Entity<IdentityUser>().HasData(superAdminUser);

            // add all roles to super-admin-user
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = superAdminId
                },

                 new IdentityUserRole<string>
                {
                    RoleId = superAdminRoleId ,
                    UserId = superAdminId
                },

                  new IdentityUserRole<string>
                {
                    RoleId = userId ,
                    UserId = superAdminId
                },

            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}
