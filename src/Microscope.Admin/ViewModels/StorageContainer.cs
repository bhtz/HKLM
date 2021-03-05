using System.ComponentModel.DataAnnotations;

namespace Microscope.Admin.ViewModels
{
    public class StorageContainer
    {
        private string _Name;
        
        [Required]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value.ToLower(); }
        }
    }
}
