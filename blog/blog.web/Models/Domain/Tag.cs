namespace blog.web.Models.Domain
{
    public class Tag
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }


       // create relationship 1 Tag could have many relatioship

        public ICollection<BlogPost> BlogPosts { get; set; }

    }
}
