namespace CustomerLeadApi.DTOs
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? ReferralSource { get; set; }
        public decimal? Price { get; set; }
        public int? ContactFrequency { get; set; }
        public DateTime? StartDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public int? EstimatedDuration { get; set; }
        public bool IsLead { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<CustomerImageDto> Images { get; set; } = new();
    }

    public class CreateCustomerDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? ReferralSource { get; set; }
        public decimal? Price { get; set; }
        public int? ContactFrequency { get; set; }
        public DateTime? StartDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public int? EstimatedDuration { get; set; }
        public bool IsLead { get; set; } = true;
    }

    public class CustomerImageDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string ImageData { get; set; } = string.Empty;
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public DateTime UploadedAt { get; set; }
    }

    public class UploadImageDto
    {
        public string ImageData { get; set; } = string.Empty; // Base64 string
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
    }

    public class UploadImagesDto
    {
        public List<UploadImageDto> Images { get; set; } = new();
    }
}