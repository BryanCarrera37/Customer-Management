using Customer_Management.Web.Contracts.DAO;
using Customer_Management.Web.Models.Entities;

namespace Customer_Management.Web.DAO
{
    public class EventsForCompaniesDAO : BaseDAO, IEventRegisterDAO
    {
        public EventsForCompaniesDAO()
        {
            InitializeDbContext();
            InitializeLogger();
        }

        public async Task RegisterAction(Guid entityId, string action)
        {
            try
            {
                var entityEvent = GetEventToRegisterIt(entityId, action);
                dbContext!.EventsForCompanies.Add(entityEvent);
                await SaveChangesOrThrowExceptionIsNecessary();
            }
            catch {
                logger!.LogError($"Could not register the action '{action}' for the company with the ID {entityId}");
                throw;
            }
        }

        protected override void InitializeLogger()
        {
            logger = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            }).CreateLogger<EventsForCompaniesDAO>();
        }

        private static EventsForCompanies GetEventToRegisterIt(Guid companyId, string action) => new EventsForCompanies
        {
            CompanyId = companyId,
            Action = action
        };
    }
}
