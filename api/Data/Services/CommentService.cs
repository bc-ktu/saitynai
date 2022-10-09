using api.Entities;
using api.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace api.Data.Services
{
    public interface ICommentService
    {
        Task CreateComment(Comment newComment);
        Task DeleteComment(Comment comment);
        Task<List<Comment>> GetAllComments(int productId);
        Task<Comment?> GetComment(int productId, int commentId);
        Task UpdateComment(int productId, int id, Comment comment);
        Task DeleteComments(List<Comment> comments);
    }

    public class CommentService : ICommentService
    {
        private StoreContext context;

        public CommentService(StoreContext context)
        {
            this.context = context;
        }

        public async Task<List<Comment>> GetAllComments(int productId)
        {
            return await context.Comments.Where(c => c.ProductId == productId).ToListAsync();
        }

        public async Task<Comment?> GetComment(int productId, int commentId)
        {
            return await context.Comments.Where(p => p.Id == commentId && p.ProductId == productId).FirstOrDefaultAsync();
        }

        public async Task CreateComment(Comment newComment)
        {
            context.Comments.Add(newComment);
            await context.SaveChangesAsync();
        }

        public async Task DeleteComment(Comment comment)
        {
            context.Comments.Remove(comment);
            await context.SaveChangesAsync();
        }

        public async Task UpdateComment(int productId, int id, Comment comment)
        {
            context.Comments.Update(comment);
            await context.SaveChangesAsync();
        }

        public async Task DeleteComments(List<Comment> comments)
        {

            context.Comments.RemoveRange(comments);
            await context.SaveChangesAsync();
        }
    }
}
