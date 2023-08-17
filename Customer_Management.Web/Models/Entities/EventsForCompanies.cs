using Customer_Management.Web.Contracts;

namespace Customer_Management.Web.Models.Entities
{
    public class EventsForCompanies : ICompanyEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime ActionDate { get; set; } = DateTime.Now;
        public string Action { get; set; } = string.Empty;
        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = null!;
    }
}
