﻿

using blog.web.Models.Domain;

namespace blog.web.Repositories
{
    public interface IBlogPostRepository
    {

        Task<IEnumerable<BlogPost>> GetAllAsync();

        Task<BlogPost?> GetAsync(Guid id);

        Task <BlogPost> AddAsync(BlogPost blogPost);

        Task<BlogPost> UpdateAsync(BlogPost blogPost);

        Task<BlogPost?> DeleteAsync(Guid id);


        Task<BlogPost?> GetByUrlHandleAsync(string urlHandle);

       
    }
}
