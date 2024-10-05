using CatchUp_server.Models.UserContent;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatchUp_server.Models.UserModels
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }

        public string? Bio { get; set; }
        public string ProfilePicUrl { get; set; }
        public string CoverPicUrl { get; set; }
        public DateTime RegisteredAt { get; set; }

        //public ICollection<Post> Posts { get; set; }
        //public ICollection<Story> Stories { get; set; }
    }
}
