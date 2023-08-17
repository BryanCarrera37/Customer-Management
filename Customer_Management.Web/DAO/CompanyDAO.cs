using Customer_Management.Web.Classes.Exception;
using Customer_Management.Web.Contracts.DAO;
using Customer_Management.Web.DTO.Company;
using Customer_Management.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Customer_Management.Web.DAO
{
    public class CompanyDAO : BaseDAO, ICompanyDAO
    {
        public CompanyDAO()
        {
            InitializeDbContext();
            InitializeLogger();
        }

        public async Task<List<AvailableCompanyDTO>> GetAll()
        {
            return await dbContext!.Companies.Where(e => e.IsDeleted == false)
                .OrderByDescending(e => e.CreatedAt)
                .Select(item => ConvertModelToDTO(item)).ToListAsync();
        }

        public async Task<AvailableCompanyDTO> Get(Guid id)
        {
            try
            {
                var company = await GetCompanyById(id);
                return ConvertModelToDTO(company);
            }
            catch { throw; }
        }

        public async Task<AvailableCompanyDTO?> GetByName(string name)
        {
            try
            {
                var company = await GetCompanyByName(name);
                return ConvertModelToDTO(company);
            }
            catch { return null; }
        }

        public async Task<AvailableCompanyDTO> Save(string name)
        {
            var company = new Company() { Name = name.Trim().ToUpper() };
            dbContext!.Companies.Add(company);
            await SaveChangesOrThrowExceptionIsNecessary();
            return ConvertModelToDTO(company);
        }

        public async Task<AvailableCompanyDTO> Regenerate(Guid id)
        {
            try
            {
                var company = await GetCompanyById(id);
                company.IsDeleted = false;
                dbContext!.Update(company);
                await SaveChangesOrThrowExceptionIsNecessary();
                return ConvertModelToDTO(company);
            }
            catch { throw; }
        }

        public async Task<AvailableCompanyDTO> Update(string modificationValue, Guid id)
        {
            try
            {
                var savedCompany = await GetCompanyById(id);
                savedCompany.Name = modificationValue;
                dbContext!.Update(savedCompany);
                await SaveChangesOrThrowExceptionIsNecessary();
                return ConvertModelToDTO(savedCompany);
            }
            catch { throw; }
        }

        public async Task Delete(Guid id)
        {
            try
            {
                var company = await GetCompanyById(id);
                company.IsDeleted = true;
                dbContext!.Update(company);
                await SaveChangesOrThrowExceptionIsNecessary();
            }
            catch { throw; }
        }

        protected override void InitializeLogger()
        {
            logger = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            }).CreateLogger<CompanyDAO>();
        }

        private async Task<Company> GetCompanyById(Guid id)
        {
            var company = await dbContext!.Companies.FirstOrDefaultAsync(e => e.Id.Equals(id));
            if(company == null)
            {
                throw new CustomHttpException(StatusCodes.Status404NotFound, new CustomHttpExceptionBody("No se ha encontrado la compañía especificada"));
            }

            return company!;
        }

        private async Task<Company> GetCompanyByName(string name)
        {
            var company = await dbContext!.Companies.FirstOrDefaultAsync(e => e.Name.Trim().Equals(name.Trim().ToUpper()));
            if (company == null)
            {
                throw new CustomHttpException(StatusCodes.Status404NotFound, new CustomHttpExceptionBody("No se ha encontrado la compañía especificada"));
            }

            return company!;
        }

        private static AvailableCompanyDTO ConvertModelToDTO(Company model)
        {
            return new AvailableCompanyDTO
            {
                Id = model.Id,
                Name = model.Name,
                CreatedAt = model.CreatedAt
            };
        }
    }
}
