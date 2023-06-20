using blog.web.Models.Domain;

namespace blog.web.Models.ViewModels
{
    public class HomeViewModel
    {

        public IEnumerable<BlogPost> BlogPosts { get; set; }


        public IEnumerable<Tag> Tags { get; set; }


    }
}
