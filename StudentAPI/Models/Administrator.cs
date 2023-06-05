using System;
using System.Collections.Generic;

namespace StudentAPI.Models
{
    public partial class Administrator
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? EmailAddress { get; set; }
        public string? Role { get; set; }
    }
}
