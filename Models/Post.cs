using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Post içeriği boş olamaz.")]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Yazar bilgisi için IdentityUser ile ilişki
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}