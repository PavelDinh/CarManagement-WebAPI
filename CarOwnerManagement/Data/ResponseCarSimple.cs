namespace CarOwnerManagement.Data
{
    public record ResponseCarSimple
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
