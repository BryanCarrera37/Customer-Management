using Customer_Management.Web.Contracts;

namespace Customer_Management.Web.Models.Entities
{
    public class EventsForCustomers : ICustomerEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime ActionDate { get; set; } = DateTime.Now;
        public string Action { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
    }
}
