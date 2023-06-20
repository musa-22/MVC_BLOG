using blog.web.Data;
using blog.web.Models.Domain;
using blog.web.Models.ViewModels;
using blog.web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace blog.web.Controllers
{
    public class AdminTagsController : Controller
    {


        private readonly ITagRepository tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {

            // var name = addTagRequest.Name;
            //var displayName = addTagRequest.DisplayName;

            // maping addtagrequest to tag domain model 
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName,
            };


           await tagRepository.AddAsync(tag);


            return RedirectToAction("List");
        }


        // read data from db
        [HttpGet]
        [ActionName("List")]
        public async Task< IActionResult> List()
        {

            // use dbcontext to read the tags
           var tags = await tagRepository.GetAllAsync();
            
            return View(tags);
        }

        
        
        
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id) 
        {
           
           var  tag = await tagRepository.GetAsync(id);

            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName,
                };
                return View(editTagRequest);
            }

            return View(null); 
        
        }

        [HttpPost]
        public async Task< IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName,

            };
       
           var updateTag = await tagRepository.UpdateAsync(tag);

            if (updateTag != null) 
            { 
              // show success messages

            } else
            {
                // show error messges 
            }


            // show error notification
            return RedirectToAction("Edit", new {id = editTagRequest.Id});
        }




        [HttpPost]
        public async Task< IActionResult> Delete(EditTagRequest editTagRequest)
        {
          
           var delete = await tagRepository.DeleteAsync(editTagRequest.Id);

            if (delete != null)
            {
                //show success 
            return RedirectToAction("List");
            }
           
            // show error notification
            return RedirectToAction("Edit" , new {id = editTagRequest.Id});

        }
    }
    
}
