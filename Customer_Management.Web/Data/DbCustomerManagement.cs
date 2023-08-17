using Customer_Management.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Customer_Management.Web.Data
{
    public class DbCustomerManagement : DbContext
    {
        public DbCustomerManagement(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<EventsForCompanies> EventsForCompanies { get; set; }
        public virtual DbSet<EventsForCustomers> EventsForCustomers { get; set; }
    }
}
