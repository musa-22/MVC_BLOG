using blog.web.Data;
using blog.web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace blog.web.Repositories
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly BloggieDbContext bloggieDbContext;


        public BlogPostCommentRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
        {
            await bloggieDbContext.BlogPostComments.AddAsync(blogPostComment);

            await bloggieDbContext.SaveChangesAsync();

            return blogPostComment; 
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommentByBlogIdAsync(Guid blogPostId)
        {
          var retrieveComment =  await bloggieDbContext.BlogPostComments.Where(x=>x.BlogPostId == blogPostId).ToListAsync();

            return retrieveComment;
        }
    }
}
