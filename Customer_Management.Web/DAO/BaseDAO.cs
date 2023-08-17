using Customer_Management.Web.Classes.Exception;
using Customer_Management.Web.Data;
using Customer_Management.Web.Helpers;
using Customer_Management.Web.StaticValues;
using Microsoft.EntityFrameworkCore;

namespace Customer_Management.Web.DAO
{
    public abstract class BaseDAO
    {
        protected DbCustomerManagement? dbContext;
        protected ILogger? logger;

        protected virtual void InitializeDbContext()
        {
            dbContext = new DbCustomerManagement(GetDbContextOptions());
        }

        protected async Task SaveChangesOrThrowExceptionIsNecessary()
        {
            if (await dbContext!.SaveChangesAsync() == 0)
            {
                logger!.LogError("Could not save changes with the ORM");
                throw new CustomHttpException(StatusCodes.Status500InternalServerError, new CustomHttpExceptionBody(DefaultServerMessage.InternalServerError));
            }
        }

        protected abstract void InitializeLogger();

        private static DbContextOptions<DbCustomerManagement> GetDbContextOptions()
        {
            var appSettingsHelper = new AppSettingsHelper();
            return new DbContextOptionsBuilder<DbCustomerManagement>()
                .UseSqlServer(appSettingsHelper.GetConnectionString(DefinedValuesForConnectionStrings.KeyForDbCustomerManagement)).Options;
        }
    }
}
