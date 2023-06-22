
using blog.web.Data;
using blog.web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace blog.web.Repositories
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostLikeRepository(BloggieDbContext bloggieDbContext)
        {
          this.bloggieDbContext = bloggieDbContext;

        }

        public  async Task<BlogPostLiks> AddLikeForBlog(BlogPostLiks blogPostLike)

        {
            await bloggieDbContext.BlogPostsLiks.AddAsync(blogPostLike);
            await bloggieDbContext.SaveChangesAsync();
            return blogPostLike;


        }

        public async Task<IEnumerable<BlogPostLiks>> GetLikesForBlog(Guid blogPostId)
        {
            return await bloggieDbContext.BlogPostsLiks.Where(x=>x.BlogPostId == blogPostId).ToListAsync();
        }




        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
          var countLikes  = await bloggieDbContext.BlogPostsLiks.CountAsync(x=>x.BlogPostId == blogPostId);

            return countLikes;
        }
    }
}
