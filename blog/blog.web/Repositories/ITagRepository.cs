using blog.web.Models.Domain;

namespace blog.web.Repositories
{
    public interface ITagRepository
    {

  
         Task< IEnumerable <Tag>> GetAllAsync();

        // get single data
        Task<Tag?> GetAsync(Guid id);



        Task<Tag> AddAsync(Tag tag);


        Task<Tag?> UpdateAsync(Tag tag);


        Task<Tag?> DeleteAsync(Guid id);


    }
}
