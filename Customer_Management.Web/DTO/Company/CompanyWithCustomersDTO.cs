using Customer_Management.Web.DTO.Customer;

namespace Customer_Management.Web.DTO.Company
{
    public class CompanyWithCustomersDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public List<CustomerDTO> Customers { get; set; } = new List<CustomerDTO>();
    }
}
