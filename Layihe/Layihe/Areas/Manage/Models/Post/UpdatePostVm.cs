using Layihe.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Layihe.Areas.Manage.Models.Post
{
    public class UpdatePostVm:BaseAudiTableEntity
    {
        public IFormFile? Image { get; set; }
        public string? ImgUrl { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "Maxsimum 25 karakter daxil ede bilersiniz!")]
        [MinLength(4, ErrorMessage = "Minimum 4 karakter daxil ede bilersiz")]
        public string Title { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "Maxsimum 25 karakter daxil ede bilersiniz!")]
        [MinLength(4, ErrorMessage = "Minimum 4 karakter daxil ede bilersiz")]
        public string Author { get; set; }
        [Required]
        [MinLength(4, ErrorMessage = "Minimum 4 karakter daxil ede bilersiz")]
        public string Description { get; set; }
    }
}
