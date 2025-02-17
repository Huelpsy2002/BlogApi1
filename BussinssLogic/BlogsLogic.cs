using BlogApi.Data;
using BlogApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace BlogApi.BussinssLogic
{

    public interface IBlogsLogic
    {
        public Task<List<getALLBlogsDto>> GetAllBlogs(string? categoryFilter);
        public Task<(bool success, List<string> Errors)> AddBlog(AddBlogDto addblogdto, string username);
        public Task<(bool success, List<string> Errors)> UpdateBlog(updateBlogDto updateblogdto, string username,int blogId);
        public Task<getBlogDto> getBlog(string username, int blogId);
        public Task<bool> deleteBlog(string username, int blogId);
        

    }



    public class BlogsLogic:IBlogsLogic
    {
        private readonly AppDbContext _context;
        public BlogsLogic(AppDbContext context)
        {
            _context = context;
        }

        public async  Task<List<string>>CategoryIdValidation(List<int>categories)
        {

            List<string> errors = new List<string>();
            foreach (var categoryId in categories) { 
                if (!int.TryParse(categoryId.ToString(), out int validCategoryId))
                {
                    errors.Add("invalid Category Id");
                    return errors;
                }
                var categroy = await _context.categories.FirstOrDefaultAsync(c => c.id == categoryId);
                if (categroy == null)
                {
                    errors.Add($"category with id {categoryId} does not exist");
                    return errors;
                }
            }
            return errors;
        }

        public async Task<List<getALLBlogsDto>> GetAllBlogs(string? categoryFilter)
        {

          

            var blogs = await _context.blogs.Where(b=> categoryFilter==null||  b.BlogCategories.Any(bc=>bc.Categories.Name==categoryFilter)).Select(b => new getALLBlogsDto
            {
                Username = b.Users.Username,
                Title = b.Title,
                Content = b.Description,
                CreatedAt = b.createdAt,
                Catgeories = b.BlogCategories.Where(bc => bc.BlogId == b.id)
                .Select(bc => bc.Categories.Name).ToList(),
                Comments = b.Comments.Select(c => new CommentDto {

                    Username = c.Users.Username,
                    CommentText = c.text,
                    CommentedAt = c.createdAt,


                }).ToList(),




            })
                .AsNoTracking().ToListAsync();
            
            return blogs;

        }
        public async Task<(bool success, List<string> Errors)> AddBlog(AddBlogDto addblogdto,string username)
        {
            Blogs blog = new Blogs { Title = addblogdto.title, Description = addblogdto.description };
            BlogsValidation blogsValidation = new BlogsValidation(blog);
            if (string.IsNullOrWhiteSpace(username)) // this to fully make sure user is logged in and this if should never be true
            {
                blogsValidation.errors.Add("user does not exist");
            }
            var user = await _context.users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
            var categoriesErrors = await CategoryIdValidation(addblogdto.categories); //make sure categories id is valid 
            blogsValidation.errors.AddRange(categoriesErrors);
            if (blogsValidation.Validate().Count > 0)
            {
                return (false, blogsValidation.errors);
            }
            blog.UserId = user.Id;
            await _context.AddAsync(blog);
            await _context.SaveChangesAsync();
            
            var blogcategories = addblogdto.categories.Select(categoryId => new BlogsCategories { BlogId = blog.id, CategoryId = categoryId }).ToList();
            await _context.blogsCategories.AddRangeAsync(blogcategories);
            await _context.SaveChangesAsync();



            return (true, new List<string>());

        }
        public async Task<(bool success, List<string> Errors)> UpdateBlog(updateBlogDto updateblogdto, string username,int blogId)
        {
            Blogs blog = await _context.blogs.FirstOrDefaultAsync(b => b.id == blogId);
            Users user = await _context.users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);



            if (blog == null)
            {
                return (false, new List<string> {$"blog with id ${blogId} does not exist "});
            }
            if (updateblogdto.title != null)
            {
                blog.Title = updateblogdto.title;
            }
            if (updateblogdto.description != null)
            {
                blog.Description = updateblogdto.description;
            }
            
            BlogsValidation blogsValidation = new BlogsValidation(blog);
           

            if (blog.UserId != user.Id)
            {
                blogsValidation.errors.Add("unauthorized to update other users blogs");
            }
            if (blogsValidation.Validate().Count > 0)
            {
                return (false, blogsValidation.errors);
            }
            
            blog.UpdatedAt = DateTime.UtcNow;     
            await _context.SaveChangesAsync();
            return (true, new List<string>());


        }
        public async Task<getBlogDto> getBlog(string username,int blogId)
        {
            var blog = await _context.blogs
                        .Include(b => b.Users) // Include user details
                        .AsNoTracking()
                        .FirstOrDefaultAsync(b => b.Users.Username == username && b.id == blogId);
            if (blog == null)
            {
                return null;
            }
            return new getBlogDto { title = blog.Title, description = blog.Description, createdAt = blog.createdAt, updatedAt = blog.UpdatedAt };
            
        }
        public async Task<bool> deleteBlog(string username,int blogId)
        {
            var blog = await _context.blogs
                                    .Include(b => b.Users).Include(b=>b.Comments).Include(b=>b.BlogCategories)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(b => b.Users.Username == username && b.id == blogId);
            if (blog == null)
            {
                return false;
            }

            if(blog.BlogCategories!=null && blog.BlogCategories.Any()) { _context.blogsCategories.RemoveRange(blog.BlogCategories); }
            if (blog.Comments != null && blog.Comments.Any()) { _context.comments.RemoveRange(blog.Comments); }

            _context.blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return true;

        }

    }
}
