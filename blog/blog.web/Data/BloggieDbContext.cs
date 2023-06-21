using blog.web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace blog.web.Data
{
    public class BloggieDbContext: DbContext
    {
        public BloggieDbContext(DbContextOptions<BloggieDbContext> options) : base(options) 
        {
        
        
        }

        public DbSet<BlogPost> BlogPosts { get; set; }

        public DbSet<Tag> Tags { get; set; }
    }

}
