using System;

namespace EduSync.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty; // Fix: Initialize with a default value
        public string Email { get; set; } = string.Empty; // Fix: Initialize with a default value
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>(); // Fix: Initialize with a default value
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>(); // Fix: Initialize with a default value
        public string Role { get; set; } = string.Empty; // Fix: Initialize with a default value
    }
}