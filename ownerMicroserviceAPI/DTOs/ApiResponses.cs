namespace ownerMicroservice.DTOs
{
    public class CreateOwnerDto
    {
        public string Name { get; set; } = string.Empty;
        public string FirstLastname { get; set; } = string.Empty;
        public string? SecondLastname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string? DocumentExtension { get; set; }
        public string Address { get; set; } = string.Empty;
    }

    public class UpdateOwnerDto
    {
        public string Name { get; set; } = string.Empty;
        public string FirstLastname { get; set; } = string.Empty;
        public string? SecondLastname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string? DocumentExtension { get; set; }
        public string Address { get; set; } = string.Empty;
    }

    public class ValidationErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
    }

    public class SuccessResponse
    {
        public string Message { get; set; } = string.Empty;
        public int Id { get; set; }
    }
}
