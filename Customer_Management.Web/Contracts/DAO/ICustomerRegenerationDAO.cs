using Customer_Management.Web.DTO.Customer;

namespace Customer_Management.Web.Contracts.DAO
{
    public interface ICustomerRegenerationDAO
    {
        Task<CustomerDTO> Regenerate(CustomerRegenerationDTO regenerationDto, Guid id);
    }
}
