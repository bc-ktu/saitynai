using api.Models;

namespace api.DTOs
{
    // prideti ID
    public class CommentDto
    {
        public DateTime DateCreated { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public bool isFeatured { get; set; }
        public bool isDeleted { get; set; }
        public string AuthorId { get; set; } 
        public int Id { get; set; } 
    }
    public class CommentWithAuthorDto
    {
        public DateTime DateCreated { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public bool isFeatured { get; set; }
        public bool isDeleted { get; set; }
        public string AuthorId { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public int Id { get; set; }
    }
    public class CreateCommentDto
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public bool isFeatured { get; set; }
    }

    public class UpdateCommentDto
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public bool isFeatured { get; set; }
    }

}