using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CatchUp_server.Models.UserModels
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
    }
}
