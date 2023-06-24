using blog.web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace blog.web.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AuthDbContext _authDbContext;

        public UsersRepository(AuthDbContext authDbContext)
        {
            this._authDbContext = authDbContext;
        }



        public async    Task<IEnumerable<IdentityUser>> GetAll()
        {

            // fetch all data in db
         var users =  await _authDbContext.Users.ToListAsync();

            var superAdminUsers = await _authDbContext.Users.FirstOrDefaultAsync( x => x.Email == "superadmin@gmail.com");
            
            if (superAdminUsers != null )
            {
                users.Remove(superAdminUsers);
            }
            return users;
        }
    }
}
