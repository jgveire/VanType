namespace VanType.Models
{
    using System;

    public class TeamModelBase
    {
        public string Name { get; set; } = string.Empty;

        public string? Slogan { get; set; }

        public Guid? TeamCaptainId { get; set; }
    }
}
