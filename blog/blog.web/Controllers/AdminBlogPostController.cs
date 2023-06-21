
using blog.web.Models.Domain;
using blog.web.Models.ViewModels;
using blog.web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace blog.web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBlogPostController : Controller
    {

        private readonly ITagRepository tagRepository;

        private readonly IBlogPostRepository _blogPostRepository;

        public AdminBlogPostController( ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;

            this._blogPostRepository = blogPostRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Add()
        {
            // get tag for repository
        var tags = await tagRepository.GetAllAsync();

            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            //map view model to domain model

            var blogPost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible,
            };



            // map tags from selected tags
            var seletcdTempTagToholdData = new List<Tag>();
            foreach (var selectedTagId in addBlogPostRequest.SeletcedTags)
            {

                var selectedTagIdGuid = Guid.Parse(selectedTagId);

                var exsitingTag = await tagRepository.GetAsync(selectedTagIdGuid);

                if (exsitingTag != null)
                {
                    seletcdTempTagToholdData.Add(exsitingTag);
                }
            }

            // maping tags back to domain model
            blogPost.Tags = seletcdTempTagToholdData;


            await _blogPostRepository.AddAsync(blogPost);



            return RedirectToAction("Add");
        }


        [HttpGet]
        public async Task<IActionResult> List()
        {

            // call repository
           var post =  await _blogPostRepository.GetAllAsync();

            return View(post);
        }



        [HttpGet]
        public async Task< IActionResult> Edit(Guid id) 
        {
            // first we need to reterive data from repository 

           var blogPost = await _blogPostRepository.GetAsync(id);

           // var tags = new LinkedList<Tag>();

            var tagsDomainModel = await tagRepository.GetAllAsync();  


            if (blogPost != null)
            {
                // map domain model into view model 
                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    ShortDescription = blogPost.ShortDescription,
                    PublishedDate = blogPost.PublishedDate,
                    Visible = blogPost.Visible,
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),

                    SeletcedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray(),

                };

                return View(model);
            }

           


            return View(null);
        
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            // map view model back to domain model 

            var blogDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                Author = editBlogPostRequest.Author,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                ShortDescription = editBlogPostRequest.ShortDescription,
                PublishedDate = editBlogPostRequest.PublishedDate,
                Visible = editBlogPostRequest.Visible,

            };

            // map tags into domain model 

            var selectedTags = new List<Tag>();
            
            foreach(var selectedTag in editBlogPostRequest.SeletcedTags)
            
            { 
            if(Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);


                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            
            }
            
            blogDomainModel.Tags = selectedTags;    

            // submit inforamtion to repository
            var updateBlog = await _blogPostRepository.UpdateAsync(blogDomainModel);

            if (updateBlog != null)
            {
                // show success notification
                return RedirectToAction("Edit");
            }
            // show error notification
            return RedirectToAction("Edit");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            // send req to repositor to delete the post
            var deleteBlogPost = await _blogPostRepository.DeleteAsync(editBlogPostRequest.Id);

            if (deleteBlogPost != null)
            {
                // show success notif
                return RedirectToAction("List");

            }

            // show error notif

            return RedirectToAction("Edit", new {id = editBlogPostRequest.Id});
        }
    }
}
