using System.ComponentModel.DataAnnotations;

namespace Microscope.Admin.ViewModels
{
    public class StorageContainer
    {
        [Required]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        public string Name { get; set; }
    }
}
