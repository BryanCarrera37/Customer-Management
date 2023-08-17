namespace Customer_Management.Web.Models.Entities
{
    public class Customer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Guid CompanyId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;

        public Company Company { get; set; } = null!;

        public ICollection<EventsForCustomers> EventsForCustomers { get; } = new List<EventsForCustomers>();
    }
}
