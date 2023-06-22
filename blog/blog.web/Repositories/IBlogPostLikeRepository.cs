using blog.web.Models.Domain;

namespace blog.web.Repositories
{
    public interface IBlogPostLikeRepository
    {

       Task<int> GetTotalLikes(Guid blogPostId);

        Task<IEnumerable<BlogPostLiks>> GetLikesForBlog(Guid blogPostId);

        Task<BlogPostLiks> AddLikeForBlog(BlogPostLiks blogPostId);




    }
}
