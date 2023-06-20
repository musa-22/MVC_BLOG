using blog.web.Data;
using blog.web.Models.Domain;
using blog.web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace blog.web.Repositories
{
    public class TagRepository : ITagRepository
    {

        private readonly BloggieDbContext bloggieDbContext;

        public TagRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }


        public async Task<Tag> AddAsync(Tag tag)
        {
            await bloggieDbContext.Tags.AddAsync(tag);
            await bloggieDbContext.SaveChangesAsync();

            return tag;
        }

        public  async Task<Tag?> DeleteAsync(Guid id)
        {
           var ex = await bloggieDbContext.Tags.FindAsync(id);

            if (ex != null)
            {
                bloggieDbContext.Tags.Remove(ex);
                await bloggieDbContext.SaveChangesAsync();
                return ex;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
           var GetAllList = await bloggieDbContext.Tags.ToListAsync();

            return GetAllList;


        }

        public async  Task<Tag?> GetAsync(Guid id)
        {
          return await bloggieDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);    

        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existtingTage = await bloggieDbContext.Tags.FindAsync(tag.Id);

            if (existtingTage != null)
            { 
                existtingTage.Name = tag.Name;
                existtingTage.DisplayName = tag.DisplayName;
                await bloggieDbContext.SaveChangesAsync();

                return existtingTage;
            }


            return null;

        }

       
    }
}
