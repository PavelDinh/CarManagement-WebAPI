namespace CarOwnerManagement.Data
{
    public record ResponseOwner
    {
        public string Name { get; set; }

        public int CarCount { get; set; }
    }
}
