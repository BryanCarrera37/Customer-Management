using Customer_Management.Web.DTO.Company;

namespace Customer_Management.Web.Contracts.DAO
{
    public interface ICompanyDAO : IDAO<AvailableCompanyDTO, string, string>, ICompanyRegenerationDAO
    {
        Task<AvailableCompanyDTO?> GetByName(string name);
    }
}
