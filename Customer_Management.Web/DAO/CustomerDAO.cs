using Customer_Management.Web.Classes.Exception;
using Customer_Management.Web.Contracts.DAO;
using Customer_Management.Web.DTO.Customer;
using Customer_Management.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Customer_Management.Web.DAO
{
    public class CustomerDAO : BaseDAO, ICustomerDAO
    {
        public CustomerDAO()
        {
            InitializeDbContext();
            InitializeLogger();
        }

        public async Task<List<CustomerDTO>> GetAll()
        {
            return await (from customer in dbContext!.Customers
                          join company in dbContext!.Companies
                          on customer.CompanyId equals company.Id
                          where customer.IsDeleted == false
                          orderby customer.CreatedAt descending
                          select GetDtoFromRelation(customer, company)).ToListAsync();
        }

        public async Task<CustomerDTO> Get(Guid id)
        {
            var customer = await GetCustomerById(id);
            return GetDtoFromRelation(customer, await GetCompanyById(customer.CompanyId));
        }

        public async Task<CustomerDTO?> GetByEmail(string email)
        {
            try
            {
                var customer = await GetCustomerByEmail(email);
                return GetDtoFromRelation(customer, await GetCompanyById(customer.CompanyId));
            }
            catch { return null; }
        }

        public async Task<List<CustomerDTO>> GetCustomersLinkedToCompany(Guid companyId)
        {
            return await (from customer in dbContext!.Customers
                         join company in dbContext!.Companies
                         on customer.CompanyId equals company.Id
                         where customer.IsDeleted == false && company.Id == companyId
                         orderby customer.CreatedAt descending
                         select GetDtoFromRelation(customer, company)).ToListAsync();
        }

        public async Task<CustomerDTO> Save(CustomerDTO creationValue)
        {
            dbContext!.Customers.Add(GetModelFromDTO(creationValue));
            await SaveChangesOrThrowExceptionIsNecessary();
            return creationValue;
        }

        public async Task<CustomerDTO> Regenerate(CustomerRegenerationDTO dto, Guid id)
        {
            try
            {
                var customer = await GetCustomerWithModifiedPropsWhenRegenerating(dto, id);
                dbContext!.Customers.Update(customer);
                await SaveChangesOrThrowExceptionIsNecessary();
                return GetDtoFromRelation(customer, await GetCompanyById(customer.CompanyId));
            }
            catch { throw; }
        }

        public async Task<CustomerDTO> Update(CustomerModificationDTO modificationValue, Guid id)
        {
            try
            {
                var customer = await GetCustomerWithModifiedPropsWhenModifying(modificationValue, id);
                dbContext!.Customers.Update(customer);
                await SaveChangesOrThrowExceptionIsNecessary();
                return GetDtoFromRelation(customer, await GetCompanyById(customer.CompanyId));
            }
            catch { throw; }
        }

        public async Task Delete(Guid id)
        {
            try
            {
                var customer = await GetCustomerById(id);
                customer.IsDeleted = true;
                dbContext!.Update(customer);
                await SaveChangesOrThrowExceptionIsNecessary();
            }
            catch { throw; }
        }

        protected override void InitializeLogger()
        {
            logger = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            }).CreateLogger<CustomerDAO>();
        }

        private async Task<Customer> GetCustomerWithModifiedPropsWhenRegenerating(CustomerRegenerationDTO dto, Guid id)
        {
            try
            {
                var customer = await GetCustomerById(id);
                customer.FirstName = dto.FirstName.Trim().ToUpper();
                customer.LastName = dto.LastName.Trim().ToUpper();
                customer.Email = dto.Email.Trim().ToLower();
                customer.CompanyId = dto.CompanyId;
                customer.IsDeleted = false;
                return customer;
            }
            catch { throw; }
        }

        private async Task<Customer> GetCustomerWithModifiedPropsWhenModifying(CustomerModificationDTO dto, Guid id)
        {
            try
            {
                var customer = await GetCustomerById(id);
                customer.FirstName = dto.FirstName.Trim().ToUpper();
                customer.LastName = dto.LastName.Trim().ToUpper();
                customer.Email = dto.Email.Trim().ToLower();
                customer.CompanyId = dto.CompanyId;
                customer.Company = new Company
                {
                    Id = dto.CompanyId,
                    Name = dto.CompanyName
                };
                return customer;
            }
            catch { throw; };
        }

        private async Task<Customer> GetCustomerById(Guid id)
        {
            var customer = await dbContext!.Customers.FirstOrDefaultAsync(e => e.Id.Equals(id));
            if (customer == null)
            {
                throw new CustomHttpException(StatusCodes.Status404NotFound, new CustomHttpExceptionBody("El cliente no fue encontrado"));
            }

            return customer;
        }

        private async Task<Customer> GetCustomerByEmail(string email)
        {
            var customer = await dbContext!.Customers.FirstOrDefaultAsync(e => e.Email.Trim().ToLower().Equals(email.Trim().ToLower()));
            if(customer == null)
            {
                throw new CustomHttpException(StatusCodes.Status404NotFound, new CustomHttpExceptionBody("El cliente no fue encontrado"));
            }

            return customer;
        }

        private async Task<Company> GetCompanyById(Guid id)
        {
            return await dbContext!.Companies.FirstAsync(e => e.Id.Equals(id));
        }

        private static CustomerDTO GetDtoFromRelation(Customer customer, Company company) => new CustomerDTO
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            CompanyId = company.Id,
            CompanyName = company.Name,
            CreatedAt = customer.CreatedAt
        };

        private static Customer GetModelFromDTO(CustomerDTO dto) => new Customer
        {
            Id = dto.Id,
            FirstName = dto.FirstName.Trim().ToUpper(),
            LastName = dto.LastName.Trim().ToUpper(),
            Email = dto.Email.Trim().ToLower(),
            CompanyId = dto.CompanyId
        };
    }
}
