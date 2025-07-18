﻿namespace DevLifePortal.Domain.Entities
{
    public class User
    {
        public int Id { get;set; }
        public string Username { get; set; } = string.Empty;
        public string FirstName { get;set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public string TechStack { get; set; } = string.Empty;
        public string ExperienceLevel { get; set; } = string.Empty;
        public string ZodiacSign { get; set; } = string.Empty;

        public CodeCasinoProfile CodeCasinoProfile { get; set; }
        public BugChaseProfile BugChaseProfile { get; set; }
        public DevDatingProfile DevDatingProfile { get; set; }
    }
}
