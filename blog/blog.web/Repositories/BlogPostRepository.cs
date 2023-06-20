using blog.web.Data;
using blog.web.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization;

namespace blog.web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {

        private readonly BloggieDbContext bloggieDbContext;
       

        public BlogPostRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;

 
            

        }

        public async  Task<BlogPost> AddAsync(BlogPost blogPost)
        {
          await bloggieDbContext.AddAsync(blogPost);
            await bloggieDbContext.SaveChangesAsync();

            return blogPost;
            
        }

        // reterive all data from db
        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
           var getAlllData = await bloggieDbContext.BlogPosts.Include(x =>x.Tags).ToListAsync();

            return getAlllData;
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
           return await bloggieDbContext.BlogPosts.Include(x =>x.Tags).FirstOrDefaultAsync(x => x.Id == id);

        }



        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
          var existingBlog =  await bloggieDbContext.BlogPosts.FindAsync(id);

            if (existingBlog != null)
            {
                bloggieDbContext.BlogPosts.Remove(existingBlog);
                await bloggieDbContext.SaveChangesAsync();  
                return existingBlog;
            }
            return null;
        }


        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlog = await bloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id ==blogPost.Id);

            if (existingBlog != null)
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.PageTitle  = blogPost.PageTitle;
                existingBlog.Content = blogPost.Content;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.Author = blogPost.Author;
                existingBlog.FeaturedImageUrl   = blogPost.FeaturedImageUrl;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.Visible = blogPost.Visible;
                existingBlog.PublishedDate   = blogPost.PublishedDate;
                existingBlog.Tags = blogPost.Tags;
                await bloggieDbContext.SaveChangesAsync();

                return existingBlog;
            }
            return null;
        }





        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await bloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(y=>y.UrlHandle == urlHandle);
            
        }
    }
}
