namespace Invoicer.DTOs
{
    public class CustomerDto
    {
        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
    }
}
