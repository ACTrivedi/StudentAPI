using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ConsumeWebAPI.Models
{
    public class StudentViewModel
    {
        
        public int StudentId { get; set; }

        [DisplayName("Student Name")]
        [StringLength(10, ErrorMessage = "Your First Name can contain only 10 characters")]
        public string Name { get; set; } = null!;

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public long? ContactNumber { get; set; }

        public int? Age { get; set; }
    }
}
