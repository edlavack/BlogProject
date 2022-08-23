using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogProject.Models
{
    public class Category
    {
        //Primary Key
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and a max {1} characters long", MinimumLength = 2)]
        public string? Name { get; set; }


        
        [StringLength(2000, ErrorMessage = "The {0} must be at least {2} and a max {1} characters long", MinimumLength = 2)]
        public string? Description { get; set; }


        //property for storing images
        public byte[]? ImageData { get; set; }
        public string? ImageType { get; set; }


        //property for passing the file imfo from the form to the post.
        //Also not saved in the database via [NotMapped] attribute.
        [NotMapped]
        public IFormFile? CategoryImage { get; set; }

        
        


        //Navigation Properties 
        public virtual ICollection<BlogPost> BlogPosts { get; set; } = new HashSet<BlogPost>();
    }
}
