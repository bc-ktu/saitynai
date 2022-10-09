using api.Models;

namespace api.DTOs
{
    public class CommentDto
    {
        public DateTime DateCreated { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public bool isFeatured { get; set; }
        public bool isDeleted { get; set; }
        public string Author { get; set; }
    }
    public class CreateCommentDto
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public bool isFeatured { get; set; }
        public string Author { get; set; }
    }

    public class UpdateCommentDto
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public bool isFeatured { get; set; }
    }

}