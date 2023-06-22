namespace blog.web.Models.Domain
{
    public class BlogPostLiks
    {
        public Guid Id { get; set; }

        public Guid BlogPostId { get; set; }

        public Guid UserId { get; set; }

    }
}
