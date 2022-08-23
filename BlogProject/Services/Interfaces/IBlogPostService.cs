using BlogProject.Models;

namespace BlogProject.Services.Interfaces
{
    public interface IBlogPostService
    {
        public Task<bool> ValidateSlugAsync(string title, int blogId);


        public Task AddTagToBlogPostAsync(int tagId, int blogPostId);


        public Task<bool> IsTagOnBlogPostAsync(int tagId, int blogPostId);


        public Task RemoveTagFromBlogPostAsync(int tagId, int blogPostId);


        //New Methods 08/12/22
        public Task<List<Category>> GetCategoriesAsync(int count);

        public Task<List<BlogPost>> GetAllBlogPostsAsync(); //All posts regardless of IsDeleted and/or IsPublished

        public Task<List<BlogPost>> GetPopularBlogPostsAsync(int count); //defined by the number of comments made

        public Task<List<BlogPost>> GetRecentBlogPostsAsync(int count); //defined by the date created

        public Task<List<BlogPost>> GetBlogPostsInCategoryAsync(int categoryId);

        public IEnumerable<BlogPost> Search(string searchString);
    }



}
