using Customer_Management.Web.DTO.Customer;

namespace Customer_Management.Web.Contracts.DAO
{
    public interface ICustomerDAO : IDAO<CustomerDTO, CustomerDTO, CustomerModificationDTO>, ICustomerRegenerationDAO
    {
        Task<CustomerDTO?> GetByEmail(string email);
        Task<List<CustomerDTO>> GetCustomersLinkedToCompany(Guid companyId);
    }
}
