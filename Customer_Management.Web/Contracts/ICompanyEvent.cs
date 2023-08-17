using Customer_Management.Web.Models.Entities;

namespace Customer_Management.Web.Contracts
{
    public interface ICompanyEvent : IEntityEvent
    {
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
