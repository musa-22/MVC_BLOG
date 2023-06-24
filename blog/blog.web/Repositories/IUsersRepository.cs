using Microsoft.AspNetCore.Identity;

namespace blog.web.Repositories
{
    public interface IUsersRepository
    {

        Task<IEnumerable<IdentityUser>> GetAll();


    }
}
