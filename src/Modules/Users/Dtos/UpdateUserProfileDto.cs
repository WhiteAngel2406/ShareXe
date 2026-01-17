using System.ComponentModel.DataAnnotations;

namespace ShareXe.Modules.Users.Dtos
{
    public class UpdateUserProfileDto
    {
        [MinLength(2)]
        public string? FullName { get; set; }
    }
}
