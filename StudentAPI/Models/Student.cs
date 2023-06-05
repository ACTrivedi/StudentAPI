using System;
using System.Collections.Generic;

namespace StudentAPI.Models
{
    public partial class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; } = null!;
        public long? ContactNumber { get; set; }
        public int? Age { get; set; }
    }
}
