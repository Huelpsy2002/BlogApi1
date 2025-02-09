using BlogApi.Data;
using BlogApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace BlogApi.BussinssLogic
{

    public interface ICommentsLogic
    {
        public Task<List<Comments>> GetAllComments(int blogId);
        public Task<(bool success, List<string> Errors)> AddComment(AddCommentsDto addCommentdto, string username, int blogId);
        public Task<(bool success, List<string> Errors)> updateComment(updateCommentsDto updatecommentdto, string username, int blogId);
        public  Task<getCommentsDto> getComment(string username, int blogId);
        public Task<bool> deleteComment(string username, int blogId);


    }



    public class CommentsLogic : ICommentsLogic
    {
        private readonly AppDbContext _context;
        public CommentsLogic(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Comments>> GetAllComments(int blogId)
        {
            var comments = await _context.comments.Where(c => c.BlogId == blogId).AsNoTracking().ToListAsync();
            if (comments == null)
            {
                return null;
            }
            return comments;

        }
        public async Task<(bool success, List<string> Errors)> AddComment(AddCommentsDto addCommentdto, string username,int blogId)
        {
            Comments comment = new Comments { text = addCommentdto.text };
            CommentsValidation commentsValidation = new CommentsValidation(comment);
            if (string.IsNullOrWhiteSpace(username)) // this to fully make sure user is logged in and this if should never be true
            {
                commentsValidation.errors.Add("user does not exist");
            }
            var user = await _context.users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
            var blog = await _context.blogs.AsNoTracking().FirstOrDefaultAsync(b => b.id == blogId);
            if (blog == null)
            {
                return (false, new List<string> { $"blog with id {blogId} does not exist" });
            }
            if (commentsValidation.Validate().Count > 0)
            {
                return (false, commentsValidation.errors);
            }
            comment.UserId = user.Id;
            comment.BlogId = blogId;
            await _context.AddAsync(comment);
            await _context.SaveChangesAsync();
            return (true, new List<string>());

        }
        public async Task<(bool success, List<string> Errors)> updateComment(updateCommentsDto updatecommentdto, string username,int commentId)
        {
            Comments comment = await _context.comments.FirstOrDefaultAsync(c => c.id == commentId);
            Users user = await _context.users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);

            if (comment == null)
            {
                return (false, new List<string> { $"comment with id {comment} does not exist" });
            }

            if (updatecommentdto.text != null)
            {
                comment.text = updatecommentdto.text;
            }
           

            CommentsValidation commentsValidation = new CommentsValidation(comment);

            if (comment.UserId != user.Id)
            {
                commentsValidation.errors.Add("unauthorized to update other users comments");
            }


            if (commentsValidation.Validate().Count > 0)
            {
                return (false, commentsValidation.errors);
            }
            comment.updatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return (true, new List<string>());


        }
        public async Task<getCommentsDto> getComment(string username, int blogId)
        {
            var comment = await _context.comments
                        .Include(c => c.Users) // Include user details
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.Users.Username == username && c.BlogId == blogId);
            if (comment == null)
            {
                return null;
            }
            return new getCommentsDto {commentId = comment.id ,text = comment.text,  createdAt = comment.createdAt, updatedAt = comment.updatedAt,userId = comment.UserId,blogId=comment.BlogId };

        }
        public async Task<bool> deleteComment(string username, int commmentId)
        {
            var comment = await _context.comments
                                    .Include(c=>c.Users)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(c => c.Users.Username == username && c.id == commmentId);
            if (comment == null)
            {
                return false;
            }

            _context.comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;

        }

    }
}
