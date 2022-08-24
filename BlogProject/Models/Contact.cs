using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        public string? Email { get; set; }


        public string? Subject { get; set; }

        public string? Message { get; set; }

        

        



    }
}
