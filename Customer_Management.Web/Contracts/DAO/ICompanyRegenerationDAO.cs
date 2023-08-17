using Customer_Management.Web.DTO.Company;

namespace Customer_Management.Web.Contracts.DAO
{
    public interface ICompanyRegenerationDAO
    {
        Task<AvailableCompanyDTO> Regenerate(Guid id);
    }
}
