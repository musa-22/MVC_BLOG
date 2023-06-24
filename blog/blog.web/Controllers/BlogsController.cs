using blog.web.Models.Domain;
using blog.web.Models.ViewModels;
using blog.web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Schema;

namespace blog.web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly   IBlogPostRepository blogPostRepository;

        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        private readonly IBlogPostCommentRepository blogPostCommentRepository;

        public BlogsController(IBlogPostRepository blogPostRepository, IBlogPostLikeRepository blogPostLikeRepository,
            SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,IBlogPostCommentRepository blogPostCommentRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.blogPostCommentRepository = blogPostCommentRepository;
        }

        [HttpGet]
        public async Task< IActionResult> Index( string urlHandle)
        {
            var liked = false;

           var blogPost =  await blogPostRepository.GetByUrlHandleAsync(urlHandle);

            var blogDetailsViewModel = new BlogDetailsViewModel();

            if (blogPost != null)
            {
              var totalNumberOfLikes = await blogPostLikeRepository.GetTotalLikes(blogPost.Id);

                if(signInManager.IsSignedIn(User))
                {
                    // check of logged user check the post or not 
                    var likesForBlogs = await blogPostLikeRepository.GetLikesForBlog(blogPost.Id);

                    var userId = userManager.GetUserId(User);
                    
                    if (userId != null)
                    {
                        var userLike = likesForBlogs.FirstOrDefault(x => x.UserId == Guid.Parse(userId));

                        liked= userLike != null;

                    }
                }


                // get comment 
                var blogcommentDomainModel = await blogPostCommentRepository.GetCommentByBlogIdAsync(blogPost.Id);

                var blogCommentsForView = new List<BlogComment>();

                foreach (var blogComment in blogcommentDomainModel)
                {
                    blogCommentsForView.Add(new BlogComment
                    {
                       
                        Description = blogComment.Description,
                        DatedAdded = blogComment.DateAdded,
                        UserName = (await userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName,
                    });
                }

                 blogDetailsViewModel = new BlogDetailsViewModel
                {
                    Id = blogPost.Id,
                    Content = blogPost.Content,
                    PageTitle = blogPost.PageTitle,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Heading = blogPost.Heading,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    UrlHandle = blogPost.UrlHandle,
                    Visible = blogPost.Visible,
                    Tags = blogPost.Tags,
                    TotalLikes = totalNumberOfLikes,
                    Liked = liked,
                    Comments = blogCommentsForView,
                };

            }

            return View(blogDetailsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
        {

            if (signInManager.IsSignedIn(User))
            {
                var domainModel = new BlogPostComment
                {
                    BlogPostId = blogDetailsViewModel.Id,
                    Description = blogDetailsViewModel.CommentDescrtpion,
                    UserId = Guid.Parse(userManager.GetUserId(User)),
                    DateAdded = DateTime.Now
                };
               
              await blogPostCommentRepository.AddAsync(domainModel);

                return RedirectToAction("Index", "Blogs", new { urlHandle = blogDetailsViewModel.UrlHandle });
            }

            return View();
           
        }

    }
}
