using Customer_Management.Web.Models.Entities;

namespace Customer_Management.Web.Contracts
{
    public interface ICustomerEvent : IEntityEvent
    {
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
