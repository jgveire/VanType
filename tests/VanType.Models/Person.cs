using System;

namespace VanType.Models
{
    public class Person
    {
        public string FullName { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; } = DateTime.Now;
    }
}
